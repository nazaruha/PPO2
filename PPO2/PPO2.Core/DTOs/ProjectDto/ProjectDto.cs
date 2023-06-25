using PPO2.Core.DTOs.CustomerDto;
using PPO2.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.DTOs.ProjectDto
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<CustomerDto.CustomerDto> Customers { get; set; } = new List<CustomerDto.CustomerDto>();
    }
}
