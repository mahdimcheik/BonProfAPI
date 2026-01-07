using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using BonProf.Models;
using Microsoft.AspNetCore.Identity;
using BonProf.Models;
using BonProf.Models.Interfaces;
using BonProf.Utilities;

namespace BonProf.Models;

public class UserApp : IdentityUser<Guid>, IArchivable, IUpdateable, ICreatable
{
    [Required]
    [MaxLength(64)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(64)]
    public string LastName { get; set; } = string.Empty;
    [Required]
    public required DateTimeOffset DateOfBirth { get; set; }

    [MaxLength(500)]
    public string? ImgUrl { get; set; }
    [Required]
    public required bool DataProcessingConsent { get; set; } = false;

    [Required]
    public required bool PrivacyPolicyConsent { get; set; } = false;
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? ArchivedAt { get; set; }

    [Required]
    public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;

    // Status account
    [Required]
    [ForeignKey(nameof(Status))]
    public Guid StatusId { get; set; }
    public StatusAccount? Status { get; set; }
    


    //gender
    [Required]
    [ForeignKey(nameof(Gender))]
    public Guid GenderId { get; set; }
    public Gender? Gender { get; set; }

    // navigation to teacher / student
    public Teacher? Teacher { get; set; }
    public Student? Student { get; set; }
    // roles
    public ICollection<IdentityUserRole<Guid>> UserRoles { get; set; } = new List<IdentityUserRole<Guid>>();
    public ICollection<Address> Addresses { get; set; } = new List<Address>();
    public ICollection<Formation> Formations { get; set; } = new List<Formation>();
    public ICollection<Language> Languages { get; set; } = new List<Language>();

    [SetsRequiredMembers]
    public UserApp() { }

    [SetsRequiredMembers]
    public UserApp(UserCreate newUser)
    {
        UserName = newUser.Email;
        Email = newUser.Email;
        FirstName = newUser.FirstName;
        LastName = newUser.LastName;
        GenderId  = newUser.GenderId;
        DataProcessingConsent = true;
        PrivacyPolicyConsent = true;
        StatusId = HardCode.ACCOUNT_PENDING;
    }
}
