using ChurchSaaS.Client.Api.Endpoints;
using ChurchSaaS.Client.Application.Commands.ChurchUnits;
using ChurchSaaS.Client.Domain.Churches;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace ChurchSaaS.Client.Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public sealed class ChurchUnitsController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IValidator<CreateChurchUnitCommand> _validator;

    public ChurchUnitsController(ISender sender, IValidator<CreateChurchUnitCommand> validator)
    {
        _sender = sender;
        _validator = validator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateChurchUnitRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateChurchUnitCommand(
            request.TenantId,
            request.LegalName,
            request.TradeName,
            request.MainContactName,
            request.MainContactEmail,
            request.MainContactPhone,
            request.Document,
            ChurchUnitType.Sede,
            request.Code,
            request.Address is null
                ? null
                : new CreateChurchUnitAddress(
                    request.Address.Logradouro,
                    request.Address.Numero,
                    request.Address.Complemento,
                    request.Address.Bairro,
                    request.Address.Cep,
                    request.Address.CidadeId),
            request.CreatedByUserId,
            new CreateChurchUnitUser(
                request.User.Email,
                request.User.Password,
                request.User.DisplayName,
                "ChurchAdmin",
                request.User.PersonId));

        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(new
            {
                Message = "Validation failed",
                Errors = validationResult.Errors.Select(e => e.ErrorMessage)
            });

        var result = await _sender.Send(command, cancellationToken);

        if (result.Success)
            return CreatedAtAction(nameof(Create), new { id = result.Data!.UnitId }, result.Data);

        return BadRequest(new
        {
            result.Message,
            result.Errors
        });
    }
}
