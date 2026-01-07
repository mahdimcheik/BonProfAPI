using Microsoft.AspNetCore.Identity;
using BonProf.Models.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace BonProf.Models;
public class RoleApp : IdentityRole<Guid>, IArchivable, ICreatable, IUpdateable
{
    public DateTimeOffset? ArchivedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public required string Color { get; set; }
    public ICollection<IdentityUserRole<Guid>> UserRoles { get; set; } =
        new List<IdentityUserRole<Guid>>();

    [SetsRequiredMembers]
    public RoleApp() { }
}
