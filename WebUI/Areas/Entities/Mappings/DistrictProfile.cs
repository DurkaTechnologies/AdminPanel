using Application.Features.Communities.Commands;
using Application.Features.Communities.Queries.GetAllCached;
using Application.Features.Communities.Queries.GetById;
using WebUI.Areas.Entities.Models;
using AutoMapper;

namespace WebUI.Areas.Entities.Mappings
{
    internal class DistrictProfile : Profile
    {
        public DistrictProfile()
        {
            CreateMap<GetAllDistrictsCachedResponse, DistrictViewModel>().ReverseMap();
            CreateMap<GetDistrictByIdResponse, DistrictViewModel>().ReverseMap();
            CreateMap<CreateDistrictCommand, DistrictViewModel>().ReverseMap();
            CreateMap<UpdateDistrictCommand, DistrictViewModel>().ReverseMap();
        }
    }
}
