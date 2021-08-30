using AdminPanel.Application.Interfaces.CacheRepositories;
using AdminPanel.Application.Interfaces.Repositories;
using AdminPanel.Infrastructure.Extensions;
using Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminPanel.Infrastructure.CacheRepositories
{
	public class DistrictCacheRepository : IDistrictCacheRepository
	{
		private readonly IDistributedCache distributedCache;
		private readonly IDistrictRepository districtRepository;

		public DistrictCacheRepository(IDistributedCache distributedCache, IDistrictRepository districtRepository)
		{
			this.distributedCache = distributedCache;
			this.districtRepository = districtRepository;
		}

		public async Task<District> GetByIdAsync(int districtId)
		{
			string cacheKey = CacheKeys.DistrictCacheKeys.GetKey(districtId);
			var brand = await distributedCache.GetAsync<District>(cacheKey);
			if (brand == null)
			{
				brand = await districtRepository.GetByIdAsync(districtId);
				await distributedCache.SetAsync(cacheKey, brand);
			}
			return brand;
		}

		public async Task<List<District>> GetCachedListAsync()
		{
			string cacheKey = CacheKeys.DistrictCacheKeys.ListKey;
			var brandList = await distributedCache.GetAsync<List<District>>(cacheKey);
			if (brandList == null)
			{
				brandList = await districtRepository.GetListAsync();
				await distributedCache.SetAsync(cacheKey, brandList);
			}
			return brandList;
		}
	}
}
