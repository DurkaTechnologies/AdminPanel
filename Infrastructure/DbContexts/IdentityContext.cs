﻿using Domain.Entities;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.DbContexts
{
	public class IdentityContext : IdentityDbContext<ApplicationUser>
	{
		public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
		{
		}

		public DbSet<Correspondence> Correspondences { get; set; }

		public DbSet<Community> Communities { get; set; }

		public DbSet<District> Districts { get; set; }

		public DbSet<Message> Messages { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
			builder.HasDefaultSchema("Identity");
			builder.Entity<ApplicationUser>(entity =>
			{
				entity.ToTable(name: "Users");
			});

			builder.Entity<IdentityRole>(entity =>
			{
				entity.ToTable(name: "Roles");
			});
			builder.Entity<IdentityUserRole<string>>(entity =>
			{
				entity.ToTable("UserRoles");
			});

			builder.Entity<IdentityUserClaim<string>>(entity =>
			{
				entity.ToTable("UserClaims");
			});

			builder.Entity<IdentityUserLogin<string>>(entity =>
			{
				entity.ToTable("UserLogins");
			});

			builder.Entity<IdentityRoleClaim<string>>(entity =>
			{
				entity.ToTable("RoleClaims");
			});

			builder.Entity<IdentityUserToken<string>>(entity =>
			{
				entity.ToTable("UserTokens");
			});

		}
	}
}
