using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PPO2.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Infrastructure.Configurations
{
    internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
       public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder
                .HasIndex(c => c.Email)
                .IsUnique();
            builder
                .HasIndex(c => c.Phone)
                .IsUnique();
        }
    }
}
