using Application.Common.Models;
using Application.Interfaces.CacheRepositories;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Communities.Queries.GetAllCached
{
	public class GetAllCommunitiesCachedQuery : IRequest<Result<List<GetAllCommunitiesCachedResponse>>>
	{
		public GetAllCommunitiesCachedQuery()
		{
		}
	}

	public class GetAllCommunitiesCachedQueryHandler : IRequestHandler<GetAllCommunitiesCachedQuery, Result<List<GetAllCommunitiesCachedResponse>>>
	{
		private readonly ICommunityCacheRepository communityCache;
		private readonly IMapper mapper;

		public GetAllCommunitiesCachedQueryHandler(ICommunityCacheRepository communityCache, IMapper mapper)
		{
			this.communityCache = communityCache;
			this.mapper = mapper;
		}

		public async Task<Result<List<GetAllCommunitiesCachedResponse>>> Handle(GetAllCommunitiesCachedQuery request, CancellationToken cancellationToken)
		{
			var communities = await communityCache.GetCachedListAsync();
			var mappedCommunities = mapper.Map<List<GetAllCommunitiesCachedResponse>>(communities);
			mappedCommunities = await communityCache.FillUserName(mappedCommunities);
			return Result<List<GetAllCommunitiesCachedResponse>>.Success(mappedCommunities);
		}
	}
}
