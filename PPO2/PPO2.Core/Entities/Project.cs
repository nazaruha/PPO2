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
    [Table("Projects")]
    public class Project : IEntity
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 50)]
        public string Name { get; set; } = string.Empty;
        //[JsonIgnore]
        public List<Customer> Customers { get; set; } = new List<Customer>();
        [JsonIgnore]
        public List<Storage> Storage { get; set; } = new List<Storage>();
        //[JsonIgnore]
        public List<Plan> Plans { get; set; } = new List<Plan>();
        //[JsonIgnore]
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
