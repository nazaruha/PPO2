using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.DTOs.OrderDto
{
    public class OrderSearchDateDto
    {
        public string? Day { get; set; }
        public string? Month { get; set; }
        public string? Year { get; set; }
        public int ProjectId { get; set; }
        public int Page { get; set; } = 1;
    }
}
