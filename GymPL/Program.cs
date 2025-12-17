using GymBLL;
using GymDAL.Data.Contexts;
using GymDAL.Repositories.Classes;
using GymDAL.Repositories.Interfaces;
using GymPL.DataSeed;
using Microsoft.EntityFrameworkCore;
namespace GymPL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            #region Services


            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<GymDBContext>(options =>
            {
                //options.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings")["DefaultConnection"]);
                //options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            //builder.Services.AddScoped( typeof(IGenericRepository<>) , typeof (GenericRepository<>));
            //builder.Services.AddScoped<IPlanRepository ,PlanRepository>();


            builder.Services.AddScoped <IUnitOfWork ,UnitOfWork> ();
            builder.Services.AddScoped<ISessionRepository ,SessionRepository>();
            builder.Services.AddAutoMapper(X=>X.AddProfile(new MappingProfiles()));

            #endregion
            var app = builder.Build();




            #region Data Seeding
            using var Scope  = app.Services.CreateScope( );
            var gymContext = Scope.ServiceProvider.GetRequiredService<GymDBContext>();
            var PendingMigration = gymContext.Database.GetPendingMigrations();
            if (PendingMigration?.Any() ?? false) gymContext.Database.Migrate();

            GymDbContextSeeding.SeedData(gymContext);
            #endregion

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
