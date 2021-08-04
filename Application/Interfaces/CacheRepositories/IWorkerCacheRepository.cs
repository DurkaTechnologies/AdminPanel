using AdminPanel.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminPanel.Application.Interfaces.CacheRepositories
{
    public interface IWorkerCacheRepository
    {
        Task<List<Worker>> GetCachedListAsync();

        Task<Worker> GetByIdAsync(int brandId);
    }
}