using System.ComponentModel.DataAnnotations;

namespace CrudeAspNet.Models
{
    public class SchoolYear
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter a school-year code.")]
        [StringLength(30)]
        public string SchoolYearCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter the semester.")]
        [StringLength(30)]
        public string Semester { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter the school-year status.")]
        [StringLength(30)]
        public string Status { get; set; } = string.Empty;

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}