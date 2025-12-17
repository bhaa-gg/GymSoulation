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
    internal class MemberShipConfiguration : IEntityTypeConfiguration<MemberShip>
    {
        public void Configure(EntityTypeBuilder<MemberShip> builder)
        {
            builder.Property(C => C.CreatedAt).HasColumnName("StartDate").HasDefaultValueSql("GETDATE()");


            builder.HasKey(MS => new { MS.PlanId , MS.MemberId  });
            builder.Ignore(MS =>MS.Id );



        }
    }
}
