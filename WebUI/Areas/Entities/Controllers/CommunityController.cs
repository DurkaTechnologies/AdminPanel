using AdminPanel.Application.Features.Communities.Commands;
using AdminPanel.Application.Features.Communities.Queries.GetAllCached;
using AdminPanel.Application.Features.Communities.Queries.GetById;
using AdminPanel.Web.Abstractions;
using WebUI.Areas.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            var result = await _mediator.Send(new GetAllDistrictsCachedQuery());

            if (result.Succeeded)
            {
                var data = _mapper.Map<IEnumerable<DistrictViewModel>>(result.Data);
                var districts = new SelectList(data, nameof(DistrictViewModel.Id), nameof(DistrictViewModel.Name), null, null);

                if (id == 0)
                {
                    var communityViewModel = new CommunityViewModel() { Districts = districts };
                    return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", communityViewModel) });
                }
                else
                {
                    var response = await _mediator.Send(new GetCommunityByIdQuery() { Id = id });
                    if (response.Succeeded)
                    {
                        var communityViewModel = _mapper.Map<CommunityViewModel>(response.Data);
                        communityViewModel.Districts = districts;
                        return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", communityViewModel) });
                    }
                    _notify.Error("Громаду не знайдено");
                    return null;
                }
            }

            _notify.Error("Не заповнено форму");
            return null;
        }

        [HttpPost]
        public async Task<JsonResult> OnPostCreateOrEdit(int id, CommunityViewModel community)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    var createCommunityCommand = _mapper.Map<CreateCommunityCommand>(community);
                    var result = await _mediator.Send(createCommunityCommand);

                    if (result.Succeeded)
                    {
                        id = result.Data;
                        _notify.Success($"Громада {community.Name} створена");
                    }
                    else
                        _notify.Error("Помилка створення");
                }
                else
                {
                    var updateCommunityCommand = _mapper.Map<UpdateCommunityCommand>(community);
                    var result = await _mediator.Send(updateCommunityCommand);
                    if (result.Succeeded) 
                        _notify.Success($"Громада {community.Name} змінена");
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
                    _notify.Error("Помилка створення");
                    return null;
                }
            }
            else
            {
                var html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", community);
                return new JsonResult(new { isValid = false, html = html });
            }
        }

        [HttpPost]
        public async Task<JsonResult> OnPostDelete(int id)
        {
            var byIdResponse = await _mediator.Send(new GetCommunityByIdQuery() { Id = id });
            var community = _mapper.Map<CommunityViewModel>(byIdResponse.Data);

            var deleteCommand = await _mediator.Send(new DeleteCommunityCommand { Id = id });
            
            if (deleteCommand.Succeeded)
            {
                _notify.Success($"Громада {community.Name} видалена");
                var response = await _mediator.Send(new GetAllCommunitiesCachedQuery());
                
                if (response.Succeeded)
                {
                    var viewModel = _mapper.Map<List<CommunityViewModel>>(response.Data);
                    var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
                    return new JsonResult(new { isValid = true, html = html });
                }
                else
                {
                    _notify.Error("Помилка видалення");
                    return null;
                }
            }
            else
            {
                _notify.Error("Помилка видалення");
                return null;
            }
        }
    }
}
