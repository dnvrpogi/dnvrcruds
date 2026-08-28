using System.ComponentModel.DataAnnotations;

namespace CrudeAspNet.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter a course code.")]
        [StringLength(30)]
        public string CourseCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter a course name.")]
        [StringLength(120)]
        public string CourseName { get; set; } = string.Empty;

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}