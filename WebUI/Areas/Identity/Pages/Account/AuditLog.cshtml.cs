using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.DTOs;
using Application.Features.ActivityLog.Queries;
using Application.Features.Logs.Queries;
using Application.Interfaces.Shared;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using WebUI.Abstractions;

namespace WebUI.Areas.Identity.Pages.Account
{
	public class AuditLogModel : BasePageModel<AuditLogModel>
	{
		private readonly UserManager<ApplicationUser> _userManager;
		public List<AuditLogResponse> AuditLogResponses;
		public bool Simple { get; set; }
		public string UserId { get; set; }

		private IViewRenderService _viewRenderer;

		public AuditLogModel(IAuthenticatedUserService userService, IViewRenderService viewRenderer, UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
			_viewRenderer = viewRenderer;
			Simple = false;
		}

		public async Task OnGet(string id)
		{
			UserId = id;
			Result<List<AuditLogResponse>> response = null;

            switch (id)
            {
				case null:
					if (User.IsInRole("SuperAdmin"))
					{
						response = await _mediator.Send(new GetAllAuditLogsQuery());
						Simple = false;
					}
					break;
				case "current":
					response = await _mediator.Send(new GetAuditLogsQuery() { userId = _userService.UserId });
					Simple = true;
					break;
				default:
					if (User.IsInRole("SuperAdmin"))
					{
						response = await _mediator.Send(new GetAuditLogsQuery() { userId = id });
						Simple = true;
					}
					break;
            }

			if (response != null)
				AuditLogResponses = response.Data;
		}
	}
}
