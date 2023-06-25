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
    [Table("Plans")]
    public class Plan : IEntity
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 500)]
        public string Text { get; set; } = string.Empty;
        public DateTime? Date { get; set; }
        [JsonIgnore]
        public List<Project> Projects { get; set; } = new List<Project>();
    }
}
