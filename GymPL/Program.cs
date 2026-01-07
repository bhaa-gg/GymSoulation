using GymBLL;
using GymBLL.Services.AttachmentServices;
using GymBLL.Services.Classes;
using GymBLL.Services.Interface;
using GymDAL.Data.Contexts;
using GymDAL.Data.DataSeed;
using GymDAL.Entities;
using GymDAL.Repositories.Classes;
using GymDAL.Repositories.Interfaces;
using GymManagementSystemBLL.Services.Classes;
using GymManagementSystemBLL.Services.Interfaces;
using GymPL.DataSeed;
using Microsoft.AspNetCore.Identity;
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


            builder.Services.AddScoped <IUnitOfWork , UnitOfWork> ();
            builder.Services.AddScoped<ISessionRepository ,SessionRepository>();
            builder.Services.AddScoped<IAnalyticService, AnalyticService>();
            builder.Services.AddScoped<IAnalyticService, AnalyticService>();
            builder.Services.AddScoped<IMemberServices ,MemberServices>();
            builder.Services.AddScoped<ITrainerService , TrainerService>();
            builder.Services.AddScoped<IPlanServices, PlanServices>();
            builder.Services.AddScoped<ISessionServices, SessionServices>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IAttachmentServices, AttachmentServices>();
            builder.Services.AddAutoMapper(X=>X.AddProfile(new MappingProfiles()));



            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(Config =>
            {
                //Config.Password.RequiredLength = 6;
                //Config.Password.RequireLowercase = true;
                //Config.Password.RequireUppercase= true;
                Config.User.RequireUniqueEmail= true;

            })
                .AddEntityFrameworkStores<GymDBContext>();


            #region Use More in Api

            //builder.Services.AddIdentityCore<ApplicationUser>()
            //    .AddEntityFrameworkStores<GymDBContext>();

            #endregion

            builder.Services.ConfigureApplicationCookie(Config =>
            {
                Config.LoginPath = "/Account/Login";
                Config.AccessDeniedPath= "/Account/AccessDenied";

            });

            #endregion
            var app = builder.Build();




            #region Data Seeding
            using var Scope  = app.Services.CreateScope( );

            var gymContext = Scope.ServiceProvider.GetRequiredService<GymDBContext>();
            var roleManager = Scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = Scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var PendingMigration = gymContext.Database.GetPendingMigrations();
            if (PendingMigration?.Any() ?? false) gymContext.Database.Migrate();

            GymDbContextSeeding.SeedData(gymContext);
            IdentityDbContextSeeding.SeedData(roleManager , userManager);


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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();


            //app.MapControllerRoute(
            //   name: "Trainers",
            //   pattern: "coach/{action}",
            //   defaults : new{ controller = "Trainer" , action="Index" }
            //   );


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id:int?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
