using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.DTOs.ProductDto
{
    public class ProductCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public int ManufacturerId { get; set; }
    }
}
