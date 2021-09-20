using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WebUI.Areas.Admin.Models;

namespace WebUI.Areas.Entities.Models
{
	public class CommunityViewModel
	{
		[Display(Name = "ІД")]
		public int Id { get; set; }

		[Display(Name = "Назва")]
		public string Name { get; set; }

		public int? DistrictId { get; set; }

		public string ApplicationUserId { get; set; }
        public string ApplicationUserName { get; set; }

        public DistrictViewModel District { get; set; }

		public SelectList Districts { get; set; }
		public SelectList Users { get; set; }
	}
}
