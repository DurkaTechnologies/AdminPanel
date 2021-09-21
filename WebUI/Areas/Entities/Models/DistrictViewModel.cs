using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.Entities.Models
{
	public class DistrictViewModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Не вказано ім'я району")]
		[MinLength(2, ErrorMessage = "Не вказано ім'я району")]
		public string Name { get; set; }
	}
}
