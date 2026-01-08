using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using BonProf.Models;

namespace BonProf.Models;

public class TeacherDetails
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }
    public List<CursusDetails> Cursuses { get; set; }
    public string? LinkedIn { get; set; }
    public string? FaceBook { get; set; }
    public string? GitHub { get; set; }
    public string? Twitter { get; set; }
    public decimal PriceIndicative { get; set; }

    public TeacherDetails() { }
    [SetsRequiredMembers]
    public TeacherDetails(Teacher teacher)
    {
        Id = teacher.Id;

        CreatedAt = teacher.CreatedAt;
        UpdatedAt = teacher.UpdatedAt;

        LinkedIn = teacher.LinkedIn;
        FaceBook = teacher.FaceBook;
        GitHub = teacher.GitHub;
        Twitter = teacher.Twitter;
        PriceIndicative = teacher.PriceIndicative;
        Cursuses = teacher.Cursuses.Select(c => new CursusDetails(c)).ToList();
    }
}

/// <summary>
/// DTO pour la cr�ation d'un profil enseignant
/// </summary>
public class TeacherCreate
{
}


public class TeacherUpdate
{
    // [StringLength(200, ErrorMessage = "Le titre ne peut pas d�passer 200 caract�res")]
    // public string? Title { get; set; }
    //
    // [StringLength(1000, ErrorMessage = "La description ne peut pas d�passer 1000 caract�res")]
    // public string? Description { get; set; }
    public string? LinkedIn { get; set; }
    public string? FaceBook { get; set; }
    public string? GitHub { get; set; }
    public string? Twitter { get; set; }
    public decimal PriceIndicative { get; set; }
    
    
    public void UpdateTeacher(Teacher teacher)
    {
        // teacher.Title = Title;
        // teacher.Description = Description;
        teacher.LinkedIn = LinkedIn;
        teacher.FaceBook = FaceBook;
        teacher.GitHub = GitHub;
        teacher.Twitter = Twitter;
        teacher.PriceIndicative = PriceIndicative;
    }
}
