using BonProf.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BonProf.Models;

public class StudentDetails 
{
    [Required]
    public Guid Id { get; set; }

    public StudentDetails(Student student)
    {
        Id = student.Id;
    }
}

public class StudentCreate
{
    public StudentCreate() { }
}

public class StudentUpdate 
{
    public void UpdateStudent(Student student)
    {
    }
}
