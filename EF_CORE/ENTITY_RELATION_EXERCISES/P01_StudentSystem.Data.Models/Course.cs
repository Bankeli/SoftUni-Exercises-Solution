using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_StudentSystem.Data.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Required]
        [MaxLength(80)]
        [Unicode(true)]
        public string Name { get; set; } = null!;

        [MaxLength(500)]
        [Unicode(true)]
        public string? Description { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        public decimal Price { get; set; }

        public ICollection<StudentCourse> StudentsCourses { get; set; }
            = new List<StudentCourse>();

        public ICollection<Resource> Resources { get; set; }
            = new List<Resource>();

        public ICollection<Homework> Homeworks { get; set; }
            = new List<Homework>();
    }


}
