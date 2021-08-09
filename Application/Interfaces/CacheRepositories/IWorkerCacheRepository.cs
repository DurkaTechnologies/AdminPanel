using AdminPanel.Domain.Common.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminPanel.Application.Interfaces.CacheRepositories
{
    public interface IWorkerCacheRepository
    {
        Task<List<IWorker>> GetCachedListAsync();

        Task<IWorker> GetByIdAsync(int brandId);
    }
}