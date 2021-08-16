using AdminPanel.Application.Interfaces.Repositories;
using AdminPanel.Domain.Common.Interfaces;
using AdminPanel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.Infrastructure.Repositories
{
    public class WorkerRepository : IWorkerRepository
    {
        private readonly IRepositoryAsync<IWorker> _repository;
        private readonly IDistributedCache _distributedCache;

        public WorkerRepository(IDistributedCache distributedCache, IRepositoryAsync<IWorker> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }

        public IQueryable<IWorker> Workers => _repository.Entities;

        public async Task DeleteAsync(IWorker worker)
        {
            await _repository.DeleteAsync(worker);
            //await _distributedCache.RemoveAsync(CacheKeys.BrandCacheKeys.ListKey);
            //await _distributedCache.RemoveAsync(CacheKeys.BrandCacheKeys.GetKey(worker.Id));
        }

        //public async Task<IWorker> GetByIdAsync(int workerId)
        //{
        //    return await _repository.Entities.Where(p => p.Id == workerId).FirstOrDefaultAsync();
        //}

        public async Task<List<IWorker>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }

        public async Task<IWorker> InsertAsync(IWorker worker)
        {
            await _repository.AddAsync(worker);
            //await _distributedCache.RemoveAsync(CacheKeys.BrandCacheKeys.ListKey);
            return worker;
        }

        public async Task UpdateAsync(IWorker brand)
        {
            await _repository.UpdateAsync(brand);
            //await _distributedCache.RemoveAsync(CacheKeys.BrandCacheKeys.ListKey);
            //await _distributedCache.RemoveAsync(CacheKeys.BrandCacheKeys.GetKey(brand.Id));
        }
    }
}
