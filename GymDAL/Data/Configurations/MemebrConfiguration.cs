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
    internal class MemebrConfiguration : GymUserConfiguration<Member> , IEntityTypeConfiguration<Member>{
        new public void Configure(EntityTypeBuilder<Member> builder) 
        { 
            builder.Property(M => M.CreatedAt).HasColumnName("JoinDate").HasDefaultValueSql("GETDATE()");
            builder.Property(M => M.Photo)
                .HasDefaultValue("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ5ExGEHlPHckD3YbxH6e4kr25Ho2X4NifiQA&s");



            base.Configure(builder);
        }

    }
}
