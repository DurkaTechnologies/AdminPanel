using Application.Interfaces.Repositories;
using Infrastructure.DbContexts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
	public class DistrictRepository : IDistrictRepository
	{
		private readonly IRepositoryAsync<District, IdentityContext> _repository;
		private readonly IDistributedCache _distributedCache;

		public DistrictRepository(IDistributedCache distributedCache, IRepositoryAsync<District, IdentityContext> repository)
		{
			_distributedCache = distributedCache;
			_repository = repository;
		}

		public IQueryable<District> Communities => _repository.Entities;

		public async Task DeleteAsync(District district)
		{
			await _repository.DeleteAsync(district);
			await _distributedCache.RemoveAsync(CacheKeys.DistrictCacheKeys.ListKey);
			await _distributedCache.RemoveAsync(CacheKeys.DistrictCacheKeys.GetKey(district.Id));
		}

		public async Task<District> GetByIdAsync(int districtId)
		{
			return await _repository.Entities.Where(d => d.Id == districtId).FirstOrDefaultAsync();
		}

		public async Task<District> GetIncludeByIdAsync(int districtId)
		{
			return await _repository.Entities.Where(d => d.Id == districtId).Include(d => d.Communities).FirstOrDefaultAsync();
		}

		public async Task<List<District>> GetListAsync()
		{
			return await _repository.Entities.ToListAsync();
		}

		public async Task<int> InsertAsync(District district)
		{
			await _repository.AddAsync(district);
			await _distributedCache.RemoveAsync(CacheKeys.DistrictCacheKeys.ListKey);
			return district.Id;
		}

		public async Task UpdateAsync(District district)
		{
			await _repository.UpdateAsync(district);
			await _distributedCache.RemoveAsync(CacheKeys.DistrictCacheKeys.ListKey);
			await _distributedCache.RemoveAsync(CacheKeys.DistrictCacheKeys.GetKey(district.Id));
		}
	}
}
