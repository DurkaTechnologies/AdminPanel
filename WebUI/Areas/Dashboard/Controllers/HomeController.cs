using AdminPanel.Web.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Web.Areas.Dashboard.Controllers
{
	[Area("Dashboard")]
	public class HomeController : BaseController<HomeController>
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
