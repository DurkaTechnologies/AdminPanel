using AdminPanel.Application.Enums;
using AdminPanel.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace AdminPanel.Infrastructure.Identity.Seeds
{
	public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));
        }
    }
}
