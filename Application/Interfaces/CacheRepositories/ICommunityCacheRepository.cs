using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.CacheRepositories
{
	public interface ICommunityCacheRepository
	{
		Task<List<Community>> GetCachedListAsync();

		Task<Community> GetByIdAsync(int brandId);
	}
}
