using AdminPanel.Application.Enums;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.Infrastructure.Identity.Seeds
{
	public static class DefaultRoles
	{
		public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
		{
			await SeedRoleAsync(roleManager, new IdentityRole(Roles.SuperAdmin.ToString()));
			await SeedRoleAsync(roleManager, new IdentityRole(Roles.Admin.ToString()));
			await SeedRoleAsync(roleManager, new IdentityRole(Roles.Worker.ToString()));
		}

		private static async Task SeedRoleAsync(RoleManager<IdentityRole> roleManager, IdentityRole role)
		{
			if (roleManager.Roles.All(r => r.Name != role.Name))
				await roleManager.CreateAsync(role);
		}
	}
}
