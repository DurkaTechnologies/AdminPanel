using AdminPanel.Application.Common.Models;
using AdminPanel.Application.Interfaces.CacheRepositories;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AdminPanel.Application.Features.Communities.Queries.GetById
{
	public class GetCommunityByIdQuery : IRequest<Result<GetCommunityByIdResponse>>
	{
		public int Id { get; set; }

		public class GeCommunityByIdQueryHandler : IRequestHandler<GetCommunityByIdQuery, Result<GetCommunityByIdResponse>>
		{
			private readonly ICommunityCacheRepository communityCache;
			private readonly IMapper mapper;

			public GeCommunityByIdQueryHandler(ICommunityCacheRepository communityCache, IMapper mapper)
			{
				this.communityCache = communityCache;
				this.mapper = mapper;
			}

			public async Task<Result<GetCommunityByIdResponse>> Handle(GetCommunityByIdQuery query, CancellationToken cancellationToken)
			{
				var community = await communityCache.GetByIdAsync(query.Id);
				var mappedCommunity = mapper.Map<GetCommunityByIdResponse>(community);
				return Result<GetCommunityByIdResponse>.Success(mappedCommunity);
			}
		}
	}
}
