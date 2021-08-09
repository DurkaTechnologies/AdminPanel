using AdminPanel.Application.Enums;
using AdminPanel.Infrastructure.Identity.Models;
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
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Worker.ToString());
                }
            }
        }
    }
}
