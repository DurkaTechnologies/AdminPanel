using WebUI.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Dashboard.Controllers
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
