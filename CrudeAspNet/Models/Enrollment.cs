using System.ComponentModel.DataAnnotations;

namespace CrudeAspNet.Models
{
    public class Enrollment
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Select a student.")]
        public int StudentId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Select a course.")]
        public int CourseId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Select a school year.")]
        public int SchoolYearId { get; set; }

        public Student Student { get; set; } = null!;
        public Course Course { get; set; } = null!;
        public SchoolYear SchoolYear { get; set; } = null!;
    }
}