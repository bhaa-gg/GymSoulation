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
    internal class HealthRecordConfiguration : IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(EntityTypeBuilder<HealthRecord> builder) 
        { 
            builder.ToTable("Members").HasKey(H=>H.Id);

            builder.HasOne<Member>().WithOne(X=>X.HealthRecord).HasForeignKey<HealthRecord>(H=>H.Id);


            builder.Ignore(H => H.CreatedAt);
            builder.Ignore(H => H.UpdatedAt);

        }

    }
}
