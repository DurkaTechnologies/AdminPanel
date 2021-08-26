using AdminPanel.Application.Enums;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.Infrastructure.Identity.Seeds
{
	public static class DefaultUser
	{
		public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
		{
			var defaultUser = new ApplicationUser
			{
				UserName = "testUser",
				Email = "testUser@gmail.com",
				FirstName = "John",
				MiddleName = "",
				LastName = "Doe",
				CommunityId = 0,
				EmailConfirmed = true,
				PhoneNumberConfirmed = true,
				IsActive = true
			};

			if (userManager.Users.All(u => u.UserName != defaultUser.UserName))
			{
				await userManager.CreateAsync(defaultUser, "123Pa$$word!");
				await userManager.AddToRoleAsync(defaultUser, Roles.Worker.ToString());
			}
		}
	}
}
