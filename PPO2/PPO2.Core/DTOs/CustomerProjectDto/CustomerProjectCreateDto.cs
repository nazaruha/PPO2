using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.DTOs.CustomerProjectDto
{
    public class CustomerProjectCreateDto
    {
        public int CustomerId { get; set; }
        public int ProjectId { get; set; }
    }
}
