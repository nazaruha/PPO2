using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.DTOs.ProductManufacturerDto
{
    public class ProductManufacturerCreateDto
    {
        public int ProductId { get; set; }
        public int ManufacturerId { get; set; }
    }
}
