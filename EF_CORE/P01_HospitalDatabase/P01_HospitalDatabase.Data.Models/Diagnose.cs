using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_HospitalDatabase.Data.Models
{
    public class Diagnose
    {
        [Key]
        public int DiagnoseId { get; set; }

        [Required]
        [MaxLength(50)]
        [Unicode(true)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(250)]
        [Unicode(true)]
        public string Comments { get; set; } = null!;

        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;
    }
}
