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
    internal class MemberSessionConfiguration : IEntityTypeConfiguration<MemberSession>
    {
        public void Configure(EntityTypeBuilder<MemberSession> builder)
        {
            builder.Ignore(C => C.Id);
            builder.Property(C => C.CreatedAt).HasColumnName("BookingDate").HasDefaultValueSql("GETDATE()");


            builder.HasKey(MS => new { MS.SessionId, MS.MemberId  });



        }
    }
}
