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

		Task<Community> GetByIdAsync(int communityId);

		Task<int> InsertAsync(Community community);

		Task UpdateAsync(Community community);

		Task DeleteAsync(Community community);
	}
}
