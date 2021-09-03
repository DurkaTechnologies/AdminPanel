using Application.Features.Communities.Commands;
using Application.Features.Communities.Queries.GetAllCached;
using Application.Features.Communities.Queries.GetById;
using WebUI.Areas.Entities.Models;
using AutoMapper;

namespace WebUI.Areas.Entities.Mappings
{
    internal class CommunityProfile : Profile
    {
        public CommunityProfile()
        {
            CreateMap<GetAllCommunitiesCachedResponse, CommunityViewModel>().ReverseMap();
            CreateMap<GetCommunityByIdResponse, CommunityViewModel>().ReverseMap();
            CreateMap<CreateCommunityCommand, CommunityViewModel>().ReverseMap();
            CreateMap<UpdateCommunityCommand, CommunityViewModel>().ReverseMap();
        }
    }
}
