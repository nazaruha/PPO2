using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.DTOs.OrderDto
{
    public class OrderCreateDto
    {
        public int CustomerId { get; set; }
        public int ProjectId { get; set; }
        public int ProductId { get; set; }
        public int TotalPrice { get; set; }
        public int ProductQuantity { get; set; }
        public DateTime SellDate { get; set; }
    }
}
