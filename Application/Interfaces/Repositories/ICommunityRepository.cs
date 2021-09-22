using Application.Features.Communities.Queries.GetAllCached;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
	public interface ICommunityRepository
	{
		IQueryable<Community> Communities { get; }

		Task<List<Community>> GetListAsync();

		IQueryable<Community> GetIncludentListAsync();

		Task<List<Community>> GetFreeListAsync();

		Task<List<Community>> GetListByUserIdAsync(string userId);

		Task<Community> GetByIdAsync(int communityId);

		Task<int> InsertAsync(Community community);

		Task UpdateAsync(Community community);

		Task DeleteAsync(Community community);
	}
}
