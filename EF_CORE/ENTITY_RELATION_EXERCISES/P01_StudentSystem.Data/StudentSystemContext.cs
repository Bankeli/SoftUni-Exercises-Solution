using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {

        public StudentSystemContext()
        {
        }

        public StudentSystemContext(DbContextOptions<StudentSystemContext> options)
    : base(options)
        {
        }
        public virtual DbSet<Course> Courses { get; set; } = null!;

        public virtual DbSet<Homework> Homeworks { get; set; } = null!;

        public virtual DbSet<Resource> Resources { get; set; } = null!;

        public virtual DbSet<Student> Students { get; set; } = null;
        
        public virtual DbSet<StudentCourse> StudentsCourses { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=BANKELI\\SQLEXPRESS;Database=StudentSystem;Trusted_Connection=True;Encrypt=False;");
            }
            base.OnConfiguring(optionsBuilder);
        }

       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentCourse>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.CourseId });
            });
            base.OnModelCreating(modelBuilder);
        }

    }
}
