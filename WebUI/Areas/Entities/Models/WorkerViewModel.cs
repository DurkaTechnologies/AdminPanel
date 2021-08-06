using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Areas.Entities.Models
{
	public class WorkerViewModel
	{
        [Display(Name = "ІД")]
        public int Id { get; set; }

        [Display(Name = "Ім'я")]
        public string Name { get; set; }

        [Display(Name = "Прізвище")]
        public string Surname { get; set; }

        [Display(Name = "По-Батькові")]
        public string Lastname { get; set; }

        [Display(Name = "Телефон")]
        public string Phone { get; set; }

        [Display(Name = "Фото")]
        public string Image { get; set; }

        public int CommunityId { get; set; }

        public virtual CommunityViewModel Community { get; set; }
    }
}
