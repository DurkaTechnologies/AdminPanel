using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
	public interface IUnitOfWork : IDisposable
	{
		Task<int> Commit(CancellationToken cancellationToken);

		Task<int> CommitApplicationDb(CancellationToken cancellationToken);

		Task Rollback();
	}
}
