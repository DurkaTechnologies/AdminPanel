using Application.Interfaces.Repositories;
using Application.Interfaces.Shared;
using Infrastructure.DbContexts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly IdentityContext dbContext;
		private readonly ApplicationDbContext applicationDbContext;
		private bool disposed;

		public UnitOfWork(IdentityContext dbContext, ApplicationDbContext applicationDbContext, IAuthenticatedUserService authenticatedUserService)
		{
			this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			this.applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
		}

		public async Task<int> Commit(CancellationToken cancellationToken)
		{
			return await dbContext.SaveChangesAsync(cancellationToken);
		}

		public async Task<int> CommitApplicationDb(CancellationToken cancellationToken)
		{
			return await applicationDbContext.SaveChangesAsync(cancellationToken);
		}

		public Task Rollback()
		{
			//todo
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					//dispose managed resources
					dbContext.Dispose();
				}
			}
			//dispose unmanaged resources
			disposed = true;
		}
	}
}
