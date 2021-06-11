using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Identity;

namespace WebStore_MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AccountController(UserManager<User>userManager, SignInManager<User>signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public IActionResult Register() =>  View();
        public IActionResult Login() =>  View();
        public IActionResult Logout() =>  View(RedirectToAction("Index","Home"));
        public IActionResult AccessDenied() =>  View();



    }
}
