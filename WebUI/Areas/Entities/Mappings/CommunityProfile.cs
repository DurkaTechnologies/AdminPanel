using AdminPanel.Application.Features.Communities.Commands;
using AdminPanel.Application.Features.Communities.Queries.GetAllCached;
using AdminPanel.Application.Features.Communities.Queries.GetById;
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
