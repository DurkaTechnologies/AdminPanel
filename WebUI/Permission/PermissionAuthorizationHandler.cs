using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Permission
{
	internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
	{
		private UserManager<ApplicationUser> _userManager;
		private RoleManager<IdentityRole> _roleManager;

		public PermissionAuthorizationHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}

		protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
		{
			if (context.User == null)
				return;

			var permissionss = context.User.Claims.Where(x => x.Type == "Permission" &&
															 x.Value == requirement.Permission &&
															 x.Issuer == "LOCAL AUTHORITY");
			if (permissionss.Any())
			{
				context.Succeed(requirement);
				return;
			}
		}
	}
}
