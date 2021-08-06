using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Areas.Entities.Models
{
	public class CommunityViewModel
	{
		[Display(Name = "ІД")]
        public int Id { get; set; }

        [Display(Name = "Назва")]
        public string Name { get; set; }

        [Display(Name = "Район")]
        public string Area { get; set; }

        [Display(Name = "Тип")]
        public string Type { get; set; }

        [Display(Name = "Дата")]
        public DateTime Date { get; set; }

        [Display(Name = "Площа")]
        public float Square { get; set; }

        [Display(Name = "Кількість Населення")]
        public int PeopleCount { get; set; }

        [Display(Name = "Кількість Рад")]
        public int Count { get; set; }

        public virtual List<WorkerViewModel> Workers { get; set; }
    }
}
