using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductShop.DTOs.Import
{
    public class ImportUserDto
    {
        public string? FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; } = null!;

        public int? Age { get; set; }
    }
}
