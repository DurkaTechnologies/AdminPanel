using Infrastructure.Identity.Models;
using AutoMapper;
using WebUI.Areas.Admin.Models;

namespace WebUI.Areas.Admin.Mappings
{
	public class UserProfile : Profile
	{
		public UserProfile()
		{
			CreateMap<ApplicationUser, UserViewModel>().ReverseMap();
		}
	}
}
