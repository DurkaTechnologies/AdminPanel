using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
	public interface IRepositoryAsync<TEntity, TContext> where TEntity : class where TContext : DbContext
	{
		IQueryable<TEntity> Entities { get; }

		Task<TEntity> GetByIdAsync(int id);

		Task<List<TEntity>> GetAllAsync();

		Task<List<TEntity>> GetPagedReponseAsync(int pageNumber, int pageSize);

		Task<TEntity> AddAsync(TEntity entity);

		Task UpdateAsync(TEntity entity);

		Task DeleteAsync(TEntity entity);
	}
}
