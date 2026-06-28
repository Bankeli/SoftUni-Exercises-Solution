using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_HospitalDatabase.Data.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        [Required]
        [MaxLength(50)]
        [Unicode(true)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        [Unicode(true)]
        public string LastName { get; set; } = null!;


        [Required]
        [MaxLength(250)]
        [Unicode(true)]
        public string Address { get; set; } = null!;

        [MaxLength(80)]
        [Unicode(false)]
        public string? Email { get; set; }

        public bool HasInsurance { get; set; }

        public ICollection<PatientMedicament> Prescriptions { get; set; }
            = new HashSet<PatientMedicament>();

        public ICollection<Diagnose> Diagnoses { get; set; } 
            = new HashSet<Diagnose>();

        public ICollection<Visitation> Visitations { get; set; } 
            = new HashSet<Visitation>();
    }
}
