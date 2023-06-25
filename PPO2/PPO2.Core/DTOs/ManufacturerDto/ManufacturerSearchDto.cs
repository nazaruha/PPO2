using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.DTOs.ManufacturerDto
{
    public class ManufacturerSearchDto
    {
        //public int Id { get; set; }
        public string? Name { get; set; }
        public int Page { get; set; } = 1;
    }
}
