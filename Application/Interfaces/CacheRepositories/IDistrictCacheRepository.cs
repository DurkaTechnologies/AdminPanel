using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.CacheRepositories
{
	public interface IDistrictCacheRepository
	{
		Task<List<District>> GetCachedListAsync();

		Task<District> GetByIdAsync(int brandId);
	}
}
