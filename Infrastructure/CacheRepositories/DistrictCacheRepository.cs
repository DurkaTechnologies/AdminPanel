using Application.Interfaces.CacheRepositories;
using Application.Interfaces.Repositories;
using Infrastructure.Extensions;
using Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.CacheKeys;

namespace Infrastructure.CacheRepositories
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
			string cacheKey = DistrictCacheKeys.GetKey(districtId);
			var district = await distributedCache.GetAsync<District>(cacheKey);

			if (district == null)
			{
				district = await districtRepository.GetByIdAsync(districtId);
				await distributedCache.SetAsync(cacheKey, district);
			}
			return district;
		}

		public async Task<List<District>> GetCachedListAsync()
		{
			string cacheKey = DistrictCacheKeys.ListKey;
			var districtList = await distributedCache.GetAsync<List<District>>(cacheKey);

			if (districtList == null)
			{
				districtList = await districtRepository.GetListAsync();
				await distributedCache.SetAsync(cacheKey, districtList);
			}
			return districtList;
		}
	}
}
