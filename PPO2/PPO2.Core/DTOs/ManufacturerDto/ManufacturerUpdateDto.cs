using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.DTOs.ManufacturerDto
{
    public class ManufacturerUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
