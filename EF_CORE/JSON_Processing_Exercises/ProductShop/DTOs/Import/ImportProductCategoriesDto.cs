using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ProductShop.DTOs.Import
{
    public class ImportProductCategoriesDto
    {
        public int CategoryId { get; set; }

        public int ProductId { get; set; }
    }
}
