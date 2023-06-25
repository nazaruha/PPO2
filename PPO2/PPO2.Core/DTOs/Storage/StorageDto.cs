using PPO2.Core.DTOs.ProductDto;
using PPO2.Core.DTOs.ProjectDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.DTOs.Storage
{
    public class StorageDto
    {
        public int ProductId { get; set; }
        public ProductDto.ProductDto Product { get; set; } = new ProductDto.ProductDto();
        public int ProjectId { get; set; }
        public ProjectDto.ProjectDto Project{ get; set; } = new ProjectDto.ProjectDto();
        public int Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Count { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
