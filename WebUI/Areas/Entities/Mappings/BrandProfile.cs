using AdminPanel.Application.Features.Communities.Commands;
using AdminPanel.Application.Features.Communities.Queries.GetAllCached;
using AdminPanel.Application.Features.Communities.Queries.GetById;
using AdminPanel.WebUI.Areas.Entities.Models;
using AutoMapper;

namespace AdminPanel.WebUI.Areas.Catalog.Mappings
{
    internal class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<GetAllCommunitiesCachedResponse, CommunityViewModel>().ReverseMap();
            CreateMap<GetCommunityByIdResponse, CommunityViewModel>().ReverseMap();
            CreateMap<CreateCommunityCommand, CommunityViewModel>().ReverseMap();
            CreateMap<UpdateCommunityCommand, CommunityViewModel>().ReverseMap();
        }
    }
}