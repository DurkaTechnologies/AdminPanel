using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading.Tasks;

namespace WebUI.Abstractions
{
	public interface IViewRenderService
	{
		Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model, ITempDataDictionary data = null);

		string RenderRazorViewToString(Controller controller, string viewName, object model = null);

	}
}
