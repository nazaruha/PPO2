using PPO2.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.DTOs.CustomerDto
{
    public class CustomerSearchDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public List<Project> Projects { get; set; } = new List<Project>();
        public int Page { get; set; } = 1;
        public int ProjectId { get; set; }
    }
}
