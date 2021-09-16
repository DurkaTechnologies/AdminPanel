using Application.Enums;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Seeds
{
	public static class DefaultUser
	{
		public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
		{
			var defaultUser = new ApplicationUser
			{
				UserName = "testUser",
				Email = "basicuser@gmail.com",
				FirstName = "John",
				MiddleName = "Saloedovich",
				LastName = "Doe",
				EmailConfirmed = true,
				PhoneNumberConfirmed = true,
				IsActive = true
			};

			if (await userManager.FindByNameAsync(defaultUser.UserName) == null)
			{
				await userManager.CreateAsync(defaultUser, "123Pa$$word!");
				await userManager.AddToRoleAsync(defaultUser, Roles.Worker.ToString());
			}
		}
	}
}
