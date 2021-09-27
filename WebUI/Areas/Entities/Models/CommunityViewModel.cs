using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.Entities.Models
{
	public class CommunityViewModel
	{
		[Display(Name = "ІД")]
		public int Id { get; set; }

		[Display(Name = "Назва")]
		public string Name { get; set; }

		public int? DistrictId { get; set; }

		public DistrictViewModel District { get; set; }

		public SelectList Districts { get; set; }
	}
}
