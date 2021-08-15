﻿using AdminPanel.Application.Interfaces.CacheRepositories;
using AdminPanel.Application.Interfaces.Repositories;
using AdminPanel.Infrastructure.Extensions;
using AdminPanel.Domain.Entities;
using AdminPanel.Infrastructure.CacheKeys;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminPanel.Infrastructure.CacheRepositories
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
			var brand = await distributedCache.GetAsync<Community>(cacheKey);
			if (brand == null)
			{
				brand = await communityRepository.GetByIdAsync(communityId);
				await distributedCache.SetAsync(cacheKey, brand);
			}
			return brand;
		}

		public async Task<List<Community>> GetCachedListAsync()
		{
			string cacheKey = CommunityCacheKeys.ListKey;
			var brandList = await distributedCache.GetAsync<List<Community>>(cacheKey);
			if (brandList == null)
			{
				brandList = await communityRepository.GetListAsync();
				await distributedCache.SetAsync(cacheKey, brandList);
			}
			return brandList;
		}
	}
}
