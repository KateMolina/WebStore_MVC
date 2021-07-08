using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Identity;
using WebStore_MVC.ViewModels;

namespace WebStore_MVC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ILogger<AccountController> logger;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountController> logger)        
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
        }
        [AllowAnonymous]
        public IActionResult Register() => View(new RegisterViewModel());

        [AllowAnonymous]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel Model)
        {
            if (!ModelState.IsValid) return View(Model);
            logger.LogInformation("Registering new user {0}", Model.UserName);
            var user = new User
            {
                UserName = Model.UserName
            };
            var createUser = await userManager.CreateAsync(user, Model.Password);
            if (createUser.Succeeded)
            {
                await signInManager.SignInAsync(user, false);
                logger.LogInformation("New user is successfully registered {0}", Model.UserName);
                return RedirectToAction("Index", "Home");
            }
            foreach (var error in createUser.Errors)
            {
                ModelState.AddModelError("", error.Description);
                logger.LogWarning("Error occured during registration of user {0} in the system {1}", Model.UserName, string.Join(",", createUser.Errors.Select(e => e.Description)));
            }
            return View(Model);
        }
        [AllowAnonymous]
        public IActionResult Login(string returnUrl) => View(new LoginViewModel() { ReturnUrl = returnUrl });
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var signInResult = await signInManager.PasswordSignInAsync(
                model.UserName,
                model.Password,
                model.RememberMe,
#if DEBUG
                false
#else
                true
#endif
                );
            if (signInResult.Succeeded)
            {
                logger.LogInformation("User {0} successfully signed in to the system", model.UserName);
                return LocalRedirect(model.ReturnUrl ?? "/");
            }
            ModelState.AddModelError("", "User name or password is incorrect");
            logger.LogWarning("Credentials used by  {0} to sign in to the system",
                model.UserName);
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            var user_name = User.Identity!.Name;
          await signInManager.SignOutAsync();
            logger.LogInformation("Пользователь {0} вышел из системы", user_name);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult AccessDenied() => View();



    }
}
