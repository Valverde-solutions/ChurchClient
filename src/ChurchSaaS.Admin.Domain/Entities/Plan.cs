using ChurchSaaS.Admin.Domain.Abstractions;

namespace ChurchSaaS.Admin.Domain.Entities;

public sealed class Plan : AuditableEntity<Guid>, IAggregateRoot
{
    public string Code { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public decimal BasePrice { get; private set; }
    public decimal PricePerAdditionalTenant { get; private set; }
    public decimal PricePerAdditionalMember { get; private set; }
    public int? MaxTenants { get; private set; }
    public int? MaxMembers { get; private set; }
    public bool HasFinance { get; private set; }
    public bool HasEvents { get; private set; }
    public bool HasCommunication { get; private set; }
    public bool HasKidsCheckin { get; private set; }

    private Plan() { }

    private Plan(
        string code,
        string name,
        string? description,
        decimal basePrice,
        decimal pricePerAdditionalTenant,
        decimal pricePerAdditionalMember,
        int? maxTenants,
        int? maxMembers,
        bool hasFinance,
        bool hasEvents,
        bool hasCommunication,
        bool hasKidsCheckin)
    {
        Code = code;
        Name = name;
        Description = description;
        BasePrice = basePrice;
        PricePerAdditionalTenant = pricePerAdditionalTenant;
        PricePerAdditionalMember = pricePerAdditionalMember;
        MaxTenants = maxTenants;
        MaxMembers = maxMembers;
        HasFinance = hasFinance;
        HasEvents = hasEvents;
        HasCommunication = hasCommunication;
        HasKidsCheckin = hasKidsCheckin;
    }

    public static Plan Create(
        string code,
        string name,
        string? description,
        decimal basePrice,
        decimal pricePerAdditionalTenant,
        decimal pricePerAdditionalMember,
        int? maxTenants,
        int? maxMembers,
        bool hasFinance,
        bool hasEvents,
        bool hasCommunication,
        bool hasKidsCheckin,
        string createdBy)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code is required.", nameof(code));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));
        if (basePrice < 0)
            throw new ArgumentException("Base price cannot be negative.", nameof(basePrice));
        if (pricePerAdditionalTenant < 0)
            throw new ArgumentException("Price per additional tenant cannot be negative.", nameof(pricePerAdditionalTenant));
        if (pricePerAdditionalMember < 0)
            throw new ArgumentException("Price per additional member cannot be negative.", nameof(pricePerAdditionalMember));
        if (maxTenants is < 0)
            throw new ArgumentException("Max tenants cannot be negative.", nameof(maxTenants));
        if (maxMembers is < 0)
            throw new ArgumentException("Max members cannot be negative.", nameof(maxMembers));

        var plan = new Plan(
            code.Trim().ToUpperInvariant(),
            name.Trim(),
            string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            basePrice,
            pricePerAdditionalTenant,
            pricePerAdditionalMember,
            maxTenants,
            maxMembers,
            hasFinance,
            hasEvents,
            hasCommunication,
            hasKidsCheckin);

        plan.SetCreated(createdBy);
        return plan;
    }

    public void Update(
        string name,
        string? description,
        decimal basePrice,
        decimal pricePerAdditionalTenant,
        decimal pricePerAdditionalMember,
        int? maxTenants,
        int? maxMembers,
        bool hasFinance,
        bool hasEvents,
        bool hasCommunication,
        bool hasKidsCheckin,
        string updatedBy)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));
        if (basePrice < 0)
            throw new ArgumentException("Base price cannot be negative.", nameof(basePrice));
        if (pricePerAdditionalTenant < 0)
            throw new ArgumentException("Price per additional tenant cannot be negative.", nameof(pricePerAdditionalTenant));
        if (pricePerAdditionalMember < 0)
            throw new ArgumentException("Price per additional member cannot be negative.", nameof(pricePerAdditionalMember));
        if (maxTenants is < 0)
            throw new ArgumentException("Max tenants cannot be negative.", nameof(maxTenants));
        if (maxMembers is < 0)
            throw new ArgumentException("Max members cannot be negative.", nameof(maxMembers));

        Name = name.Trim();
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        BasePrice = basePrice;
        PricePerAdditionalTenant = pricePerAdditionalTenant;
        PricePerAdditionalMember = pricePerAdditionalMember;
        MaxTenants = maxTenants;
        MaxMembers = maxMembers;
        HasFinance = hasFinance;
        HasEvents = hasEvents;
        HasCommunication = hasCommunication;
        HasKidsCheckin = hasKidsCheckin;

        SetUpdated(updatedBy);
    }
}
