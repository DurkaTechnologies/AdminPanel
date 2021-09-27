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
	public class CommunityRepository : ICommunityRepository
	{
		private readonly IRepositoryAsync<Community, IdentityContext> repository;
		private readonly IDistributedCache distributedCache;

		public CommunityRepository(IDistributedCache distributedCache, 
			IRepositoryAsync<Community, IdentityContext> repository)
		{
			this.distributedCache = distributedCache;
			this.repository = repository;
		}

		public IQueryable<Community> Communities => repository.Entities;

		public async Task DeleteAsync(Community community)
		{
			await repository.DeleteAsync(community);
			await distributedCache.RemoveAsync(CacheKeys.CommunityCacheKeys.ListKey);
			await distributedCache.RemoveAsync(CacheKeys.CommunityCacheKeys.GetKey(community.Id));
		}

		public async Task<Community> GetByIdAsync(int communityId)
		{
			return await repository.Entities.Where(p => p.Id == communityId).FirstOrDefaultAsync();
		}

		public async Task<List<Community>> GetListAsync()
		{
			return await repository.Entities.ToListAsync();
		}

		public IQueryable<Community> GetIncludentListAsync()
		{
			return repository.Entities.Include(c => c.District);
		}

		public async Task<List<Community>> GetFreeListAsync()
		{
			return await repository.Entities.Include(c => c.District).Where(c => c.ApplicationUserId == null).ToListAsync();
		}
		public async Task<List<Community>> GetListByUserIdAsync(string userId)
		{
			return await repository.Entities.Include(c => c.District).Where(c => c.ApplicationUserId == userId).ToListAsync();
		}

		public async Task<int> InsertAsync(Community community)
		{
			await repository.AddAsync(community);
			await distributedCache.RemoveAsync(CacheKeys.CommunityCacheKeys.ListKey);
			return community.Id;
		}

		public async Task UpdateAsync(Community community)
		{
			await repository.UpdateAsync(community);
			await distributedCache.RemoveAsync(CacheKeys.CommunityCacheKeys.ListKey);
			await distributedCache.RemoveAsync(CacheKeys.CommunityCacheKeys.GetKey(community.Id));
		}
	}
}
