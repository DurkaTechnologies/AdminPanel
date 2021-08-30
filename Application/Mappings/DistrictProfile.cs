using AdminPanel.Application.Features.Communities.Commands;
using AdminPanel.Application.Features.Communities.Queries.GetAllCached;
using AdminPanel.Application.Features.Communities.Queries.GetById;
using Domain.Entities;
using AutoMapper;

namespace AdminPanel.Application.Mappings
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
