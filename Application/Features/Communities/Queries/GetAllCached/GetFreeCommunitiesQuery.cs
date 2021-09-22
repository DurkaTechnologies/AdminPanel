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
	public class GetFreeCommunitiesQuery : IRequest<Result<List<GetCommunityByIdResponse>>>
	{
		public GetFreeCommunitiesQuery()
		{
		}
	}

	public class GetFreeCommunitiesQueryHandler : IRequestHandler<GetFreeCommunitiesQuery, Result<List<GetCommunityByIdResponse>>>
	{
		private readonly ICommunityRepository communityRepository;
		private readonly IMapper mapper;

		public GetFreeCommunitiesQueryHandler(ICommunityRepository communityRepository, IMapper mapper)
		{
			this.communityRepository = communityRepository;
			this.mapper = mapper;
		}

		public async Task<Result<List<GetCommunityByIdResponse>>> Handle(GetFreeCommunitiesQuery request, CancellationToken cancellationToken)
		{
			var communities = await communityRepository.GetFreeListAsync();
			var mappedCommunities = mapper.Map<List<GetCommunityByIdResponse>>(communities);
			return Result<List<GetCommunityByIdResponse>>.Success(mappedCommunities);
		}
	}
}
