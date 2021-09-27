using AdminPanel.Application.Common.Models;
using AdminPanel.Application.Interfaces.CacheRepositories;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AdminPanel.Application.Features.Communities.Queries.GetById
{
	public class GetDistrictByIdQuery : IRequest<Result<GetDistrictByIdResponse>>
	{
		public int Id { get; set; }

		public class GetDistrictByIdQueryHandler : IRequestHandler<GetDistrictByIdQuery, Result<GetDistrictByIdResponse>>
		{
			private readonly IDistrictCacheRepository districtCache;
			private readonly IMapper mapper;

			public GetDistrictByIdQueryHandler(IDistrictCacheRepository districtCache, IMapper mapper)
			{
				this.districtCache = districtCache;
				this.mapper = mapper;
			}

			public async Task<Result<GetDistrictByIdResponse>> Handle(GetDistrictByIdQuery query, CancellationToken cancellationToken)
			{
				var district = await districtCache.GetByIdAsync(query.Id);
				var mappedProduct = mapper.Map<GetDistrictByIdResponse>(district);
				return Result<GetDistrictByIdResponse>.Success(mappedProduct);
			}
		}
	}
}
