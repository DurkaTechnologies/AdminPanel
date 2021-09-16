using Application.Common.Models;
using Application.Features.Communities.Queries.GetById;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Communities.Queries.GetAllCached
{
	public class GeUserCommunitiesQuery : IRequest<Result<List<GetCommunityByIdResponse>>>
	{
		public string UserId { get; set; }

		public class GeUserCommunitiesQueryHandler : IRequestHandler<GeUserCommunitiesQuery, Result<List<GetCommunityByIdResponse>>>
		{
			private readonly ICommunityRepository communityRepository;
			private readonly IMapper mapper;

			public GeUserCommunitiesQueryHandler(ICommunityRepository communityRepository, IMapper mapper)
			{
				this.communityRepository = communityRepository;
				this.mapper = mapper;
			}

			public async Task<Result<List<GetCommunityByIdResponse>>> Handle(GeUserCommunitiesQuery request, CancellationToken cancellationToken)
			{
				var communities = await communityRepository.GetListByUserIdAsync(request.UserId);
				var mappedCommunities = mapper.Map<List<GetCommunityByIdResponse>>(communities);
				return Result<List<GetCommunityByIdResponse>>.Success(mappedCommunities);
			}
		}
	}
}
