﻿using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces.Contexts
{
	public interface IApplicationDbContext
	{
		bool HasChanges { get; }

		EntityEntry Entry(object entity);

		Task<int> SaveChangesAsync(CancellationToken cancellationToken);
	}
}
