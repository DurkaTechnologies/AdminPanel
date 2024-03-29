﻿using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
	public interface IDistrictRepository
	{
		IQueryable<District> Communities { get; }

		Task<List<District>> GetListAsync();

		Task<District> GetByIdAsync(int communityId);

		Task<District> GetIncludeByIdAsync(int communityId);

		Task<int> InsertAsync(District community);

		Task UpdateAsync(District community);

		Task DeleteAsync(District community);
	}
}
