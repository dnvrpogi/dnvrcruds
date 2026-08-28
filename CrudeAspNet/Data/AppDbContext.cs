using Microsoft.EntityFrameworkCore;
using CrudeAspNet.Models;

namespace CrudeAspNet.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<SchoolYear> SchoolYears { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().HasIndex(course => course.CourseCode).IsUnique();
            modelBuilder.Entity<SchoolYear>().HasIndex(year => year.SchoolYearCode).IsUnique();
            modelBuilder.Entity<Enrollment>().HasIndex(enrollment => new
            {
                enrollment.StudentId,
                enrollment.CourseId,
                enrollment.SchoolYearId
            }).IsUnique();
        }
    }
}
