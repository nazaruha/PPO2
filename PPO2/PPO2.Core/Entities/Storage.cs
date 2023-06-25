using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PPO2.Core.Entities
{
    [PrimaryKey(nameof(ProductId), nameof(ProjectId))]
    public class Storage
    {
        public int ProductId { get; set; }
        //[JsonIgnore]
        public Product Product { get; set; } = new Product();
        public int ProjectId { get; set; }
        //[JsonIgnore]
        public Project Project { get;set; } = new Project();
        public int Price { get; set; }
        [AllowNull, StringLength(maximumLength:255)]
        public string? Description { get; set; }
        public int Count { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
