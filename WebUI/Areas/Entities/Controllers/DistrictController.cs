using Application.Features.Communities.Commands;
using Application.Features.Communities.Queries.GetAllCached;
using Application.Features.Communities.Queries.GetById;
using WebUI.Abstractions;
using WebUI.Areas.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.AuditModels;
using Application.Features.Logs.Commands;
using Application.Features.Districts.Queries;
using Application.Common.Models;
using System;

namespace WebUI.Areas.Entities.Controllers
{
    [Area("Entities")]
    public class DistrictController : BaseController<DistrictController>
    {
        // GET: DistrictController
        public IActionResult Index()
        {
            var model = new DistrictViewModel();
            return View(model);
        }

        public async Task<IActionResult> LoadAll()
        {
            var response = await _mediator.Send(new GetAllDistrictsQuery());
            if (response.Succeeded)
            {
                var viewModel = _mapper.Map<List<DistrictViewModel>>(response.Data);
                return PartialView("_ViewAll", viewModel);
            }
            return null;
        }

        public async Task<JsonResult> OnGetCreateOrEdit(int id = 0)
        {
            if (id == 0)
            {
                var districtViewModel = new DistrictViewModel();
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", districtViewModel) });
            }
            else
            {
                var response = await _mediator.Send(new GetDistrictByIdQuery() { Id = id });
                if (response.Succeeded)
                {
                    var districtViewModel = _mapper.Map<DistrictViewModel>(response.Data);
                    return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", districtViewModel) });
                }
                return null;
            }
        }

        [HttpPost]
        public async Task<JsonResult> OnPostCreateOrEdit(int id, DistrictViewModel district)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    var createDistrictCommand = _mapper.Map<CreateDistrictCommand>(district);
                    var result = await _mediator.Send(createDistrictCommand);

                    if (result.Succeeded)
                    {
                        id = result.Data;

                        Log log = new Log()
                        {
                            UserId = _userService.UserId,
                            Action = "Create",
                            TableName = "District",
                            NewValues = district
                        };

                        await _mediator.Send(new AddLogCommand() { Log = log });

                        _notify.Success($"Район {district.Name} створений");
                    }
                    else
                        _notify.Error("Помилка створення");
                }
                else
                {
                    var oldDistrict = _mediator.Send(new GetDistrictByIdQuery() { Id = district.Id }).Result.Data;
                    var updateDistrictCommand = _mapper.Map<UpdateDistrictCommand>(district);
                    var result = await _mediator.Send(updateDistrictCommand);

                    if (result.Succeeded)
                    {
                        Log log = new Log()
                        {
                            UserId = _userService.UserId,
                            Action = "Update",
                            TableName = "District",
                            OldValues = oldDistrict,
                            NewValues = district,
                            Key = oldDistrict.Id.ToString(),
                        };

                        await _mediator.Send(new AddLogCommand() { Log = log });

                        _notify.Success($"Район {district.Name} змінений");
                    }
                }

                var response = await _mediator.Send(new GetAllDistrictsQuery());

                if (response.Succeeded)
                {
                    var viewModel = _mapper.Map<List<DistrictViewModel>>(response.Data);
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


                var html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", district);
                return new JsonResult(new { isValid = false, html = html });
            }
        }

        [HttpPost]
        public async Task<JsonResult> OnPostDelete(int id)
        {
            var byIdResponse = await _mediator.Send(new GetDistrictByIdQuery() { Id = id });
            var district = _mapper.Map<DistrictViewModel>(byIdResponse.Data);
            Result<int> deleteCommand;

            try
            {
                deleteCommand = await _mediator.Send(new DeleteDistrictCommand { Id = id });

                if (deleteCommand.Succeeded)
                {
                    Log log = new Log()
                    {
                        UserId = _userService.UserId,
                        Action = "Delete",
                        TableName = "District",
                        OldValues = district,
                        Key = district.Id.ToString()
                    };

                    await _mediator.Send(new AddLogCommand() { Log = log });

                    _notify.Success($"Район {district.Name} видалений");
                    var response = await _mediator.Send(new GetAllDistrictsQuery());

                    if (response.Succeeded)
                    {
                        var viewModel = _mapper.Map<List<DistrictViewModel>>(response.Data);
                        var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
                        return new JsonResult(new { isValid = true, html = html });
                    }
                }

                _notify.Error("Помилка видалення");
                return null;
            }
            catch (Exception)
            {
                _notify.Error("Помилка видалення. Район має зв'язки");
                return null;
            }
        }
    }
}
