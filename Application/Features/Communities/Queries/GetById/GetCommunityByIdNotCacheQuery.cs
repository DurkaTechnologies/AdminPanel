using Application.Common.Models;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Communities.Queries.GetById
{
	public class GetCommunityByIdNotCacheQuery : IRequest<Result<GetCommunityByIdResponse>>
	{
		public int Id { get; set; }

		public class GeCommunityByIdQueryHandler : IRequestHandler<GetCommunityByIdNotCacheQuery, Result<GetCommunityByIdResponse>>
		{
			private readonly ICommunityRepository repository;
			private readonly IMapper mapper;

			public GeCommunityByIdQueryHandler(ICommunityRepository repository, IMapper mapper)
			{
				this.repository = repository;
				this.mapper = mapper;
			}

			public async Task<Result<GetCommunityByIdResponse>> Handle(GetCommunityByIdNotCacheQuery query, CancellationToken cancellationToken)
			{
				var community = await repository.GetByIdAsync(query.Id);
				var mappedCommunity = mapper.Map<GetCommunityByIdResponse>(community);
				return Result<GetCommunityByIdResponse>.Success(mappedCommunity);
			}
		}
	}
}
