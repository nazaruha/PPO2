using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.DTOs.Storage
{
    public class StorageCreateDto
    {
        public int ProductId { get; set; }
        public int ProjectId { get; set; }
        public int Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Count { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
