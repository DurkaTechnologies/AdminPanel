using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminPanel.Application.DTOs;
using AdminPanel.Application.Features.ActivityLog.Queries;
using AdminPanel.Application.Interfaces.Shared;
using AdminPanel.Web.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Areas.Identity.Pages.Account
{
    public class AuditLogModel : BasePageModel<AuditLogModel>
    {
        
        public List<AuditLogResponse> AuditLogResponses;
        private IViewRenderService _viewRenderer;

        public AuditLogModel(IAuthenticatedUserService userService, IViewRenderService viewRenderer)
        {
            //_mediator = mediator;
            _viewRenderer = viewRenderer;
        }

        public async Task OnGet(string id)
        {
            if (id == null)
                id = _userService.UserId;

            var response = await _mediator.Send(new GetAuditLogsQuery() { userId = id });
            AuditLogResponses = response.Data;
        }
    }
}