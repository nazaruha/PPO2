using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.DTOs.Storage
{
    public class StorageSearchDto
    {
        public string? ProductName { get; set; }
        public string? ManufacturerName { get; set; }
        public int Price { get; set; }
        public string? Description { get; set; }
        public int Count { get; set; }
        public int Page { get; set; } = 1;
    }
}
