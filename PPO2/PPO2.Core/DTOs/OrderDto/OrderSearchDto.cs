using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.DTOs.OrderDto
{
    public class OrderSearchDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string? CustomerFirstName { get; set; }
        public string? CustomerSecondName { get; set; }
        public string? ProductName { get; set; }
        public string? ManufacturerName { get; set; }
        public int TotalPrice { get; set; }
        public int ProductQuantity { get; set; }
        public DateTime SellDate { get; set; }
        public int Page { get; set; } = 1;
    }
}
