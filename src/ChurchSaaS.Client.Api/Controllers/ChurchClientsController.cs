using ChurchSaaS.Client.Application.Commands.ChurchClients;
using ChurchSaaS.Client.Api.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchSaaS.Client.Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/public/[controller]")]
public sealed class ChurchClientsController : ControllerBase
{
    private readonly ISender _sender;

    public ChurchClientsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateChurchClientRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateChurchClientCommand(
            request.LegalName,
            request.TradeName,
            request.MainContactName,
            request.MainContactEmail,
            request.MainContactPhone,
            request.Document,
            request.PlanCode,
            request.TrialEndsAt,
            request.Address is null
                ? null
                : new CreateChurchClientAddress(
                    request.Address.Logradouro,
                    request.Address.Numero,
                    request.Address.Complemento,
                    request.Address.Bairro,
                    request.Address.Cep,
                    request.Address.CidadeId),
            request.RequestedBy);

        var result = await _sender.Send(command, cancellationToken);

        if (result.Success)
            return CreatedAtAction(nameof(Create), new { id = result.Data }, result);

        return BadRequest(result);
    }
}
