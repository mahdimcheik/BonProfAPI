using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using BonProf.Models;

namespace BonProf.Models;

/// <summary>
/// DTO pour l'affichage d�taill� du profil d'un enseignant
/// </summary>
public class TeacherDetails
{
    /// <summary>
    /// Identifiant unique du profil enseignant
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Titre professionnel de l'enseignant
    /// </summary>
    /// <example>Professeur de Math�matiques</example>
    public string? Title { get; set; }

    /// <summary>
    /// Description du profil de l'enseignant
    /// </summary>
    /// <example>Enseignant passionn� avec 10 ans d'exp�rience</example>
    public string? Description { get; set; }

    /// <summary>
    /// Date de cr�ation du profil
    /// </summary>
    [Required]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Date de derni�re mise � jour du profil
    /// </summary>
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
        Title = teacher.Title;
        Description = teacher.Description;
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
    /// <summary>
    /// Titre professionnel de l'enseignant
    /// </summary>
    /// <example>Professeur de Math�matiques</example>
    [StringLength(200, ErrorMessage = "Le titre ne peut pas d�passer 200 caract�res")]
    public string? Title { get; set; }

    /// <summary>
    /// Description du profil de l'enseignant
    /// </summary>
    /// <example>Enseignant passionn� avec 10 ans d'exp�rience</example>
    [StringLength(1000, ErrorMessage = "La description ne peut pas d�passer 1000 caract�res")]
    public string? Description { get; set; }
}

/// <summary>
/// DTO pour la mise � jour d'un profil enseignant
/// </summary>
public class TeacherUpdate
{
    [StringLength(200, ErrorMessage = "Le titre ne peut pas d�passer 200 caract�res")]
    public string? Title { get; set; }

    [StringLength(1000, ErrorMessage = "La description ne peut pas d�passer 1000 caract�res")]
    public string? Description { get; set; }
    public string? LinkedIn { get; set; }
    public string? FaceBook { get; set; }
    public string? GitHub { get; set; }
    public string? Twitter { get; set; }
    public decimal PriceIndicative { get; set; }
    
    
    public void UpdateTeacher(Teacher teacher)
    {
        teacher.Title = Title;
        teacher.Description = Description;
        teacher.LinkedIn = LinkedIn;
        teacher.FaceBook = FaceBook;
        teacher.GitHub = GitHub;
        teacher.Twitter = Twitter;
        teacher.PriceIndicative = PriceIndicative;
    }
}
