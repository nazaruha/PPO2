using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.DTOs.ManufacturerDto
{
    public class ManufacturerSearchResultDto
    {
        public List<ManufacturerDto> Manufacturers { get; set; }
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
        public int Total { get; set; }
    }
}
