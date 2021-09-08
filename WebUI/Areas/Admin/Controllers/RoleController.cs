using Application.Enums;
using Infrastructure.AuditModels;
using Infrastructure.Identity.Models;
using WebUI.Abstractions;
using Application.Features.Logs.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebUI.Areas.Admin.Models;

namespace WebUI.Areas.Admin
{
	[Area("Admin")]
	[Authorize(Roles = "SuperAdmin")]
	public class RoleController : BaseController<RoleController>
	{
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<ApplicationUser> _userManager;

		public RoleController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> LoadAll()
		{
			var roles = await _roleManager.Roles.ToListAsync();
			var model = _mapper.Map<IEnumerable<RoleViewModel>>(roles);

			return PartialView("_ViewAll", model);
		}

		public async Task<IActionResult> OnGetCreate(string id)
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
				var role = await _roleManager.FindByIdAsync(id);
				var roleviewModel = _mapper.Map<RoleViewModel>(role);

				return new JsonResult(new
				{
					isValid = true,
					html = await _viewRenderer.RenderViewToStringAsync("_Create", roleviewModel)
				});
			}
		}

		[HttpPost]
		public async Task<IActionResult> OnPostCreate(RoleViewModel role)
		{
			if (ModelState.IsValid)
			{
				var result = await _roleManager.FindByNameAsync(role.Name);

				if (result == null)
				{
					if (string.IsNullOrEmpty(role.Id))
					{
						_notify.Success($"Роль {role.Name} створено");
						await _roleManager.CreateAsync(new IdentityRole(role.Name));

						Log log = new Log()
						{
							UserId = _userService.UserId,
							Action = "Create",
							TableName = "Roles",
							NewValues = role
						};

						await _mediator.Send(new AddLogCommand() { Log = log });
					}
					else
					{
						var existingRole = await _roleManager.FindByIdAsync(role.Id);

						Log log = new Log()
						{
							UserId = _userService.UserId,
							Action = "Update",
							TableName = "Roles",
							OldValues = _mapper.Map<RoleViewModel>(existingRole),
							NewValues = role
						};

						existingRole.Name = role.Name;
						existingRole.NormalizedName = role.Name.ToUpper();
						await _roleManager.UpdateAsync(existingRole);

						_notify.Success($"Роль {role.Name} змінено");
						await _mediator.Send(new AddLogCommand() { Log = log });
					}
				}
				else
				{
					_notify.Error($"Роль {role.Name} вже існує");
				}

				var roles = await _roleManager.Roles.ToListAsync();
				var mappedRoles = _mapper.Map<IEnumerable<RoleViewModel>>(roles);
				var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", mappedRoles);

				return new JsonResult(new { isValid = true, html = html });
			}
			else
			{
				var html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", role);
				return new JsonResult(new { isValid = false, html = html });
			}
		}

		public async Task<JsonResult> OnPostDelete(string id)
		{
			var existingRole = await _roleManager.FindByIdAsync(id);

			if (existingRole.Name != Roles.SuperAdmin.ToString())
			{
				bool roleIsNotUsed = true;
				// stupit проверка.
				var allUsers = await _userManager.Users.ToListAsync();

				foreach (var user in allUsers)
					if (await _userManager.IsInRoleAsync(user, existingRole.Name))
						roleIsNotUsed = false;
				// до сюда
				if (roleIsNotUsed)
				{
					await _roleManager.DeleteAsync(existingRole);

					Log log = new Log()
					{
						UserId = _userService.UserId,
						Action = "Delete",
						TableName = "Roles",
						OldValues = _mapper.Map<RoleViewModel>(existingRole)
					};

					await _mediator.Send(new AddLogCommand() { Log = log });
					_notify.Success($"Роль {existingRole.Name} видалено");
				}
				else
					_notify.Error($"Роль {existingRole.Name} використовується");
			}
			else
				_notify.Error($"Роль {existingRole.Name} не може бути видаленою");

			var roles = await _roleManager.Roles.ToListAsync();
			var mappedRoles = _mapper.Map<IEnumerable<RoleViewModel>>(roles);
			var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", mappedRoles);

			return new JsonResult(new { isValid = true, html = html });
		}
	}
}
