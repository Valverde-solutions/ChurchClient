using System.Collections.ObjectModel;
using ChurchSaaS.Admin.Domain.Abstractions;
using ChurchSaaS.Client.Domain.Abstractions;
using ChurchSaaS.Client.Domain.Churches;
using ChurchSaaS.Client.Domain.ValueObjects;

namespace ChurchSaaS.Client.Domain.Entities;

/// <summary>
/// Unidade de igreja na árvore macro: Regional -> Sede -> Congregação -> Ponto.
/// </summary>
public sealed class ChurchUnit : TenantAuditableEntity<Guid>, IAggregateRoot
{
    private readonly List<ChurchUnit> _children = new();

    // EF / Serialização
    private ChurchUnit() { }
    public string LegalName { get; private set; } = default!;        // Razão social / nome oficial
    public string? TradeName { get; private set; }       // Nome fantasia / como aparece na UI

    // Documento e contato principal (Value Objects)
    public Document? Document { get; private set; }      // CNPJ/CPF/Outro
    public string MainContactName { get; private set; } = default!;
    public Email MainContactEmail { get; private set; }
    public PhoneNumber? MainContactPhone { get; private set; }

    // Endereço (Value Object já existente no domínio)
    public Address? Address { get; private set; }

    public string Name
    {
        get => LegalName;
        set => LegalName = value;
    }

    public string? Code { get; private set; }

    public ChurchUnitType Type { get; private set; }

    public Guid? ParentId { get; private set; }
    public ChurchUnit? Parent { get; private set; }

    /// <summary>
    /// Sempre aponta para a igreja local raiz (SEDE).
    /// </summary>
    public Guid LocalChurchId { get; private set; }
    public ChurchUnit? LocalChurch { get; private set; }

    public bool IsActive { get; private set; } = true;

    public IReadOnlyCollection<ChurchUnit> Children => new ReadOnlyCollection<ChurchUnit>(_children);

    #region Fábricas

    /// <summary>
    /// Cria uma unidade raiz (Regional ou Sede).
    /// </summary>
    public static ChurchUnit CreateRoot(
        TenantId tenantId,
        string legalName,
        string mainContactName,
        Email mainContactEmail,
        ChurchUnitType type,
        string createdByUserId,
        string? tradeName = null,
        Document? document = null,
        PhoneNumber? mainContactPhone = null,
        Address? address = null,
        string? code = null)
    {
        if (string.IsNullOrWhiteSpace(legalName))
            throw new ArgumentException("LegalName is required.", nameof(legalName));

        if (type is not (ChurchUnitType.Regional or ChurchUnitType.Sede))
            throw new InvalidOperationException("Root unit must be Regional or Sede.");

        var unit = new ChurchUnit
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            LegalName = legalName.Trim(),
            TradeName = string.IsNullOrWhiteSpace(tradeName) ? null : tradeName?.Trim(),
            MainContactName = mainContactName,
            MainContactEmail = mainContactEmail,
            Document = document,
            MainContactPhone = mainContactPhone,
            Address = address,
            Code = string.IsNullOrWhiteSpace(code) ? null : code.Trim(),
            Type = type,
            IsActive = true
        };

        // Se for Sede, a igreja local raiz é ela mesma
        if (type == ChurchUnitType.Sede)
        {
            unit.LocalChurchId = unit.Id;
        }

        unit.SetCreated(createdByUserId);
        return unit;
    }

    /// <summary>
    /// Cria uma unidade filha a partir de um pai já carregado.
    /// </summary>
    public static ChurchUnit CreateChild(
        ChurchUnit parent,
        string name,
        ChurchUnitType childType,
        string createdByUserId,
        string? code = null)
    {
        if (parent is null)
            throw new ArgumentNullException(nameof(parent));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));

        parent.ValidateChildType(childType);

        var localChurchId = parent.ResolveLocalChurchIdForChild(childType);

        var child = new ChurchUnit
        {
            Id = Guid.NewGuid(),
            TenantId = parent.TenantId,
            LegalName = name.Trim(),
            Code = string.IsNullOrWhiteSpace(code) ? null : code.Trim(),
            Type = childType,
            ParentId = parent.Id,
            LocalChurchId = localChurchId,
            IsActive = true
        };

        child.SetCreated(createdByUserId);

        parent._children.Add(child);
        return child;
    }

    #endregion

    #region Comportamentos

    /// <summary>
    /// Atalho para criar e adicionar filho pelo próprio agregado.
    /// </summary>
    public ChurchUnit AddChild(
        string name,
        ChurchUnitType childType,
        string createdByUserId,
        string? code = null)
        => CreateChild(this, name, childType, createdByUserId, code);

    public void UpdateBasicInfo(
        string name,
        string? code,
        string updatedByUserId)
    {
        if (IsDeleted)
            throw new InvalidOperationException("Cannot update a deleted church unit.");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));

        Name = name.Trim();
        Code = string.IsNullOrWhiteSpace(code) ? null : code.Trim();

        SetUpdated(updatedByUserId);
    }

    public void Activate(string updatedByUserId)
    {
        if (IsDeleted)
            throw new InvalidOperationException("Cannot activate a deleted church unit.");

        if (!IsActive)
        {
            IsActive = true;
            SetUpdated(updatedByUserId);
        }
    }

    public void Deactivate(string updatedByUserId)
    {
        if (IsDeleted)
            throw new InvalidOperationException("Cannot deactivate a deleted church unit.");

        if (IsActive)
        {
            IsActive = false;
            SetUpdated(updatedByUserId);
        }
    }

    /// <summary>
    /// (Opcional) Desativar em cascata todos os filhos.
    /// </summary>
    public void DeactivateCascade(string updatedByUserId)
    {
        Deactivate(updatedByUserId);

        foreach (var child in _children)
        {
            child.DeactivateCascade(updatedByUserId);
        }
    }

    #endregion

    #region Regras de domínio internas

    private void ValidateChildType(ChurchUnitType childType)
    {
        switch (Type)
        {
            case ChurchUnitType.Regional:
                if (childType is not (ChurchUnitType.Sede or ChurchUnitType.Congregacao))
                    throw new InvalidOperationException("Regional só pode ter Sede ou Congregação.");
                break;

            case ChurchUnitType.Sede:
                if (childType is not (ChurchUnitType.Congregacao or ChurchUnitType.Ponto))
                    throw new InvalidOperationException("Sede só pode ter Congregação ou Ponto.");
                break;

            case ChurchUnitType.Congregacao:
                if (childType != ChurchUnitType.Ponto)
                    throw new InvalidOperationException("Congregação só pode ter apenas Ponto.");
                break;

            case ChurchUnitType.Ponto:
                throw new InvalidOperationException("Ponto não pode ter filhos de ChurchUnit.");
        }
    }

    private Guid ResolveLocalChurchIdForChild(ChurchUnitType childType)
    {
        // Se eu sou Sede, qualquer filho (Congregação ou Ponto) pertence a essa Sede
        if (Type == ChurchUnitType.Sede)
            return Id;

        // Se eu já tenho uma LocalChurch (ex: Congregação/Ponto ligado a uma Sede),
        // qualquer filho herda a mesma igreja raiz
        if (LocalChurchId != Guid.Empty)
            return LocalChurchId;

        throw new InvalidOperationException("Não foi possível resolver LocalChurchId para o filho.");
    }

    #endregion
}
