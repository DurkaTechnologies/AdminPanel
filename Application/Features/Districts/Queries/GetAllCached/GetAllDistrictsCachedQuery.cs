using AdminPanel.Application.Common.Models;
using AdminPanel.Application.Interfaces.CacheRepositories;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AdminPanel.Application.Features.Communities.Queries.GetAllCached
{
	public class GetAllDistrictsCachedQuery : IRequest<Result<List<GetAllDistrictsCachedResponse>>>
	{
		public GetAllDistrictsCachedQuery()
		{
		}
	}

	public class GetAllDistrictsCachedQueryHandler : IRequestHandler<GetAllDistrictsCachedQuery, Result<List<GetAllDistrictsCachedResponse>>>
	{
		private readonly IDistrictCacheRepository districtCache;
		private readonly IMapper mapper;

		public GetAllDistrictsCachedQueryHandler(IDistrictCacheRepository districtCache, IMapper mapper)
		{
			this.districtCache = districtCache;
			this.mapper = mapper;
		}

		public async Task<Result<List<GetAllDistrictsCachedResponse>>> Handle(GetAllDistrictsCachedQuery request, CancellationToken cancellationToken)
		{
			var districts = await districtCache.GetCachedListAsync();
			var mappedDistricts = mapper.Map<List<GetAllDistrictsCachedResponse>>(districts);
			return Result<List<GetAllDistrictsCachedResponse>>.Success(mappedDistricts);
		}
	}
}
