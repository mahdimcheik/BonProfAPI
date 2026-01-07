using BonProf.Models;
using System.ComponentModel.DataAnnotations;

namespace BonProf.Models;


public class ReservationDetailsDTO(Reservation booking)
{
    public Guid Id => booking.Id;
    public string Title => booking.Title;
    public string Description => booking.Description;
    public StatusReservationOutput? Status =>
        booking.Status is not null ? new StatusReservationOutput(booking.Status) : null;
    public StudentDetails? Student => null; // todo repair
    public SlotDetails? Slot => booking.Slot is not null ? new SlotDetails(booking.Slot) : null;
    public ProductDetails Product =>  new ProductDetails(booking.Product);
}

public class ReservationCreateDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public UserDetails? Student { get; set; }
    public Guid SlotId { get; set; }
    public Guid StudentId { get; set; }
    public Guid ProductId { get; set; }

    public ReservationCreateDTO(Reservation reservation)
    {
        Title = reservation.Title;
        Description = reservation.Description;
        SlotId = reservation.SlotId;
        StudentId = reservation.StudentId;
        ProductId = reservation.ProductId;
    }
}

public class BookingUpdateDTO
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Title { get; set; }
    public string Description { get; set; }

    public void UpdateModel(Reservation reservation)
    {
        reservation.Title = Title;
        reservation.Description = Description;
    }
}