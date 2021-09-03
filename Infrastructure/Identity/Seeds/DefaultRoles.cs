using Application.Enums;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Seeds
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
			if (await roleManager.FindByNameAsync(role.Name) == null)
				await roleManager.CreateAsync(role);
		}
	}
}
