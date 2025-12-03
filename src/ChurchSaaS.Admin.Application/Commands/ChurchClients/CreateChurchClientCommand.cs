using ChurchSaaS.Admin.Application.Abstractions;
using MediatR;

namespace ChurchSaaS.Admin.Application.Commands.ChurchClients;

public sealed record CreateChurchClientCommand(
    string LegalName,
    string? TradeName,
    string MainContactName,
    string MainContactEmail,
    string? MainContactPhone,
    string? Document,
    string PlanCode,
    DateTimeOffset? TrialEndsAt,
    CreateChurchClientAddress? Address,
    string RequestedBy) : IRequest<Result<Guid>>;
