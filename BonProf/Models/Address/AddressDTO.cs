using BonProf.Utilities;
using System.ComponentModel.DataAnnotations;

namespace BonProf.Models;

/// <summary>
/// DTO pour l'affichage des informations d'une adresse
/// </summary>
public class AddressDetails
{
    /// <summary>
    /// Identifiant unique de l'adresse
    /// </summary>
    /// <example>550e8400-e29b-41d4-a716-446655440000</example>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Rue de l'adresse
    /// </summary>
    /// <example>123 Rue de la Paix</example>
    [Required]
    public string Street { get; set; }

    /// <summary>
    /// Ville
    /// </summary>
    /// <example>Paris</example>
    [Required]
    public string City { get; set; }


    /// <summary>
    /// Pays
    /// </summary>
    /// <example>France</example>
    [Required]
    public string Country { get; set; }

    /// <summary>
    /// Code postal
    /// </summary>
    /// <example>75001</example>
    [Required]
    public string ZipCode { get; set; }

    /// <summary>
    /// Informations supplémentaires
    /// </summary>
    /// <example>Appartement 42, 3ème étage</example>
    public string? AdditionalInfo { get; set; }

    /// <summary>
    /// Longitude géographique
    /// </summary>
    /// <example>2.3522</example>
    public float? Longitude { get; set; }

    /// <summary>
    /// Latitude géographique
    /// </summary>
    /// <example>48.8566</example>
    public float? Latitude { get; set; }

    /// <summary>
    /// user , prof ou eleve
    /// </summary>
    public Guid ProfileId { get; set; }
    [Required(ErrorMessage = "Le type est requis")]
    public Guid TypeId { get; set; }



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

    public AddressDetails() { }

    public AddressDetails(Address address)
    {
        Id = address.Id;
        Street = address.Street;
        City = address.City;
        Country = address.Country;
        ZipCode = address.ZipCode;
        AdditionalInfo = address.AdditionalInfo;
        Longitude = address.Longitude;
        Latitude = address.Latitude;
        ProfileId = address.UserId;
        TypeId = address.TypeId;
        CreatedAt = address.CreatedAt;
        UpdatedAt = address.UpdatedAt;
    }
}

/// <summary>
/// DTO pour la création d'une nouvelle adresse
/// </summary>
public class AddressCreate
{
    /// <summary>
    /// Rue de l'adresse
    /// </summary>
    /// <example>123 Rue de la Paix</example>
    [Required(ErrorMessage = "La rue est requise")]
    [StringLength(128, ErrorMessage = "La rue ne peut pas dépasser 128 caractères")]
    public string Street { get; set; }

    /// <summary>
    /// Ville
    /// </summary>
    /// <example>Paris</example>
    [Required(ErrorMessage = "La ville est requise")]
    [StringLength(64, ErrorMessage = "La ville ne peut pas dépasser 64 caractères")]
    public string City { get; set; }


    /// <summary>
    /// Pays
    /// </summary>
    /// <example>France</example>
    [Required(ErrorMessage = "Le pays est requis")]
    [StringLength(64, ErrorMessage = "Le pays ne peut pas dépasser 64 caractères")]
    public string Country { get; set; }

    /// <summary>
    /// Code postal
    /// </summary>
    /// <example>75001</example>
    [Required(ErrorMessage = "Le code postal est requis")]
    [StringLength(16, ErrorMessage = "Le code postal ne peut pas dépasser 16 caractères")]
    public string ZipCode { get; set; }

    /// <summary>
    /// Informations supplémentaires
    /// </summary>
    /// <example>Appartement 42, 3ème étage</example>
    [StringLength(200, ErrorMessage = "Les informations supplémentaires ne peuvent pas dépasser 200 caractères")]
    public string? AdditionalInfo { get; set; }

    /// <summary>
    /// Longitude géographique
    /// </summary>
    /// <example>2.3522</example>
    [Range(-180, 180, ErrorMessage = "La longitude doit être comprise entre -180 et 180")]
    public float? Longitude { get; set; }

    /// <summary>
    /// Latitude géographique
    /// </summary>
    /// <example>48.8566</example>
    [Range(-90, 90, ErrorMessage = "La latitude doit être comprise entre -90 et 90")]
    public float? Latitude { get; set; }

    [Required(ErrorMessage = "L'identifiant utilisateur est requis")]
    public Guid UserId { get; set; }
    [Required(ErrorMessage = "Le type est requis")]
    public Guid TypeId { get; set; }
}

/// <summary>
/// DTO pour la mise à jour d'une adresse existante
/// </summary>
public class AddressUpdate
{
    [Required]
    public Guid Id { get; set; }
    /// <summary>
    /// Rue de l'adresse
    /// </summary>
    /// <example>123 Rue de la Paix</example>
    [Required(ErrorMessage = "La rue est requise")]
    [StringLength(128, ErrorMessage = "La rue ne peut pas dépasser 128 caractères")]
    public string Street { get; set; }

    /// <summary>
    /// Ville
    /// </summary>
    /// <example>Paris</example>
    [Required(ErrorMessage = "La ville est requise")]
    [StringLength(64, ErrorMessage = "La ville ne peut pas dépasser 64 caractères")]
    public string City { get; set; }

    /// <summary>
    /// Pays
    /// </summary>
    /// <example>France</example>
    [Required(ErrorMessage = "Le pays est requis")]
    [StringLength(64, ErrorMessage = "Le pays ne peut pas dépasser 64 caractères")]
    public string Country { get; set; }

    /// <summary>
    /// Code postal
    /// </summary>
    /// <example>75001</example>
    [Required(ErrorMessage = "Le code postal est requis")]
    [StringLength(16, ErrorMessage = "Le code postal ne peut pas dépasser 16 caractères")]
    public string ZipCode { get; set; }

    /// <summary>
    /// Informations supplémentaires
    /// </summary>
    /// <example>Appartement 42, 3ème étage</example>
    [StringLength(200, ErrorMessage = "Les informations supplémentaires ne peuvent pas dépasser 200 caractères")]
    public string? AdditionalInfo { get; set; }

    /// <summary>
    /// Longitude géographique
    /// </summary>
    /// <example>2.3522</example>
    [Range(-180, 180, ErrorMessage = "La longitude doit être comprise entre -180 et 180")]
    public float? Longitude { get; set; }

    /// <summary>
    /// Latitude géographique
    /// </summary>
    /// <example>48.8566</example>
    [Range(-90, 90, ErrorMessage = "La latitude doit être comprise entre -90 et 90")]
    public float? Latitude { get; set; }

    /// <summary>
    /// Identifiant de l'utilisateur associé
    /// </summary>
    /// <example>550e8400-e29b-41d4-a716-446655440001</example>
    [Required(ErrorMessage = "L'identifiant utilisateur est requis")]
    public Guid ProfileId { get; set; }
    [Required(ErrorMessage = "Le type est requis")]
    public Guid TypeId { get; set; }
}