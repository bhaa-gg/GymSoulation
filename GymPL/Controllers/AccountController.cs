using GymBLL.Services.Interface;
using GymBLL.ViewModels.AccountViewModels;
using GymDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymPL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            IAccountService accountService,
            SignInManager<ApplicationUser> signInManager
            )
        {
            _accountService = accountService;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel LoginData)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("InvalidLogin", "Check Fields");
                return View(LoginData);
            }
            var User = _accountService.Login(LoginData);
            if(User is null)
            {
                ModelState.AddModelError("InvalidLogin", "Invalid Email or Password");
                return View(LoginData);
            }


            var Res = _signInManager.PasswordSignInAsync(User, LoginData.Password, LoginData.RememberMe, false).Result;

            if(Res.IsNotAllowed)
            {
                ModelState.AddModelError("InvalidLogin", "Your Account Is Not Allowed");
                return View(    LoginData);
            }
            if (Res.IsLockedOut)
            {
                ModelState.AddModelError("InvalidLogin", "Your Account Is Locked Out");
                return View(LoginData);
            }
            if (Res.Succeeded)
            {
                return RedirectToAction("Index","Home");
            }

            return View(LoginData);
        }




        [HttpPost]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync().GetAwaiter().GetResult();
            return RedirectToAction(nameof(Login));
        }


        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
