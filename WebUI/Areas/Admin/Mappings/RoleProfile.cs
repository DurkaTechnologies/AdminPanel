using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WebUI.Areas.Admin.Models;

namespace WebUI.Areas.Admin.Mappings
{
	public class RoleProfile : Profile
	{
		public RoleProfile()
		{
			CreateMap<IdentityRole, RoleViewModel>().ReverseMap();
		}
	}
}
