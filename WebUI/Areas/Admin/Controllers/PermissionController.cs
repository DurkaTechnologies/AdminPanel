using Application.Constants;
using WebUI.Abstractions;
using WebUI.Areas.Admin.Models;
using WebUI.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WebUI.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "SuperAdmin")]
	public class PermissionController : BaseController<PermissionController>
	{
		private readonly RoleManager<IdentityRole> _roleManager;

		public PermissionController(RoleManager<IdentityRole> roleManager)
		{
			_roleManager = roleManager;
		}

		public async Task<ActionResult> Index(string roleId)
		{
			var model = new PermissionViewModel();
			var allPermissions = new List<RoleClaimsViewModel>();
			allPermissions.GetPermissions(typeof(Permissions.Communities), roleId);
			allPermissions.GetPermissions(typeof(Permissions.Users), roleId);
			allPermissions.GetPermissions(typeof(Permissions.Districts), roleId);

			var role = await _roleManager.FindByIdAsync(roleId);
			model.RoleId = roleId;
			var claims = await _roleManager.GetClaimsAsync(role);
			var claimsModel = _mapper.Map<List<RoleClaimsViewModel>>(claims);
			var allClaimValues = allPermissions.Select(a => a.Value).ToList();
			var roleClaimValues = claimsModel.Select(a => a.Value).ToList();
			var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();

			foreach (var permission in allPermissions)
			{
				if (authorizedClaims.Any(a => a == permission.Value))
					permission.Selected = true;
			}

			model.RoleClaims = _mapper.Map<List<RoleClaimsViewModel>>(allPermissions);
			ViewData["Title"] = $"Права для {role.Name}";
			ViewData["Caption"] = $"Керувати правами для {role.Name}.";

			return View(model);
		}

		public async Task<IActionResult> Update(PermissionViewModel model)
		{
			var role = await _roleManager.FindByIdAsync(model.RoleId);

			var claims = await _roleManager.GetClaimsAsync(role);
			foreach (var claim in claims)
				await _roleManager.RemoveClaimAsync(role, claim);

			var selectedClaims = model.RoleClaims.Where(a => a.Selected).ToList();
			foreach (var claim in selectedClaims)
				await _roleManager.AddPermissionClaim(role, claim.Value);

			_notify.Success($"Дозволи для ролі {role.Name} змінено");
			return RedirectToAction("Index", new { roleId = model.RoleId });
		}
	}
}
