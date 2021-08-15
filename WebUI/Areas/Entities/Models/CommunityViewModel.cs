using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.WebUI.Areas.Entities.Models
{
	public class CommunityViewModel
	{
		[Display(Name = "ІД")]
        public int Id { get; set; }

        [Display(Name = "Назва")]
        public string Name { get; set; }

        //public virtual List<WorkerViewModel> Workers { get; set; }
    }
}
