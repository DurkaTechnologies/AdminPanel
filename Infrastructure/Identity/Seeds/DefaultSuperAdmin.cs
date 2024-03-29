﻿using Application.Constants;
using Application.Enums;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Seeds
{
	public static class DefaultSuperAdminUser
	{
		public static async Task AddPermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
		{
			var allClaims = await roleManager.GetClaimsAsync(role);
			var allPermissions = Permissions.GeneratePermissionsForModule(module);
			foreach (var permission in allPermissions)
			{
				if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
				{
					await roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, permission));
				}
			}
		}

		private async static Task SeedClaimsForSuperAdmin(this RoleManager<IdentityRole> roleManager)
		{
			var adminRole = await roleManager.FindByNameAsync("SuperAdmin");
			await roleManager.AddPermissionClaim(adminRole, "Users");
			await roleManager.AddPermissionClaim(adminRole, "Communities");
			await roleManager.AddPermissionClaim(adminRole, "Districts");
		}

		public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			var defaultUser = new ApplicationUser
			{
				UserName = "superadmin",
				Email = "superadmin@gmail.com",
				FirstName = "Super",
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
				await userManager.AddToRoleAsync(defaultUser, Roles.SuperAdmin.ToString());
				await roleManager.SeedClaimsForSuperAdmin();
			}
		}
	}
}
