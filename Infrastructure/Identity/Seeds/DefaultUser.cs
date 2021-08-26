﻿using AdminPanel.Application.Enums;
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
                MiddleName = "Saloedovich",
                LastName = "Doe",
                CommunityId = null,
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
