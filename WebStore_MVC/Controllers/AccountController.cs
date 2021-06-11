using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Identity;
using WebStore_MVC.ViewModels;

namespace WebStore_MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public IActionResult Register() => View(new RegisterViewModel());
        public async Task<IActionResult> Register(RegisterViewModel Model)
        {
            if (!ModelState.IsValid) return View(Model);
            var user = new User
            {
                UserName = Model.UserName
            };
            var createUser = await userManager.CreateAsync(user, Model.Password);
            if (createUser.Succeeded)
            {
                await signInManager.SignInAsync(user, false);
                RedirectToAction("Index", "Home");
            }
            foreach(var error in createUser.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(Model);
        }
        public IActionResult Login() => View();
        public IActionResult Logout() => View(RedirectToAction("Index", "Home"));
        public IActionResult AccessDenied() => View();



    }
}
