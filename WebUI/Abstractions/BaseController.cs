using Application.Interfaces.Shared;
using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WebUI.Abstractions
{
	public abstract class BaseController<T> : Controller
	{
		/*private*/

		private IMediator _mediatorInstance;
		private ILogger<T> _loggerInstance;
		private IViewRenderService _viewRenderInstance;
		private IMapper _mapperInstance;
		private INotyfService _notifyInstance;
		private IAuthenticatedUserService _userServiceInstanse;

		/*protected*/

		protected IAuthenticatedUserService _userService => _userServiceInstanse ??= HttpContext.RequestServices.GetService<IAuthenticatedUserService>();
		protected INotyfService _notify => _notifyInstance ??= HttpContext.RequestServices.GetService<INotyfService>();
		protected IMediator _mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>();
		protected ILogger<T> _logger => _loggerInstance ??= HttpContext.RequestServices.GetService<ILogger<T>>();
		protected IViewRenderService _viewRenderer => _viewRenderInstance ??= HttpContext.RequestServices.GetService<IViewRenderService>();
		protected IMapper _mapper => _mapperInstance ??= HttpContext.RequestServices.GetService<IMapper>();
	}
}
