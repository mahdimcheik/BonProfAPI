using System.ComponentModel.DataAnnotations;

namespace BonProf.Models;

/// <summary>
/// DTO pour l'affichage des informations d'une formation
/// </summary>
public class FormationDetails
{
    /// <summary>
    /// Identifiant unique de la formation
    /// </summary>
    /// <example>550e8400-e29b-41d4-a716-446655440000</example>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Titre de la formation
    /// </summary>
    /// <example>Master en Informatique</example>
    [Required]
    public string Title { get; set; }

    /// <summary>
    /// Description détaillée de la formation
    /// </summary>
    /// <example>Formation spécialisée en développement logiciel et intelligence artificielle</example>
    [Required]
    public string Description { get; set; }

    /// <summary>
    /// Institution où la formation a été suivie
    /// </summary>
    /// <example>Université de Paris</example>
    [Required]
    public string Institute { get; set; }

    /// <summary>
    /// Date de début de la formation
    /// </summary>
    /// <example>2020-09-01T00:00:00Z</example>
    [Required]
    public DateTimeOffset DateFrom { get; set; }

    /// <summary>
    /// Date de fin de la formation (optionnelle si en cours)
    /// </summary>
    /// <example>2022-06-30T00:00:00Z</example>
    public DateTimeOffset? DateTo { get; set; }

    /// <summary>
    /// Date de création de l'enregistrement
    /// </summary>
    /// <example>2023-01-15T10:30:00Z</example>
    [Required]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Date de dernière mise à jour
    /// </summary>
    /// <example>2023-01-20T14:45:00Z</example>
    public DateTimeOffset? UpdatedAt { get; set; }

    public FormationDetails() { }

    public FormationDetails(Formation formation)
    {
        Id = formation.Id;
        Title = formation.Title;
        Description = formation.Description;
        Institute = formation.Institute;
        DateFrom = formation.DateFrom;
        DateTo = formation.DateTo;
        CreatedAt = formation.CreatedAt;
        UpdatedAt = formation.UpdatedAt;
    }
}

/// <summary>
/// DTO pour la création d'une nouvelle formation
/// </summary>
public class FormationCreate
{
    /// <summary>
    /// Titre de la formation
    /// </summary>
    /// <example>Master en Informatique</example>
    [Required(ErrorMessage = "Le titre est requis")]
    [StringLength(200, ErrorMessage = "Le titre ne peut pas dépasser 200 caractères")]
    public string Title { get; set; }

    /// <summary>
    /// Description détaillée de la formation
    /// </summary>
    /// <example>Formation spécialisée en développement logiciel et intelligence artificielle</example>
    [Required(ErrorMessage = "La description est requise")]
    [StringLength(1000, ErrorMessage = "La description ne peut pas dépasser 1000 caractères")]
    public string Description { get; set; }

    /// <summary>
    /// Institution où la formation a été suivie
    /// </summary>
    /// <example>Université de Paris</example>
    [Required(ErrorMessage = "L'institution est requise")]
    [StringLength(200, ErrorMessage = "L'institution ne peut pas dépasser 200 caractères")]
    public string Institute { get; set; }

    /// <summary>
    /// Date de début de la formation
    /// </summary>
    /// <example>2020-09-01T00:00:00Z</example>
    [Required(ErrorMessage = "La date de début est requise")]
    public DateTimeOffset DateFrom { get; set; }

    /// <summary>
    /// Date de fin de la formation (optionnelle si en cours)
    /// </summary>
    /// <example>2022-06-30T00:00:00Z</example>
    public DateTimeOffset? DateTo { get; set; }

    /// <summary>
    /// id du professeur ou de l'étudiant associé
    /// </summary>
    public Guid TeacherId { get; set; }
}

/// <summary>
/// DTO pour la mise à jour d'une formation existante
/// </summary>
public class FormationUpdate
{
    [Required(ErrorMessage = "L'identifiant est requis")]
    public Guid Id { get; set; }
    /// <summary>
    /// Titre de la formation
    /// </summary>
    /// <example>Master en Informatique</example>
    [Required(ErrorMessage = "Le titre est requis")]
    [StringLength(200, ErrorMessage = "Le titre ne peut pas dépasser 200 caractères")]
    public string Title { get; set; }

    /// <summary>
    /// Description détaillée de la formation
    /// </summary>
    /// <example>Formation spécialisée en développement logiciel et intelligence artificielle</example>
    [Required(ErrorMessage = "La description est requise")]
    [StringLength(1000, ErrorMessage = "La description ne peut pas dépasser 1000 caractères")]
    public string Description { get; set; }

    /// <summary>
    /// Institution où la formation a été suivie
    /// </summary>
    /// <example>Université de Paris</example>
    [Required(ErrorMessage = "L'institution est requise")]
    [StringLength(200, ErrorMessage = "L'institution ne peut pas dépasser 200 caractères")]
    public string Institute { get; set; }

    /// <summary>
    /// Date de début de la formation
    /// </summary>
    /// <example>2020-09-01T00:00:00Z</example>
    [Required(ErrorMessage = "La date de début est requise")]
    public DateTimeOffset DateFrom { get; set; }

    /// <summary>
    /// Date de fin de la formation (optionnelle si en cours)
    /// </summary>
    /// <example>2022-06-30T00:00:00Z</example>
    public DateTimeOffset? DateTo { get; set; }

    /// <summary>
    /// id du professeur ou de l'étudiant associé
    /// </summary>
    public Guid? TeacherId { get; set; }
}
