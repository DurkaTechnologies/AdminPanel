using Application.Features.Communities.Commands;
using Application.Features.Communities.Queries.GetById;
using Application.Features.Logs.Commands;
using Application.Interfaces.Shared;
using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Infrastructure.AuditModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Areas.Entities.Models;

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

	public abstract class BaseUserController<T> : BaseController<T>
	{
		protected IEnumerable<UpdateCommunityCommand> GenerateUpdate(IEnumerable<int> communities, string userId)
		{
			return communities.Select(id => new UpdateCommunityCommand()
			{
				Id = id,
				ApplicationUserId = userId
			});
		}

		protected async Task<bool> ExecuteUpdateCommands(IEnumerable<UpdateCommunityCommand> commands)
		{
			foreach (var command in commands)
			{
				var result = await _mediator.Send(new GetCommunityByIdNotCacheQuery() { Id = command.Id });
				if (!result.Succeeded)
				{
					_notify.Error($"Громаду з індексом {command.Id} не знайдено");
					return false;
				}

				var community = result.Data;
				var communityRes = await _mediator.Send(command);

				if (!communityRes.Succeeded)
				{
					_notify.Error($"Помилка при редагувані громади: {community.Name}");
					return false;
				}
				else
				{
					_notify.Information($"{community.Name} громада змінена");
					community.District = null;
					Log log = new Log()
					{
						UserId = _userService.UserId,
						Action = "Update",
						TableName = "Community",
						OldValues = _mapper.Map<CommunityViewModel>(community),
						NewValues = _mapper.Map<CommunityViewModel>(command),
						Key = community.Id.ToString()
					};

					if (!(await _mediator.Send(new AddLogCommand() { Log = log })).Succeeded)
						_notify.Warning($"Помилка запису інформації про зміну громади: {community.Name}");
				}
			}
			return true;
		}
	}
}
