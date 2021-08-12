using AdminPanel.Application.Features.Communities.Commands;
using AdminPanel.Application.Features.Communities.Queries.GetAllCached;
using AdminPanel.Application.Features.Communities.Queries.GetById;
using AdminPanel.Web.Abstractions;
using AdminPanel.WebUI.Areas.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Areas.Entities.Controllers
{
	[Area("Entities")]
	public class CommunityController : BaseController<CommunityController>
	{
        // GET: CommunityController
        public IActionResult Index()
        {
            var model = new CommunityViewModel();
            return View(model);
        }

        public async Task<IActionResult> LoadAll()
        {
            var response = await _mediator.Send(new GetAllCommunitiesCachedQuery());
            if (response.Succeeded)
            {
                var viewModel = _mapper.Map<List<CommunityViewModel>>(response.Data);
                return PartialView("_ViewAll", viewModel);
            }
            return null;
        }

        public async Task<JsonResult> OnGetCreateOrEdit(int id = 0)
        {
            var communitiesResponse = await _mediator.Send(new GetAllCommunitiesCachedQuery());

            if (id == 0)
            {
                var communityViewModel = new CommunityViewModel();
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", communityViewModel) });
            }
            else
            {
                var response = await _mediator.Send(new GetCommunityByIdQuery() { Id = id });
                if (response.Succeeded)
                {
                    var communityViewModel = _mapper.Map<CommunityViewModel>(response.Data);
                    return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", communityViewModel) });
                }
                return null;
            }
        }

        [HttpPost]
        public async Task<JsonResult> OnPostCreateOrEdit(int id, CommunityViewModel brand)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    var createCommunityCommand = _mapper.Map<CreateCommunityCommand>(brand);
                    var result = await _mediator.Send(createCommunityCommand);
                    if (result.Succeeded)
                    {
                        id = result.Data;
                    }
                }
                else
                {
                    var updateCommunityCommand = _mapper.Map<UpdateCommunityCommand>(brand);
                    var result = await _mediator.Send(updateCommunityCommand);
                    //if (result.Succeeded) _notify.Information($"Brand with ID {result.Data} Updated.");
                }
                var response = await _mediator.Send(new GetAllCommunitiesCachedQuery());
                if (response.Succeeded)
                {
                    var viewModel = _mapper.Map<List<CommunityViewModel>>(response.Data);
                    var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
                    return new JsonResult(new { isValid = true, html = html });
                }
                else
                {
                    //_notify.Error(response.Message);
                    return null;
                }
            }
            else
            {
                var html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", brand);
                return new JsonResult(new { isValid = false, html = html });
            }
        }

        [HttpPost]
        public async Task<JsonResult> OnPostDelete(int id)
        {
            var deleteCommand = await _mediator.Send(new DeleteCommunityCommand { Id = id });
            if (deleteCommand.Succeeded)
            {
                //_notify.Information($"Brand with Id {id} Deleted.");
                var response = await _mediator.Send(new GetAllCommunitiesCachedQuery());
                if (response.Succeeded)
                {
                    var viewModel = _mapper.Map<List<CommunityViewModel>>(response.Data);
                    var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
                    return new JsonResult(new { isValid = true, html = html });
                }
                else
                {
                    //_notify.Error(response.Message);
                    return null;
                }
            }
            else
            {
                //_notify.Error(deleteCommand.Message);
                return null;
            }
        }
    }
}
