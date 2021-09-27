using Application.Common.Models;
using Application.Interfaces.CacheRepositories;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Communities.Queries.GetById
{
	public class GetDistrictByIdQuery : IRequest<Result<GetDistrictByIdResponse>>
	{
		public int Id { get; set; }

		public class GetDistrictByIdQueryHandler : IRequestHandler<GetDistrictByIdQuery, Result<GetDistrictByIdResponse>>
		{
			private readonly IDistrictRepository districtCache;
			private readonly IMapper mapper;

			public GetDistrictByIdQueryHandler(IDistrictRepository districtCache, IMapper mapper)
			{
				this.districtCache = districtCache;
				this.mapper = mapper;
			}

			public async Task<Result<GetDistrictByIdResponse>> Handle(GetDistrictByIdQuery query, CancellationToken cancellationToken)
			{
				var district = await districtCache.GetByIdAsync(query.Id);
				var mappedDistrict = mapper.Map<GetDistrictByIdResponse>(district);
				return Result<GetDistrictByIdResponse>.Success(mappedDistrict);
			}
		}
	}
}
