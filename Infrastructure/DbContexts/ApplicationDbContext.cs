﻿using Application.Interfaces.Contexts;
using Application.Interfaces.Shared;
using Domain.Common.Models;
using Infrastructure.AuditModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.DbContexts
{
	public class ApplicationDbContext : AuditableContext, IApplicationDbContext
	{
		//private readonly IDateTimeService _dateTime;
		//private readonly IAuthenticatedUserService _authenticatedUser;

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
			//_authenticatedUser = currentUserService;
			//_dateTime = dateTime;
		}

		public bool HasChanges => ChangeTracker.HasChanges();

		//public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
		//{
		//	foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
		//	{
		//		switch (entry.State)
		//		{
		//			case EntityState.Added:
		//				entry.Entity.CreatedBy = _authenticatedUser.UserId;
		//				entry.Entity.Created = _dateTime.NowUtc;
		//				break;

		//			case EntityState.Modified:
		//				entry.Entity.LastModifiedBy = _authenticatedUser.UserId;
		//				entry.Entity.LastModified = _dateTime.NowUtc;
		//				break;
		//		}
		//	}

		//	if (String.IsNullOrEmpty(_authenticatedUser.UserId))
		//		return await base.SaveChangesAsync(cancellationToken);
		//	return await base.SaveChangesAsync(_authenticatedUser.UserId);
		//}

		//protected override void OnModelCreating(ModelBuilder builder)
		//{
		//	base.OnModelCreating(builder);
		//}
	}
}
