using CarForum.Models;
using CarForum.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarForum.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;
        private IdentityRole identityRole;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, IdentityRole identityRole)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.identityRole = identityRole;
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = userManager.Users;
            return View(users);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                identityRole = new IdentityRole() { Name = model.RoleName };
                var result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }
                foreach (IdentityError item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;

            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string Id)
        {
            identityRole = await roleManager.FindByIdAsync(Id);

            if (identityRole == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {Id} cannot be found";
                return View("NotFound");
            }

            var model = new EditRoleViewModel()
            {
                Id = identityRole.Id,
                Name = identityRole.Name
            };

            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, identityRole.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {

                identityRole = await roleManager.FindByIdAsync(model.Id);

                if (identityRole == null)
                {
                    ViewBag.ErrorMessage = $"Role with id = {model.Id} cannot be found";
                    return View("NotFound");
                }

                else
                {
                    identityRole.Name = model.Name;
                    var result = await roleManager.UpdateAsync(identityRole);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            identityRole = await roleManager.FindByIdAsync(id);

            if (identityRole != null)
            {
                var result = await roleManager.DeleteAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                else
                {
                    ViewBag.ErrorMessage = $"Role with id = {id} cannot be delete";
                    return View("NotFound");
                }
            }

            return RedirectToAction("Home", "Index");    

        }
        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            identityRole = await roleManager.FindByIdAsync(roleId);

            if (identityRole == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {roleId} cannot be found";
                return View("NotFound");
            }
            else
            {
                ViewBag.RoleId = roleId;
                ViewBag.Name = identityRole.Name;

                var model = new List<UserRoleViewModel>();

                foreach (var user in userManager.Users)
                {
                    var userRoleViewModel = new UserRoleViewModel()
                    {
                        UserId = user.Id,
                        UserName = user.UserName
                    };
                    if (await userManager.IsInRoleAsync(user, identityRole.Name))
                    {
                        userRoleViewModel.IsSelected = true;
                    }
                    else
                    {
                        userRoleViewModel.IsSelected = false;
                    }
                    model.Add(userRoleViewModel);
                }
                return View(model);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Update(List<UserRoleViewModel> model, string roleId)
        {
            identityRole = await roleManager.FindByIdAsync(roleId);

            if (identityRole == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {roleId} cannot be found";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);
                IdentityResult result = null;

                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, identityRole.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, identityRole.Name);

                }
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, identityRole.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, identityRole.Name);

                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < model.Count-1)
                    {
                        continue;
                    }
                    else
                    {
                        return RedirectToAction("EditRole", new { id = roleId });
                    }
                }
            }

            return RedirectToAction("EditRole", new { id = roleId });
        }

    }
}
