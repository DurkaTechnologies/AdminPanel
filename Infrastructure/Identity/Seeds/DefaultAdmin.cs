using Application.Enums;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Seeds
{
	public static class DefaultAdminUser
	{
		private async static Task SeedClaimsForAdmin(this RoleManager<IdentityRole> roleManager)
		{
			var adminRole = await roleManager.FindByNameAsync("Admin");
			await roleManager.AddPermissionClaim(adminRole, "Communities");
			await roleManager.AddPermissionClaim(adminRole, "Districts");

		}

		public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			var defaultUser = new ApplicationUser
			{
				UserName = "admin",
				Email = "admin@gmail.com",
				FirstName = "Default",
				MiddleName = "Adminovich",
				LastName = "Admin",
				EmailConfirmed = true,
				PhoneNumberConfirmed = true,
				IsActive = true
			};

			if (await userManager.FindByNameAsync(defaultUser.UserName) == null)
			{
				await userManager.CreateAsync(defaultUser, "123Pa$$word!");
				await userManager.AddToRoleAsync(defaultUser, Roles.Worker.ToString());
				await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
				await roleManager.SeedClaimsForAdmin();

			}
		}
	}
}
