using AdminPanel.Web.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Areas.Identity.Management.Controllers
{
	[Area("Management")]
	public class UsersController : BaseController<UsersController>
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
