using BonProf.Models;
using BonProf.Models.Interfaces;
using BonProf.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace BonProf.Models;

[Table("Reservations")]
public class Reservation : BaseModel
{
    [Required]
    [StringLength(64)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(256)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [ForeignKey(nameof(Slot))]
    public Guid SlotId { get; set; }
    public Slot? Slot { get; set; }

    [Required]
    [ForeignKey(nameof(Product))]
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }

    [Required]
    [ForeignKey(nameof(Status))]
    public Guid StatusId { get; set; } = HardCode.RESERVATION_PENDING;
    public StatusReservation? Status { get; set; }

    [Required]
    [ForeignKey(nameof(Order))]
    public Guid OrderId { get; set; }
    public Order? Order { get; set; }

    [Required]
    [ForeignKey(nameof(Student))]
    public Guid StudentId { get; set; }
    public Student? Student { get; set; }

    // Parameterless constructor for EF Core
    public Reservation()
    {
    }

    [SetsRequiredMembers]
    public Reservation(ReservationCreateDTO bookingCreate)
    {
        Title = bookingCreate.Title;
        Description = bookingCreate.Description ?? "";
        SlotId = bookingCreate.SlotId;
        StudentId = bookingCreate.StudentId;
    }
}