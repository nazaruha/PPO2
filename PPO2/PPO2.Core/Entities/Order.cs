using PPO2.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PPO2.Core.Entities
{
    [Table("Orders")]
    public class Order : IEntity
    {
        [Key]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        [JsonIgnore]
        public Customer Customer { get; set; } = new Customer();
        public int ProjectId { get; set; }
        [JsonIgnore]
        public Project Project { get; set; } = new Project();
        public int ProductId { get; set; }
        [JsonIgnore]
        public Product Product { get; set; } = new Product();
        public int TotalPrice { get; set; }
        public int ProductQuantity { get; set; }
        public DateTime SellDate { get; set; }
    }
}
