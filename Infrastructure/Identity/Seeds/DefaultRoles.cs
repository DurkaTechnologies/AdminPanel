using AdminPanel.Application.Enums;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace AdminPanel.Infrastructure.Identity.Seeds
{
	public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Worker.ToString()));
        }
    }
}
