using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using BonProf.Models.Interfaces;

namespace BonProf.Models;

public class Teacher : BaseModel
{
    [Required]
    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }
    public required UserApp? User { get; set; }
    public string? LinkedIn { get; set; }
    public string? FaceBook { get; set; }
    public string? GitHub { get; set; }
    public string? Twitter { get; set; }
    [Required]
    public required decimal PriceIndicative { get; set; }

    public ICollection<Cursus> Cursuses { get; set; } = new List<Cursus>();
    public ICollection<Experience> Experiences { get; set; } = new List<Experience>();
    public ICollection<Slot> Slots { get; set; } = new List<Slot>();

    [SetsRequiredMembers]
    public Teacher()
    {
    }
    
    [SetsRequiredMembers]
    public Teacher(Guid userId)
    {
        Id =  userId;
        UserId = userId;
    }
}
