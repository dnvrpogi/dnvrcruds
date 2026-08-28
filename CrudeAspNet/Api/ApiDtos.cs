namespace CrudeAspNet.Api;

public record StudentRequest(string StudentName, string StudentId, string? Email);
public record StudentResponse(int Id, string StudentName, string StudentId, string? Email);
public record CourseRequest(string CourseCode, string CourseName);
public record CourseResponse(int Id, string CourseCode, string CourseName);
public record SchoolYearRequest(string SchoolYearCode, string Semester, string Status);
public record SchoolYearResponse(int Id, string SchoolYearCode, string Semester, string Status);
public record EnrollmentRequest(int StudentId, int CourseId, int SchoolYearId);
public record EnrollmentResponse(
    int Id,
    int StudentId,
    string StudentName,
    int CourseId,
    string CourseCode,
    int SchoolYearId,
    string SchoolYearCode);