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
    [Table("Customers")]
    public class Customer : IEntity
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 50)]
        public string FirstName { get; set; } = string.Empty;
        [StringLength(maximumLength: 50)]
        public string SecondName { get; set; } = string.Empty;
        [StringLength(maximumLength: 150)]
        public string Email { get; set; } = string.Empty;
        [StringLength(maximumLength: 23)]
        public string Phone { get; set; } = string.Empty;
        [StringLength(maximumLength: 250)]
        public string? Address { get; set; } = string.Empty;
        [JsonIgnore]
        public List<Project> Projects { get; set; } = new List<Project>();
        [JsonIgnore]
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
