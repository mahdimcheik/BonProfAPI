using BonProf.Models.Interfaces;

namespace BonProf.Models;

public class CategoryCursus : BaseModelOption
{
    public ICollection<Cursus> Cursuses { get; set; } = new List<Cursus>();
}
