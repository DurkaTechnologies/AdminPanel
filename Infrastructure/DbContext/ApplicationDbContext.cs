using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Infrastructure.DbContext
{
	public class ApplicationDbContext
	{
		//private readonly IDateTimeService _dateTime;
		//private readonly IAuthenticatedUserService _authenticatedUser;

		//public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTimeService dateTime, IAuthenticatedUserService authenticatedUser) : base(options)
		//{
		//	_dateTime = dateTime;
		//	_authenticatedUser = authenticatedUser;
		//}

		//public IDbConnection Connection => Database.GetDbConnection();

		//public bool HasChanges => ChangeTracker.HasChanges();

		//public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
		//{
		//	foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
		//	{
		//		switch (entry.State)
		//		{
		//			case EntityState.Added:
		//				entry.Entity.CreatedBy = _currentUserService.UserId;
		//				entry.Entity.Created = _dateTime.Now;
		//				break;

		//			case EntityState.Modified:
		//				entry.Entity.LastModifiedBy = _currentUserService.UserId;
		//				entry.Entity.LastModified = _dateTime.Now;
		//				break;
		//		}
		//	}

		//	var result = await base.SaveChangesAsync(cancellationToken);

		//	await DispatchEvents();

		//	return result;
		//}

		//protected override void OnModelCreating(ModelBuilder builder)
		//{
		//	foreach (var property in builder.Model.GetEntityTypes()
		//	.SelectMany(t => t.GetProperties())
		//	.Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
		//	{
		//		property.SetColumnType("decimal(18,2)");
		//	}
		//	base.OnModelCreating(builder);
		//}

		//private async Task DispatchEvents()
		//{
		//	while (true)
		//	{
		//		var domainEventEntity = ChangeTracker.Entries<IHasDomainEvent>()
		//			.Select(x => x.Entity.DomainEvents)
		//			.SelectMany(x => x)
		//			.Where(domainEvent => !domainEvent.IsPublished)
		//			.FirstOrDefault();
		//		if (domainEventEntity == null) break;

		//		domainEventEntity.IsPublished = true;
		//		await _domainEventService.Publish(domainEventEntity);
		//	}
		//}
	}
}
