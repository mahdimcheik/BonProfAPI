using BonProf.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BonProf.Models;

[Table("Orders")]
public class Order : BaseModel
{
    [Required]
    [MaxLength(50)]
    public string OrderNumber { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "timestamp with time zone")]
    public DateTimeOffset OrderDate { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal ReductionPercentage { get; set; } = 0m;

    [Column(TypeName = "decimal(18,2)")]
    public decimal ReductionAmount { get; set; } = 0m;

    [Required]
    [ForeignKey(nameof(Student))]
    public Guid StudentId { get; set; }
    public Student? Student { get; set; }

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    [ForeignKey(nameof(Payment))]
    public Guid? PaymentId { get; set; }
    public Payment? Payment { get; set; }

    // Parameterless constructor for EF Core
    public Order()
    {
    }
}
