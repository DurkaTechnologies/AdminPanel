using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebUI.Abstractions
{
	public interface IViewRenderService
	{
		Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model);

		string RenderRazorViewToString(Controller controller, string viewName, object model = null);

	}
}
