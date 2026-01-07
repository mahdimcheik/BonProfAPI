using BonProf.Models.Interfaces;

namespace BonProf.Models;

public class Language : BaseModelOption
{
    public ICollection<Teacher>? Teachers { get; set; }
}
