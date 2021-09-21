using Application.Interfaces.CacheRepositories;
using Application.Interfaces.Repositories;
using Infrastructure.Extensions;
using Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.CacheKeys;
using Microsoft.AspNetCore.Identity;
using Application.Features.Communities.Queries.GetAllCached;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Identity.Models;

namespace Infrastructure.CacheRepositories
{
	public class CommunityCacheRepository : ICommunityCacheRepository
	{
		private readonly IDistributedCache distributedCache;
		private readonly ICommunityRepository communityRepository;
		private readonly IMapper mapper;

		public CommunityCacheRepository(IDistributedCache distributedCache,
			ICommunityRepository communityRepository, IMapper mapper)
		{
			this.distributedCache = distributedCache;
			this.communityRepository = communityRepository;
			this.mapper = mapper;
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

		public async Task<List<GetAllCommunitiesCachedResponse>> GetCachedListAsync()
		{
			string cacheKey = CommunityCacheKeys.ListKey;
			var communityList = await distributedCache.GetAsync<List<GetAllCommunitiesCachedResponse>>(cacheKey);

			if (communityList == null)
			{
				communityList = await communityRepository.GetIncludentListAsync()
					.ProjectTo<GetAllCommunitiesCachedResponse>(mapper.ConfigurationProvider)
					.ToListAsync();

				await distributedCache.SetAsync(cacheKey, communityList);
			}
			return communityList;
		}
	}
}
