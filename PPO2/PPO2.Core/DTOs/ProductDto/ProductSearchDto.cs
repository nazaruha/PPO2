using PPO2.Core.DTOs.ManufacturerDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.DTOs.ProductDto
{
    public class ProductSearchDto
    {
        public string? Name { get; set; }
        public string? ManufacturerName { get; set; }
        public int Page { get; set; } = 1;

    }
}
