using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.Entities.Models
{
	public class CommunityViewModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Не вказано ім'я громади")]
		public string Name { get; set; }

		[Required]
		public int? DistrictId { get; set; }

		public string ApplicationUserId { get; set; }

        public string ApplicationUserName { get; set; }

        public DistrictViewModel District { get; set; }

		public SelectList Districts { get; set; }

		public SelectList Users { get; set; }
	}
}
