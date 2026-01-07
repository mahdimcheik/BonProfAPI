using BonProf.Models;
using System.ComponentModel.DataAnnotations;

namespace BonProf.Models;

public class StatusAccountDetails(StatusAccount gender)
{
    public Guid Id => gender.Id;
    public string Name => gender.Name;
    public string Color => gender.Color;
    public string? Icon => gender.Icon;
}
