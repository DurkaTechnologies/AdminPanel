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
	public class CommunityCacheRepository : ICommunityCacheRepository
	{
		private readonly IDistributedCache distributedCache;
		private readonly ICommunityRepository communityRepository;

		public CommunityCacheRepository(IDistributedCache distributedCache, ICommunityRepository communityRepository)
		{
			this.distributedCache = distributedCache;
			this.communityRepository = communityRepository;
		}

		public async Task<Community> GetByIdAsync(int communityId)
		{
			string cacheKey = CommunityCacheKeys.GetKey(communityId);
			var community = await distributedCache.GetAsync<Community>(cacheKey);

			if (community == null)
			{
				community = await communityRepository.GetByIdAsync(communityId);
				await distributedCache.SetAsync(cacheKey, community);
			}
			return community;
		}

		public async Task<List<Community>> GetCachedListAsync()
		{
			string cacheKey = CommunityCacheKeys.ListKey;
			var communityList = await distributedCache.GetAsync<List<Community>>(cacheKey);

			if (communityList == null)
			{
				communityList = await communityRepository.GetListAsync();
				await distributedCache.SetAsync(cacheKey, communityList);
			}
			return communityList;
		}
	}
}
