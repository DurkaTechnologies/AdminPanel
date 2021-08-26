using AdminPanel.Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.Infrastructure.Repositories
{
	public class CommunityRepository : ICommunityRepository
	{
		private readonly IRepositoryAsync<Community> _repository;
		private readonly IDistributedCache _distributedCache;

		public CommunityRepository(IDistributedCache distributedCache, IRepositoryAsync<Community> repository)
		{
			_distributedCache = distributedCache;
			_repository = repository;
		}

		public IQueryable<Community> Communities => _repository.Entities;

		public async Task DeleteAsync(Community community)
		{
			await _repository.DeleteAsync(community);
			await _distributedCache.RemoveAsync(CacheKeys.CommunityCacheKeys.ListKey);
			await _distributedCache.RemoveAsync(CacheKeys.CommunityCacheKeys.GetKey(community.Id));
		}

		public async Task<Community> GetByIdAsync(int communityId)
		{
			return await _repository.Entities.Where(p => p.Id == communityId).FirstOrDefaultAsync();
		}

		public async Task<List<Community>> GetListAsync()
		{
			return await _repository.Entities.ToListAsync();
		}

		public async Task<int> InsertAsync(Community community)
		{
			await _repository.AddAsync(community);
			await _distributedCache.RemoveAsync(CacheKeys.CommunityCacheKeys.ListKey);
			return community.Id;
		}

		public async Task UpdateAsync(Community community)
		{
			await _repository.UpdateAsync(community);
			await _distributedCache.RemoveAsync(CacheKeys.CommunityCacheKeys.ListKey);
			await _distributedCache.RemoveAsync(CacheKeys.CommunityCacheKeys.GetKey(community.Id));
		}
	}
}
