using AdminPanel.Application.Interfaces.Repositories;
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
        private readonly IRepositoryAsync<Worker> _repository;
        private readonly IDistributedCache _distributedCache;

        public WorkerRepository(IDistributedCache distributedCache, IRepositoryAsync<Worker> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }

        public IQueryable<Worker> Workers => _repository.Entities;

        public async Task DeleteAsync(Worker worker)
        {
            await _repository.DeleteAsync(worker);
            //await _distributedCache.RemoveAsync(CacheKeys.BrandCacheKeys.ListKey);
            //await _distributedCache.RemoveAsync(CacheKeys.BrandCacheKeys.GetKey(worker.Id));
        }

        public async Task<Worker> GetByIdAsync(int workerId)
        {
            return await _repository.Entities.Where(p => p.Id == workerId).FirstOrDefaultAsync();
        }

        public async Task<List<Worker>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }

        public async Task<int> InsertAsync(Worker worker)
        {
            await _repository.AddAsync(worker);
            //await _distributedCache.RemoveAsync(CacheKeys.BrandCacheKeys.ListKey);
            return worker.Id;
        }

        public async Task UpdateAsync(Worker brand)
        {
            await _repository.UpdateAsync(brand);
            //await _distributedCache.RemoveAsync(CacheKeys.BrandCacheKeys.ListKey);
            //await _distributedCache.RemoveAsync(CacheKeys.BrandCacheKeys.GetKey(brand.Id));
        }
    }
}