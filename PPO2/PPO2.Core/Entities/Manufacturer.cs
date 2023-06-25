using PPO2.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.Entities
{
    [Table("Manufacturers")]
    public class Manufacturer : IEntity
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 50)]
        public string Name { get; set; } = string.Empty;
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
