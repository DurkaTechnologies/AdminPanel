using AdminPanel.Application.Features.Communities.Commands;
using AdminPanel.Application.Features.Communities.Queries.GetAllCached;
using AdminPanel.Application.Features.Communities.Queries.GetById;
using AdminPanel.Domain.Entities;
using AutoMapper;

namespace AdminPanel.Application.Mappings
{
	internal class CommunityProfile : Profile
	{
		public CommunityProfile()
		{
			CreateMap<CreateCommunityCommand, Community>().ReverseMap();
			CreateMap<GetCommunityByIdResponse, Community>().ReverseMap();
			CreateMap<GetAllCommunitiesCachedResponse, Community>().ReverseMap();
		}
	}
}
