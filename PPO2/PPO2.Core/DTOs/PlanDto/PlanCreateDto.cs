using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.DTOs.PlanDto
{
    public class PlanCreateDto
    {
        public string Text { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}
