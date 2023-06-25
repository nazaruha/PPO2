using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.DTOs.CustomerDto
{
    public class CustomerSearchResultDto
    {
        public List<CustomerDto> Customers { get; set; }
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
        public int Total { get; set; }
        public string ProjectName { get; set; }
        public int ProjectId { get; set; }
    }
}
