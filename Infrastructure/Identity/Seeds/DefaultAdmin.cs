using AdminPanel.Application.Enums;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.Infrastructure.Identity.Seeds
{
	public static class DefaultAdminUser
	{
		private async static Task SeedClaimsForAdmin(this RoleManager<IdentityRole> roleManager)
		{
			var adminRole = await roleManager.FindByNameAsync("Admin");
			await roleManager.AddPermissionClaim(adminRole, "Communities");
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
				CommunityId = null,
				EmailConfirmed = true,
				PhoneNumberConfirmed = true,
				IsActive = true
			};

			if (userManager.Users.All(u => u.UserName != defaultUser.UserName))
			{
				await userManager.CreateAsync(defaultUser, "123Pa$$word!");
				await userManager.AddToRoleAsync(defaultUser, Roles.Worker.ToString());
				await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
				await roleManager.SeedClaimsForAdmin();
			}
		}
	}
}
