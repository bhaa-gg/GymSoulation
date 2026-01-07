using GymBLL.Services.Interface;
using GymBLL.ViewModels.AccountViewModels;
using GymDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymBLL.Services.Classes
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public ApplicationUser? Login(LoginViewModel userData)
        {
            var User = _userManager.FindByEmailAsync(userData.Email).Result;
            if (User is null || !_userManager.CheckPasswordAsync(User, userData.Password).Result) return null;
            return User;
        }
    }
}
