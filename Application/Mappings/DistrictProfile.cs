using Application.Features.Communities.Commands;
using Application.Features.Communities.Queries.GetAllCached;
using Application.Features.Communities.Queries.GetById;
using Domain.Entities;
using AutoMapper;

namespace Application.Mappings
{
	internal class DistrictProfile : Profile
	{
		public DistrictProfile()
		{
			CreateMap<CreateDistrictCommand, District>().ReverseMap();
			CreateMap<GetDistrictByIdResponse, District>().ReverseMap();
			CreateMap<GetAllDistrictsCachedResponse, District>().ReverseMap();
		}
	}
}
