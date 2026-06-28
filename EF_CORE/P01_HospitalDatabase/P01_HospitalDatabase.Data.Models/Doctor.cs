using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace P01_HospitalDatabase.Data.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        [Required]
        [MaxLength(100)]
        [Unicode(true)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        [Unicode(true)]
        public string Specialty { get; set; } = null!;

        public ICollection<Visitation> Visitations { get; set; }
            = new HashSet<Visitation>();
    }
}
