using ChurchSaaS.Client.Application.Abstractions;
using ChurchSaaS.Client.Application.Interfaces.Repositories;
using ChurchSaaS.Client.Application.Interfaces.Services;
using ChurchSaaS.Client.Domain.Abstractions;
using ChurchSaaS.Client.Domain.Churches;
using ChurchSaaS.Client.Domain.Entities;
using ChurchSaaS.Client.Domain.ValueObjects;
using MediatR;

namespace ChurchSaaS.Client.Application.Commands.ChurchUnits;

public sealed class CreateChurchUnitCommandHandler : IRequestHandler<CreateChurchUnitCommand, Result<CreateChurchUnitResult>>
{
    private readonly IChurchUnitRepository _churchUnitRepository;
    private readonly IUserProvisioningService _userProvisioningService;

    public CreateChurchUnitCommandHandler(
        IChurchUnitRepository churchUnitRepository,
        IUserProvisioningService userProvisioningService)
    {
        _churchUnitRepository = churchUnitRepository;
        _userProvisioningService = userProvisioningService;
    }

    public async Task<Result<CreateChurchUnitResult>> Handle(CreateChurchUnitCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var tenantId = new TenantId(request.TenantId);

            var email = Email.Create(request.MainContactEmail);
            var phone = string.IsNullOrWhiteSpace(request.MainContactPhone) ? null : PhoneNumber.Create(request.MainContactPhone);
            var document = string.IsNullOrWhiteSpace(request.Document) ? null : Document.Create(request.Document!);
            var address = request.Address is null
                ? null
                : Address.Create(
                    request.Address.Logradouro,
                    request.Address.Numero,
                    request.Address.Complemento,
                    request.Address.Bairro,
                    request.Address.Cep,
                    request.Address.CidadeId);

            ChurchUnit churchUnit = ChurchUnit.CreateRoot(
                tenantId,
                request.LegalName,
                request.MainContactName,
                email,
                request.Type,
                request.CreatedByUserId,
                request.TradeName,
                document,
                phone,
                address,
                request.Code);

            await _churchUnitRepository.AddAsync(churchUnit, cancellationToken);

            var userResult = await _userProvisioningService.CreateUserAsync(
                new CreateUserRequest(
                    request.User.Email,
                    request.User.Password,
                    request.User.DisplayName,
                    request.User.Role,
                    request.TenantId,
                    request.User.PersonId),
                cancellationToken);

            if (!userResult.Success)
                return Result<CreateChurchUnitResult>.Fail(userResult.Message ?? "Failed to create user", userResult.Errors);

            await _churchUnitRepository.SaveChangesAsync(cancellationToken);

            return Result<CreateChurchUnitResult>.Ok(new CreateChurchUnitResult(churchUnit.Id, userResult.Data!));
        }
        catch (Exception ex)
        {
            return Result<CreateChurchUnitResult>.Fail("Unexpected error while creating church unit.", new[] { ex.Message });
        }
    }
}
