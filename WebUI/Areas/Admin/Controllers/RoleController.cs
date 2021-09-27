using Application.Enums;
using Infrastructure.AuditModels;
using Infrastructure.Identity.Models;
using WebUI.Abstractions;
using Application.Features.Logs.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebUI.Areas.Admin.Models;

namespace WebUI.Areas.Admin
{
	[Area("Admin")]
	[Authorize(Roles = "SuperAdmin")]
	public class RoleController : BaseController<RoleController>
	{
		#region Fields

		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<ApplicationUser> _userManager;

		#endregion

		public RoleController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}

		#region Main Controller Methods

		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> LoadAll()
		{
			var roles = await _roleManager.Roles.ToListAsync();
			return PartialView("_ViewAll", _mapper.Map<IEnumerable<RoleViewModel>>(roles));
		}

		public async Task<IActionResult> OnGetCreateOrEdit(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return new JsonResult(new
				{
					isValid = true,
					html = await _viewRenderer.RenderViewToStringAsync("_Create", new RoleViewModel())
				});
			}
			else
			{
				var roleViewModel = _mapper.Map<RoleViewModel>(await _roleManager.FindByIdAsync(id));

				return new JsonResult(new
				{
					isValid = true,
					html = await _viewRenderer.RenderViewToStringAsync("_Create", roleViewModel)
				});
			}
		}

		[HttpPost]
		public async Task<IActionResult> OnPostCreateOrEdit(RoleViewModel role)
		{
			if (ModelState.IsValid)
			{
				if (await _roleManager.FindByNameAsync(role.Name) != null)
				{
					_notify.Information($"Роль {role.Name} вже існує");
					return new JsonResult(new { isValid = true, html = await GetJSONRolesListAsync() });
				}

				if (string.IsNullOrEmpty(role.Id))
				{
					var result = await _roleManager.CreateAsync(new IdentityRole(role.Name));

					if (result.Succeeded)
					{
						Log log = new Log()
						{
							UserId = _userService.UserId,
							Action = "Create",
							TableName = "Roles",
							NewValues = role
						};

						await _mediator.Send(new AddLogCommand() { Log = log });
						_notify.Success($"Роль {role.Name} створено");
					}
					else
						_notify.Error($"Помилка при створені ролі {role.Name}");
				}
				else
				{
					var existingRole = await _roleManager.FindByIdAsync(role.Id);

						Log log = new Log()
						{
							UserId = _userService.UserId,
							Action = "Update",
							TableName = "Roles",
							Key = existingRole.Id,
							OldValues = _mapper.Map<RoleViewModel>(existingRole),
							NewValues = role
						};

					existingRole.Name = role.Name;
					existingRole.NormalizedName = role.Name.ToUpper();

					var result = await _roleManager.UpdateAsync(existingRole);
					if (result.Succeeded)
					{
						_notify.Success($"Роль {role.Name} змінено");
						await _mediator.Send(new AddLogCommand() { Log = log });
					}
					else
						_notify.Error($"Помилка при збережені змін до {role.Name}");
				}

				return new JsonResult(new { isValid = true, html = await GetJSONRolesListAsync() });
			}
			else
			{
				var html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", role);
				return new JsonResult(new { isValid = false, html = html });
			}
		}

		[HttpPost]
		public async Task<JsonResult> OnPostDelete(string id)
		{
			var existingRole = await _roleManager.FindByIdAsync(id);

			if (existingRole.Name != Roles.SuperAdmin.ToString() && existingRole.Name != Roles.Worker.ToString())
			{
				var allUsers = await _userManager.Users.ToListAsync();
				bool roleIsNotUsed = true;

				foreach (var user in allUsers)
					if (await _userManager.IsInRoleAsync(user, existingRole.Name))
					{
						roleIsNotUsed = false;
						break;
					}

				if (roleIsNotUsed)
				{
					if ((await _roleManager.DeleteAsync(existingRole)).Succeeded)
					{

						Log log = new Log()
						{
							UserId = _userService.UserId,
							Key = existingRole.Id,
							Action = "Delete",
							TableName = "Roles",
							OldValues = _mapper.Map<RoleViewModel>(existingRole)
						};

						await _mediator.Send(new AddLogCommand() { Log = log });
						_notify.Success($"Роль {existingRole.Name} видалено");
					}
					else
						_notify.Error($"Помилка при видалені ролі {existingRole.Name}");
				}
				else
					_notify.Warning($"Роль {existingRole.Name} використовується");
			}
			else
				_notify.Error($"Роль {existingRole.Name} не може бути видаленою");

			return new JsonResult(new { isValid = true, html = await GetJSONRolesListAsync() });
		}
		#endregion

		#region Other Methods
		private async Task<string> GetJSONRolesListAsync()
		{
			var roles = await _roleManager.Roles.ToListAsync();
			var mappedRoles = _mapper.Map<IEnumerable<RoleViewModel>>(roles);
			return await _viewRenderer.RenderViewToStringAsync("_ViewAll", mappedRoles);
		}

		#endregion
	}
}
