using AdminPanel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading;
using System.Threading.Tasks;

namespace AdminPanel.Application.Interfaces.Contexts
{
	public interface IApplicationDbContext
	{
		bool HasChanges { get; }

		EntityEntry Entry(object entity);

		Task<int> SaveChangesAsync(CancellationToken cancellationToken);

		DbSet<Community> Communities { get; set; }
	}
}
