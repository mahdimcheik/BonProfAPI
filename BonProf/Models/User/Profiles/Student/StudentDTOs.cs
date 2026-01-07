using BonProf.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BonProf.Models;

public class StudentDetails : BaseModel
{
    /// <summary>
    /// Identifiant unique du profil enseignant
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    public StudentDetails(Student student)
    {
        Id = student.Id;
    }
}
