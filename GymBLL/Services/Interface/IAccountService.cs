using GymBLL.ViewModels.AccountViewModels;
using GymDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymBLL.Services.Interface
{
    public interface IAccountService
    {
        ApplicationUser? Login(LoginViewModel userData);
    }
}
