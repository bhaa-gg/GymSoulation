using GymDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace GymDAL.Data.Contexts
{
    public class GymDBContext : IdentityDbContext<ApplicationUser>
    {


        public GymDBContext (DbContextOptions<GymDBContext> options) : base(options) { }



        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server = . ; Database = GymManagement ; Trusted_Connection= true ; TrustServerCertificate = true");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating( modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Entity<ApplicationUser>(Ep => {
                Ep.Property(X => X.FirstName).HasColumnType("varchar").HasMaxLength(50);
                Ep.Property(X => X.LastName).HasColumnType("varchar").HasMaxLength(50);
            });
        }

        
        //public DbSet<ApplicationUser> Users { get; set; }
        //public DbSet<IdentityRole> Roles{ get; set; }
        //public DbSet<IdentityUserRole<string>> UserRoles{ get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<HealthRecord> HealthRecords{ get; set; }
        public DbSet<Trainer> Trainers{ get; set; }
        public DbSet<Plan> Plans{ get; set; }
        public DbSet<Category> Categories{ get; set; }
        public DbSet<MemberShip> MemberShips { get; set; }
        public DbSet<MemberSession> MemberSessions { get; set; }


    }
}
