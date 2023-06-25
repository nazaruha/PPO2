using PPO2.Core.DTOs.ManufacturerDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.DTOs.ProductDto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ManufacturerId { get; set; }
        public string ManufacturerName { get; set; } = string.Empty;
        public List<ManufacturerDto.ManufacturerDto> Manufacturers { get; set; } = new List<ManufacturerDto.ManufacturerDto>();
    }
}
