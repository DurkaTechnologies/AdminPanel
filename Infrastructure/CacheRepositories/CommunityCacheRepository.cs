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

namespace Infrastructure.CacheRepositories
{
	public class CommunityCacheRepository : ICommunityCacheRepository
	{
		private readonly IDistributedCache distributedCache;
		private readonly ICommunityRepository communityRepository;
		private readonly IMapper mapper;
    private readonly UserManager<ApplicationUser> userManager;
    
		public CommunityCacheRepository(IDistributedCache distributedCache, 
			ICommunityRepository communityRepository, IMapper mapper, 
      UserManager<ApplicationUser> userManager)
		{
			this.distributedCache = distributedCache;
			this.communityRepository = communityRepository;
			this.mapper = mapper;
      this.userManager = userManager;
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


        
        //HACK: Я не знаю чи при цьому не порушується архітектура, якщо з інфраструктури я підключусь до application
     public async Task<List<GetAllCommunitiesCachedResponse>> FillUserName(List<GetAllCommunitiesCachedResponse> list)
        {
            foreach (var el in list)
            {
                ApplicationUser user = await userManager.FindByIdAsync(el.ApplicationUserId);
                if (user == null)
                    el.ApplicationUserName = "Громада вільна";
                else
                    el.ApplicationUserName = user.LastName + " " + user.FirstName;
            }
            return list;
        }
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
