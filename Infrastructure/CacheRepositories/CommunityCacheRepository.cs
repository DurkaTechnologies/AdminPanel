using Application.Interfaces.CacheRepositories;
using Application.Interfaces.Repositories;
using Infrastructure.Extensions;
using Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.CacheKeys;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Identity.Models;
using Application.Features.Communities.Queries.GetAllCached;

namespace Infrastructure.CacheRepositories
{
    public class CommunityCacheRepository : ICommunityCacheRepository
    {
        private readonly IDistributedCache distributedCache;
        private readonly ICommunityRepository communityRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public CommunityCacheRepository(IDistributedCache distributedCache, ICommunityRepository communityRepository, UserManager<ApplicationUser> userManager)
        {
            this.distributedCache = distributedCache;
            this.communityRepository = communityRepository;
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
}
