using AutoMapper;
using System.Security.Claims;
using WebUI.Areas.Admin.Models;

namespace WebUI.Areas.Admin.Mappings
{
	public class ClaimsProfile : Profile
	{
		public ClaimsProfile()
		{
			CreateMap<Claim, RoleClaimsViewModel>().ReverseMap();
		}
	}
}
