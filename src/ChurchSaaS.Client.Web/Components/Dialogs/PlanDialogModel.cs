using System.ComponentModel.DataAnnotations;

namespace ChurchSaaS.Client.Web.Components.Dialogs;

public class PlanDialogModel
{
    [Required]
    [MaxLength(50)]
    public string? Code { get; set; }

    [Required]
    [MaxLength(200)]
    public string? Name { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    [Range(0, double.MaxValue)]
    public decimal BasePrice { get; set; }

    [Range(0, double.MaxValue)]
    public decimal PricePerAdditionalTenant { get; set; }

    [Range(0, double.MaxValue)]
    public decimal PricePerAdditionalMember { get; set; }

    public int? MaxTenants { get; set; }
    public int? MaxMembers { get; set; }

    public bool HasFinance { get; set; } = true;
    public bool HasEvents { get; set; } = true;
    public bool HasCommunication { get; set; } = true;
    public bool HasKidsCheckin { get; set; }
}
