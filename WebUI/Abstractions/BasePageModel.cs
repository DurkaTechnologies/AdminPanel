using AdminPanel.Application.Interfaces.Shared;
using AspNetCoreHero.ToastNotification.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AdminPanel.Web.Abstractions
{
    public class BasePageModel<T> : PageModel where T : class
    {
        /*private*/

        private IMediator _mediatorInstance;
        private ILogger<T> _loggerInstance;
        private INotyfService notyf;
        private IAuthenticatedUserService _userServiceInstanse;

        /*protected*/

        protected IAuthenticatedUserService _userService => _userServiceInstanse ??= HttpContext.RequestServices.GetService<IAuthenticatedUserService>();
        protected IMediator _mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>();
        protected ILogger<T> _logger => _loggerInstance ??= HttpContext.RequestServices.GetService<ILogger<T>>();
        protected INotyfService _notyf => notyf ??= HttpContext.RequestServices.GetService<INotyfService>();
    }
}
