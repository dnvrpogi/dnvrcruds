namespace CrudeAspNet.Models;

public class KierDashboardViewModel
{
    public bool IsAvailable { get; set; }
    public string? ErrorMessage { get; set; }
    public List<KierStudent> Students { get; set; } = [];
    public List<KierCourse> Courses { get; set; } = [];
    public List<KierSchoolYear> SchoolYears { get; set; } = [];
    public List<KierEnrollment> Enrollments { get; set; } = [];
}

public record KierStudent(string Studid, string StudentName, string Status);
public record KierCourse(string Courscode, string CourseName);
public record KierSchoolYear(string Sycode, string SchoolYearName);
public record KierEnrollment(
    int EnrollmentId,
    string Studid,
    string StudentName,
    string Sycode,
    string SchoolYear,
    string Courscode,
    string CourseName,
    string Semcode,
    string SemesterName,
    string Status,
    DateTime EnrollmentDate);