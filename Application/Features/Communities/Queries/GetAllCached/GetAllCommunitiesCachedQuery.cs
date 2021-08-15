using AdminPanel.Application.Common.Models;
using AdminPanel.Application.Interfaces.CacheRepositories;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AdminPanel.Application.Features.Communities.Queries.GetAllCached
{
	public class GetAllCommunitiesCachedQuery : IRequest<Result<List<GetAllCommunitiesCachedResponse>>>
	{
		public GetAllCommunitiesCachedQuery()
		{
		}
	}

	public class GetAllBrandsCachedQueryHandler : IRequestHandler<GetAllCommunitiesCachedQuery, Result<List<GetAllCommunitiesCachedResponse>>>
	{
		private readonly ICommunityCacheRepository communityCache;
		private readonly IMapper mapper;

		public GetAllBrandsCachedQueryHandler(ICommunityCacheRepository communityCache, IMapper mapper)
		{
			this.communityCache = communityCache;
			this.mapper = mapper;
		}

		public async Task<Result<List<GetAllCommunitiesCachedResponse>>> Handle(GetAllCommunitiesCachedQuery request, CancellationToken cancellationToken)
		{
			var communities = await communityCache.GetCachedListAsync();
			var mappedCommunities = mapper.Map<List<GetAllCommunitiesCachedResponse>>(communities);
			return Result<List<GetAllCommunitiesCachedResponse>>.Success(mappedCommunities);
		}
	}
}
