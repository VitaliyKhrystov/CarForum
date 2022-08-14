using CarForum.Models;
using CarForum.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace CarForum.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;
        private IdentityRole identityRole;
        private User user;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, IdentityRole identityRole, User user)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.identityRole = identityRole;
            this.user = user;
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = userManager.Users;
            return View(users);
        }


        [HttpGet]
        public async Task<IActionResult> EditUser(string userId)
        {
           user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id: {userId} cannot be found";
                return View("NotFoundInfo");
            }

            var claims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);

            EditUserViewModel editUserViewModel = new EditUserViewModel()
            {
                Id = user.Id,
                NameUser = user.UserName,
                Email = user.Email,
                Claims = claims.Select(c => c.Value).ToList(),
                Roles = roles.ToList()
            };

            return View(editUserViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {

                user = await userManager.FindByIdAsync(model.Id);

                if (user == null)
                {
                    ViewBag.ErrorMessage = $"User with id: {model.Id} cannot be found";
                    return View("NotFoundInfo");
                }

                else
                {
                    user.UserName = model.NameUser;
                    user.Email = model.Email;

                    var result = await userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListUsers");
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
        public async Task<IActionResult> DeleteUser(string userId)
        {
            user = await userManager.FindByIdAsync(userId);

            if (user != null)
            {
                await userManager.DeleteAsync(user);
            }

            return RedirectToAction("ListUsers", "Administration");
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRole(string userId)
        {
            if (userId == null)
            {
                ViewBag.ErrorMessage = $"User with id: {userId} cannot be found";
                return View("NotFoundInfo");
            }
            user = await userManager.FindByIdAsync(userId);
            ViewBag.UserId = user.Id;
            ViewBag.UserName = user.UserName;

            var model = new List<UserRolesViewModel>();
            
            foreach (var role in roleManager.Roles)
            {
                var userRole = new UserRolesViewModel()
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRole.IsSelected = true;
                }
                else
                {
                    userRole.IsSelected = false;
                }
                model.Add(userRole);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserRole(List<UserRolesViewModel> model, string userId)
        {
            if (userId == null)
            {
                ViewBag.ErrorMessage = $"User with id: {userId} cannot be found";
                return View("NotFoundInfo");
            }
            if (ModelState.IsValid)
            {
                user = await userManager.FindByIdAsync(userId);
                IdentityResult result = null;

                for (int i = 0; i < model.Count; i++)
                {
                    if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, model[i].RoleName)))
                    {
                        result = await userManager.AddToRoleAsync(user, model[i].RoleName);
                    }
                    else if (!(model[i].IsSelected) && await userManager.IsInRoleAsync(user, model[i].RoleName))
                    {
                        result = await userManager.RemoveFromRoleAsync(user, model[i].RoleName);
                    }
                    else
                    {
                        continue;
                    }
                    if (result.Succeeded)
                    {
                        if (i < model.Count - 1)
                        {
                            continue;
                        }
                        else
                        {
                            return RedirectToAction("EditUser", new { userId = user.Id });
                        }
                    }
                }
                return RedirectToAction("EditUser", new { userId = user.Id });
            }

            return View(model);
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
                ViewBag.ErrorMessage = $"Role with id: {Id} cannot be found";
               return View("NotFoundInfo");
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
                   ViewBag.ErrorMessage = $"Role with id: {model.Id} cannot be found";
                   return View("NotFoundInfo");
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
        public async Task<IActionResult> DeleteRole(string id)
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
                    ViewBag.ErrorMessage = $"Role with id: {id} cannot be delete";
                   return View("NotFoundInfo");
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
                ViewBag.ErrorMessage = $"Role with id: {roleId} cannot be found";
               return View("NotFoundInfo");
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
                ViewBag.ErrorMessage = $"Role with id: {roleId} cannot be found";
               return View("NotFoundInfo");
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
