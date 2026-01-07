using GymDAL.Constant;
using GymDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymDAL.Data.DataSeed
{
    public class IdentityDbContextSeeding

    {
        public static bool SeedData(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager) {

            try {

                var HasUsers = userManager.Users.Any();
                var HasRoles= roleManager.Roles.Any();
                if (HasUsers && HasRoles) return false;


                if (!HasRoles) {
                    var roles = new List<IdentityRole>() {
                        new (){Name=Role.SuperAdmin.ToString()},
                        new (){Name=Role.Admin.ToString()}
                    };


                    foreach (var role in roles)
                    {
                        if(!roleManager.RoleExistsAsync(role?.Name!).Result) roleManager.CreateAsync(role).Wait();
                    }

                }


                if (!HasUsers)
                {
                    var MainAdmin = new ApplicationUser() { 
                        FirstName = "Bahaa",
                        LastName= "Wafy",
                        UserName= "Bahaa_Wafy",
                        Email= "Bahaa_Wafy@gmail.com",
                        PhoneNumber = "01210031428"
                    };
                    userManager.CreateAsync(MainAdmin , "Haa@123456").Wait();
                    userManager.AddToRoleAsync(MainAdmin, Role.SuperAdmin.ToString()).Wait();

                    var Admin = new ApplicationUser()
                    {
                        FirstName = "Asmaa",
                        LastName = "Ebrahem",
                        UserName = "Asmaa_Ebrahem",
                        Email = "Asmaa_Ebrahem@gmail.com",
                        PhoneNumber = "01284927600"
                    };
                    userManager.CreateAsync(Admin, "Haa@1234567").Wait();
                    userManager.AddToRoleAsync(Admin, Role.Admin.ToString()).Wait();

                }

                return true;

            }
            catch (Exception err ) {
                Console.WriteLine($"Err in seed data {err}");
                return false;
            }
        }
    }
}
