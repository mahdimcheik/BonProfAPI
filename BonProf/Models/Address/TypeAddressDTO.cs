using System.ComponentModel.DataAnnotations;

namespace BonProf.Models;

public class TypeAddressDetails
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }

    [Required]
    public string Color { get; set; }
    public string? Icon { get; set; }
    public TypeAddressDetails()
    {
    }
    public TypeAddressDetails(TypeAddress typeAddress)
    {
        Id = typeAddress.Id;
        Name = typeAddress.Name;
        Color = typeAddress.Color;
        Icon = typeAddress.Icon;
    }
}
