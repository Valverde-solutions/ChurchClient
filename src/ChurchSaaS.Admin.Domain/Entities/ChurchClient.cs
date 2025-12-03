using ChurchSaaS.Admin.Domain.Abstractions;
using ChurchSaaS.Admin.Domain.Churches;
using ChurchSaaS.Admin.Domain.ValueObjects;
using Address = ChurchSaaS.Admin.Domain.Entities.Endereco;

namespace ChurchSaaS.Admin.Domain.Entities;

public sealed class ChurchClient : AuditableEntity<Guid>, IAggregateRoot
{
    // Identidade de negócio
    public string LegalName { get; private set; }        // Razão social / nome oficial
    public string? TradeName { get; private set; }       // Nome fantasia / como aparece na UI

    // Documento e contato principal (Value Objects)
    public Document? Document { get; private set; }      // CNPJ/CPF/Outro
    public string MainContactName { get; private set; }
    public Email MainContactEmail { get; private set; }
    public PhoneNumber? MainContactPhone { get; private set; }

    // Endereço (Value Object já existente no domínio)
    public Address? Address { get; private set; }

    // Plano / assinatura (lado Admin)
    public string PlanCode { get; private set; }         // ex: BASIC, PRO, ENTERPRISE
    public DateTimeOffset? TrialEndsAt { get; private set; }
    public DateTimeOffset? ActivatedAt { get; private set; }
    public DateTimeOffset? CancelledAt { get; private set; }

    // Integração com o Produto (tenant)
    public string? ProductTenantId { get; private set; } // Id/slug usado pelo sistema Product
    public ChurchClientStatus Status { get; private set; }

    // Construtor privado para ORM/serialização
    private ChurchClient() { }

    private ChurchClient(
        Guid id,
        string legalName,
        string? tradeName,
        string mainContactName,
        Email mainContactEmail,
        PhoneNumber? mainContactPhone,
        Document? document,
        string planCode,
        DateTimeOffset? trialEndsAt,
        Address? address)
        : base(id)
    {
        LegalName = legalName;
        TradeName = string.IsNullOrWhiteSpace(tradeName) ? legalName : tradeName;

        MainContactName = mainContactName;
        MainContactEmail = mainContactEmail;
        MainContactPhone = mainContactPhone;

        Document = document;

        PlanCode = planCode;
        TrialEndsAt = trialEndsAt;
        Address = address;

        Status = ChurchClientStatus.PendingProvisioning;
    }

    public static ChurchClient Create(
        string legalName,
        string? tradeName,
        string mainContactName,
        string mainContactEmail,
        string? mainContactPhone,
        string? document,
        string planCode,
        DateTimeOffset? trialEndsAt,
        string createdByUserId,
        Address? address = null)
    {
        if (string.IsNullOrWhiteSpace(legalName))
            throw new ArgumentException("Legal name is required.", nameof(legalName));

        if (string.IsNullOrWhiteSpace(mainContactName))
            throw new ArgumentException("Main contact name is required.", nameof(mainContactName));

        if (string.IsNullOrWhiteSpace(mainContactEmail))
            throw new ArgumentException("Main contact email is required.", nameof(mainContactEmail));

        if (string.IsNullOrWhiteSpace(planCode))
            throw new ArgumentException("Plan code is required.", nameof(planCode));

        var emailVo = Email.Create(mainContactEmail);
        PhoneNumber? phoneVo = null;
        if (!string.IsNullOrWhiteSpace(mainContactPhone))
        {
            phoneVo = PhoneNumber.Create(mainContactPhone);
        }

        Document? documentVo = null;
        if (!string.IsNullOrWhiteSpace(document))
        {
            documentVo = Document.Create(document);
        }

        var client = new ChurchClient(
            Guid.NewGuid(),
            legalName,
            tradeName,
            mainContactName,
            emailVo,
            phoneVo,
            documentVo,
            planCode,
            trialEndsAt,
            address);

        client.SetCreated(createdByUserId);

        return client;
    }

    public void UpdateBasicInfo(
        string legalName,
        string? tradeName,
        string mainContactName,
        string mainContactEmail,
        string? mainContactPhone,
        string? document,
        string updatedByUserId)
    {
        if (IsDeleted)
            throw new InvalidOperationException("Cannot update a deleted church client.");

        if (string.IsNullOrWhiteSpace(legalName))
            throw new ArgumentException("Legal name is required.", nameof(legalName));

        if (string.IsNullOrWhiteSpace(mainContactName))
            throw new ArgumentException("Main contact name is required.", nameof(mainContactName));

        if (string.IsNullOrWhiteSpace(mainContactEmail))
            throw new ArgumentException("Main contact email is required.", nameof(mainContactEmail));

        LegalName = legalName;
        TradeName = string.IsNullOrWhiteSpace(tradeName) ? legalName : tradeName;

        MainContactName = mainContactName;
        MainContactEmail = Email.Create(mainContactEmail);
        MainContactPhone = string.IsNullOrWhiteSpace(mainContactPhone)
            ? null
            : PhoneNumber.Create(mainContactPhone);

        Document = string.IsNullOrWhiteSpace(document)
            ? null
            : Document.Create(document);

        SetUpdated(updatedByUserId);
    }

    public void SetAddress(Address address, string updatedByUserId)
    {
        if (IsDeleted)
            throw new InvalidOperationException("Cannot update address of a deleted church client.");

        Address = address ?? throw new ArgumentNullException(nameof(address));

        SetUpdated(updatedByUserId);
    }

    public void ClearAddress(string updatedByUserId)
    {
        if (IsDeleted)
            throw new InvalidOperationException("Cannot clear address of a deleted church client.");

        Address = null;
        SetUpdated(updatedByUserId);
    }

    public void MarkProvisioned(string productTenantId, string updatedByUserId)
    {
        if (string.IsNullOrWhiteSpace(productTenantId))
            throw new ArgumentException("Product tenant id is required.", nameof(productTenantId));

        ProductTenantId = productTenantId;
        Status = ChurchClientStatus.Active;
        ActivatedAt = DateTimeOffset.UtcNow;

        SetUpdated(updatedByUserId);
    }

    public void Suspend(string updatedByUserId)
    {
        if (IsDeleted)
            throw new InvalidOperationException("Cannot suspend a deleted church client.");

        if (Status == ChurchClientStatus.Cancelled)
            throw new InvalidOperationException("Cannot suspend a cancelled church client.");

        Status = ChurchClientStatus.Suspended;
        SetUpdated(updatedByUserId);
    }

    public void Activate(string updatedByUserId)
    {
        if (IsDeleted)
            throw new InvalidOperationException("Cannot activate a deleted church client.");

        if (Status == ChurchClientStatus.Cancelled)
            throw new InvalidOperationException("Cannot activate a cancelled church client.");

        Status = ChurchClientStatus.Active;
        ActivatedAt ??= DateTimeOffset.UtcNow;

        SetUpdated(updatedByUserId);
    }

    public void Cancel(string updatedByUserId)
    {
        if (IsDeleted)
            throw new InvalidOperationException("Cannot cancel a deleted church client.");

        if (Status == ChurchClientStatus.Cancelled)
            return;

        Status = ChurchClientStatus.Cancelled;
        CancelledAt = DateTimeOffset.UtcNow;

        SetDeleted(updatedByUserId);
    }
}
