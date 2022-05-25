using Licenta.ApplicationLogic.Services;
using Licenta.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministratorsController : Controller
    {
        public AdministratorsController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, EmployeeServices employeeServices,
            ILogger<AdministratorsController> logger)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            EmployeeServices = employeeServices;
            Logger = logger;
        }

        private readonly UserManager<IdentityUser> UserManager;
        private readonly RoleManager<IdentityRole> RoleManager;
        private readonly EmployeeServices EmployeeServices;
        private readonly ILogger<AdministratorsController> Logger;

        public IActionResult Index()
        {
            try
            {
                var users = UserManager.Users.ToList();
                Logger.LogInformation("Users were retrieved successfully");
                return View(users);
            }
            catch (Exception e)
            {
                Logger.LogDebug("Failed to retrieve users accounts {@Exception}", e);
                Logger.LogError("Failed to retrieve users accounts {Exception}", e.Message);
                return BadRequest();
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateUserAccount([FromForm] UserAccountCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = model.Name,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    EmailConfirmed = true
                };

                var createResult = await UserManager.CreateAsync(user, model.Password);
                if (createResult.Succeeded)
                {
                    var createdUser = await UserManager.FindByEmailAsync(model.Email);
                    EmployeeServices.AddEmployee(createdUser.Id, model.Name, model.Email, model.Role);
                    if (await RoleManager.FindByNameAsync(model.Role) == null)
                    {
                        var role = new IdentityRole(model.Role);
                        await RoleManager.CreateAsync(role);
                        await UserManager.AddToRoleAsync(user, role.Name);
                    }
                    else
                    {
                        await UserManager.AddToRoleAsync(user, model.Role);
                    }
                }
            }
            var users = UserManager.Users.ToList();
            return PartialView("_TablePartial", users);

        }
        [HttpGet]
        public async Task<IActionResult> EditUserAccount(string userId)
        {
            var user = await UserManager.FindByIdAsync(userId.ToString());
            UserAccontEditViewModel model = new UserAccontEditViewModel()
            {
                Name = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserId = userId.ToString(),
                Role = (await UserManager.GetRolesAsync(user)).FirstOrDefault()
            };
            return PartialView("_EditUserAccount", model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUserAccount([FromForm] UserAccontEditViewModel model)
        {
            if (ModelState.IsValid)
            {

                var userSaved = await UserManager.FindByIdAsync(model.UserId);
                var formerRole = await UserManager.GetRolesAsync(userSaved);
                userSaved.Email = model.Email;
                userSaved.UserName = model.Name;
                userSaved.PhoneNumber = model.PhoneNumber;
                await UserManager.UpdateAsync(userSaved);
                var roles = await UserManager.GetRolesAsync(userSaved);
                EmployeeServices.UpdateEmployee(model.Name, model.Email, formerRole[0], model.UserId);
                if (await UserManager.IsInRoleAsync(userSaved, model.Role) == false)
                {
                    await UserManager.RemoveFromRoleAsync(userSaved, roles[0]);
                    await UserManager.AddToRoleAsync(userSaved, model.Role);
                }
            }
            else
            {

                return View();

            }

            return PartialView("_EditUserAccount", model);
        }
        public IActionResult GetUsersPartialView()
        {
            var users = UserManager.Users.ToList();
            return PartialView("_TablePartial", users);
        }
        public async Task<IActionResult> DeleteUserAccount([FromRoute] string UserId)
        {
            try
            {
                var user = await UserManager.FindByIdAsync(UserId);
                await UserManager.DeleteAsync(user);
                EmployeeServices.DeleteEmployee(UserId);
            }
            catch (Exception e)
            {
                Logger.LogDebug("Failed to delete user account,most likely model was not correct {@Exception}", e);
                Logger.LogError("Failed to delete user account,most likely model was not correct {Exception}", e.Message);
                return BadRequest();
            }
            var users = UserManager.Users.ToList();
            return PartialView("_TablePartial", users);
        }

    }
}