using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.DTOs.OrderDto
{
    public class OrderUpdateDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        //public CustomerDto.CustomerDto Customer { get; set; }
        public int ProjectId { get; set; }
        //public ProjectDto.ProjectDto Project { get; set; }
        public int ProductId { get; set; }
        //public ProductDto.ProductDto Product { get; set; }
        public int TotalPrice { get; set; }
        public int ProductQuantity { get; set; }
        public DateTime SellDate { get; set; }
    }
}
