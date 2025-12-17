using GymDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymDAL.Data.Configurations
{
    internal class TrainerConfiguration : GymUserConfiguration<Trainer> , IEntityTypeConfiguration<Trainer>
    {
        new public void Configure(EntityTypeBuilder<Trainer> builder) 
        { 
            builder.Property(M => M.CreatedAt).HasColumnName("HireDate").HasDefaultValueSql("GETDATE()");


                
            base.Configure(builder);
        }
    }
}
