using AdminPanel.Infrastructure.AuditModels;
using Infrastructure.Identity.Models;
using AdminPanel.Web.Abstractions;
using Application.Features.Logs.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Areas.Admin.Models;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class UserRoleController : BaseController<UserRoleController>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRoleController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index(string userId)
        {
            var viewModel = new List<UserRolesViewModel>();
            var user = await _userManager.FindByIdAsync(userId);
            ViewData["Title"] = $"{user.UserName} - Ролі";
            ViewData["Caption"] = $"Керувати ролями {user.Email}";

            foreach (var role in _roleManager.Roles.ToList())
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                viewModel.Add(userRolesViewModel);
            }
            var model = new ManageUserRolesViewModel()
            {
                UserId = userId,
                UserRoles = viewModel
            };

            return View(model);
        }

        public async Task<IActionResult> Update(string id, ManageUserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            result = await _userManager.AddToRolesAsync(user, model.UserRoles.Where(x => x.Selected).Select(y => y.RoleName));
            var currentUser = await _userManager.GetUserAsync(User);
            await _signInManager.RefreshSignInAsync(currentUser);
            await AdminPanel.Infrastructure.Identity.Seeds.DefaultSuperAdminUser.SeedAsync(_userManager, _roleManager);
            var newRoles = await _userManager.GetRolesAsync(user);

            _notify.Success($"Ролі для {user.FirstName + " " + user.LastName} змінено");

            Log log = new Log()
            {
                UserId = _userService.UserId,
                Action = "Manage Roles",
                TableName = "Users",
                OldValues = roles,
                NewValues = newRoles,
                Key = user.Id
            };

            await _mediator.Send(new AddLogCommand() { Log = log });

            return RedirectToAction("Index", new { userId = id });
        }
    }
}
