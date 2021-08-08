using AdminPanel.Application.Common.Models;
using AdminPanel.Application.Interfaces.CacheRepositories;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AdminPanel.Application.Features.Communities.Queries.GetById
{
	public class GetBrandByIdQuery : IRequest<Result<GetCommunityByIdResponse>>
	{
		public int Id { get; set; }

		public class GetProductByIdQueryHandler : IRequestHandler<GetBrandByIdQuery, Result<GetCommunityByIdResponse>>
		{
			private readonly ICommunityCacheRepository communityCache;
			private readonly IMapper mapper;

			public GetProductByIdQueryHandler(ICommunityCacheRepository communityCache, IMapper mapper)
			{
				this.communityCache = communityCache;
				this.mapper = mapper;
			}

			public async Task<Result<GetCommunityByIdResponse>> Handle(GetBrandByIdQuery query, CancellationToken cancellationToken)
			{
				var product = await communityCache.GetByIdAsync(query.Id);
				var mappedProduct = mapper.Map<GetCommunityByIdResponse>(product);
				return Result<GetCommunityByIdResponse>.Success(mappedProduct);
			}
		}
	}
}
