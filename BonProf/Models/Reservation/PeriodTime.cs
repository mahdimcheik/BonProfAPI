namespace BonProf.Models;
/// <summary>
/// Période de recherche 
/// </summary>
public class PeriodTime
{
    public required DateTimeOffset DateFrom { get; set; }
    public required DateTimeOffset DateTo{ get; set; }
}