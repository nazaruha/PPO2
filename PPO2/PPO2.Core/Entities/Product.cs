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
    [Table("Products")]
    public class Product : IEntity
    {
        public int Id { get; set; }
        public int ManufacturerId { get; set; }
        [JsonIgnore]
        public Manufacturer Manufacturer { get; set; } = new Manufacturer();
        [StringLength(maximumLength: 50)]
        public string Name { get; set; } = string.Empty;
        [JsonIgnore]
        public List<Storage> Storage { get; set; } = new List<Storage>();
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
