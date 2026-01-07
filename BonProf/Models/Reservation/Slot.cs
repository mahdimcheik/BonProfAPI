using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BonProf.Models.Interfaces;

namespace BonProf.Models
{
    [Table("Slots")]
    public class Slot : BaseModel
    {
        [Required]
        [Column(TypeName = "timestamp with time zone")]
        public DateTimeOffset DateFrom { get; set; }
        
        [Required]
        [Column(TypeName = "timestamp with time zone")]
        public DateTimeOffset DateTo { get; set; }
        
        [Required]
        [ForeignKey(nameof(Teacher))]
        public Guid TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
        
        [ForeignKey(nameof(Type))]
        public Guid? TypeId { get; set; }
        public TypeSlot? Type { get; set; }
        
        public Reservation? Reservation { get; set; }
    }
}
