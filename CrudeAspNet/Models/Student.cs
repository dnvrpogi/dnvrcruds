using System.ComponentModel.DataAnnotations;

namespace CrudeAspNet.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter the student's name.")]
        [StringLength(100)]
        public string StudentName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter a student ID.")]
        [StringLength(40)]
        public string StudentId { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string? Email { get; set; }
    }
}
