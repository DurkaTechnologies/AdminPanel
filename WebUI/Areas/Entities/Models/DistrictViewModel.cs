using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.Entities.Models
{
	public class DistrictViewModel
	{
		[Display(Name = "ІД")]
		public int Id { get; set; }

		[Display(Name = "Назва")]
		public string Name { get; set; }
	}
}
