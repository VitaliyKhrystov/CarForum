using CarForum.Domain;
using CarForum.Models;
using CarForum.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarForum.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private User user;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, User user, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.user = user;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerModel)
        {
            if (ModelState.IsValid)
            {
                user = new User { UserName = registerModel.Email,  Email = registerModel.Email, Date = registerModel.Date };

                var addNewUser = await userManager.CreateAsync(user, registerModel.Password);

                if (addNewUser.Succeeded)
                {
                    if (await roleManager.RoleExistsAsync("User"))
                    {
                        await userManager.AddToRoleAsync(user, "User");
                    }

                    if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Administration");
                    }

                        await signInManager.SignInAsync(user, false); 
                        return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var item in addNewUser.Errors)
                    {
                        ModelState.AddModelError(string.Empty, item.Description);
                    }
                }

            }

            return View(registerModel);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, loginModel.Remember, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(loginModel.ReturnUrl) && Url.IsLocalUrl(loginModel.ReturnUrl))
                    {
                        return Redirect(loginModel.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Wrong email and/or password!");
                }
            }
            return View(loginModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
