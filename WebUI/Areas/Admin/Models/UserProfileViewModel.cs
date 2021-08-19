using AdminPanel.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Areas.Admin.Models
{
    public class UserProfileViewModel
    {
        public ApplicationUser User { get; set; }

        public SelectList Communities { get; set; }
    }
}
