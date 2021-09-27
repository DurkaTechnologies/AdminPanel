using Application.Common.Models;
using Application.Features.Communities.Queries.GetAllCached;
using Application.Features.Logs.Queries;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Districts.Queries
{
	public class GetAllDistrictsQuery : IRequest<Result<List<GetAllDistrictsCachedResponse>>>
	{
		public GetAllDistrictsQuery()
		{

		}
	}

	public class GetAllDistrictsQueryHandler : IRequestHandler<GetAllDistrictsQuery, Result<List<GetAllDistrictsCachedResponse>>>
	{
		private readonly IDistrictRepository repository;
		private readonly IMapper mapper;

		public GetAllDistrictsQueryHandler(IDistrictRepository repository, IMapper mapper)
		{
			this.repository = repository;
			this.mapper = mapper;
		}

		public async Task<Result<List<GetAllDistrictsCachedResponse>>> Handle(GetAllDistrictsQuery request, CancellationToken cancellationToken)
		{
			var districts = mapper.Map<List<GetAllDistrictsCachedResponse>>(await repository.GetListAsync());
			return Result<List<GetAllDistrictsCachedResponse>>.Success(districts);
		}
	}
}
