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
    internal class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(X => X.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50);

            builder.Property(X => X.Email)
                .HasColumnType("varchar")
                .HasMaxLength(100);


            builder.Property(X => X.Phone)
              .HasColumnType("varchar")
              .HasMaxLength(11);



            builder.ToTable(Tb =>
            {
                Tb.HasCheckConstraint("GymUserValidEmail", "Email Like '_%@_%._%'");
                Tb.HasCheckConstraint("GymUserValidEmail", "Phone Like '01%' and Phone Not Like '%[^0-9]%'");
            });

            builder.HasIndex(X => X.Email).IsUnique();
            builder.HasIndex(X => X.Phone).IsUnique();


            builder.OwnsOne(X => X.Adress, Address => {

                Address.Property(A => A.City).HasColumnType("varchar").HasColumnName("City").HasMaxLength(30);
                Address.Property(A => A.Street).HasColumnType("varchar").HasColumnName("Street").HasMaxLength(30);
                Address.Property(A => A.BuildingNymber).HasColumnName("BuildingNymber");
            });
                
        }
    }
}
