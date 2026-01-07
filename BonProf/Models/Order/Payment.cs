using BonProf.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BonProf.Models;

public class Payment : BaseModel
{
    [Required]
    public decimal Amount { get; set; }
    [Required]
    public Guid MethodId { get; set; }
    public PaymentMethod? Method { get; set; }
    [Required]
    [ForeignKey(nameof(Order))]
    public Guid OrderId { get; set; }
    public Order? Order { get; set; }
    public Guid TransactionRef { get; set; }
}
