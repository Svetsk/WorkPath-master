using System.ComponentModel.DataAnnotations.Schema;
using WorkPath.Server.Models;

namespace WorkPath.Server.Entities;

public class UserEntity
{
    public Guid ID { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public char Gender { get; set; }
    public string Login { get; set; }
    public DateOnly DateBirth { get; set; }
    public DateTime CreatedAt { get; set; }
    public HashSet<CompanyEntity>? Companies { get; set; } = new();
    public HashSet<EducationEntity>? Educations { get; set; } = new();
    public string PasswordHash { get; set; }
}

public class EducationEntity
{
    public Guid ID { get; set; }
    public Guid UserID { get; set; }
    [ForeignKey(nameof(UserID))] public UserEntity? User { get; set; }
    public EducationLevel Level { get; set; }
    public string Institution { get; set; }
    public int Course { get; set; }
    public int StartedAt { get; set; }
    public int? FinishedAt { get; set; }
    public string Specialization { get; set; }
    public string AcademicPerformance { get; set; }
}

public class AwardsAndDiplomas
{
    // properties for awards and diplomas
}

public class WorkExperience
{
    // properties for work experience
}

public class Comment
{
    // properties for comments
}

public class Internship
{

}



public class CompanyEntity
{
    public Guid ID { get; set; }
    public string Name { get; set; }
    public string INN { get; set; }
    public string OGRN { get; set; }
    public Guid DirectorID { get; set; }
    [ForeignKey(nameof(DirectorID))]
    public UserEntity? Director { get; set; }

    public HashSet<JobEntity>? Jobs { get; set; } = new();
}

public class JobEntity
{
    public Guid ID { get; set; }
    public Guid CompanyID { get; set; }
    [ForeignKey(nameof(CompanyID))]
    public CompanyEntity? Company { get; set; }
    
    public DateTime ApplicationDeadline { get; set; }
    public DateTime ProgramStart { get; set; }
    public string Vacancy { get; set; }
    public decimal Salary { get; set; }
    public EmploymentType EmploymentType { get; set; }
    public string Location { get; set; }
    public bool ExperienceRequired { get; set; }
    public string Responsibilities { get; set; }
    public string Requirements { get; set; }
    public string Conditions { get; set; }
    public string KeySkills { get; set; }
    public string ContactInformation { get; set; }
    
    public int Duration { get; set; }
    public string SelectionProcess { get; set; }
    
    public string Tags { get; set; }
}