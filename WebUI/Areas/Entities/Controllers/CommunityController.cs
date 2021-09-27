using Application.Features.Communities.Commands;
using Application.Features.Communities.Queries.GetAllCached;
using Application.Features.Communities.Queries.GetById;
using WebUI.Abstractions;
using WebUI.Areas.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Features.Logs.Commands;
using Infrastructure.AuditModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Identity.Models;
using Microsoft.EntityFrameworkCore;
using WebUI.Areas.Admin.Models;

namespace WebUI.Areas.Entities.Controllers
{
	[Area("Entities")]
	public class CommunityController : BaseController<CommunityController>
	{
		private readonly UserManager<ApplicationUser> userManager;
        public CommunityController(UserManager<ApplicationUser> userManager)
        {
			this.userManager = userManager;
        }

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
				var viewModel = _mapper.Map<List<CommunityViewModel>>(await FillUserName(response.Data));
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

				var appUsers = await userManager.Users.ToListAsync();
				appUsers.Insert(0, new ApplicationUser() { Id = ""});
				var usersData = _mapper.Map<IEnumerable<UserViewModel>>(appUsers);
				var users = new SelectList(usersData, nameof(UserViewModel.Id), nameof(UserViewModel.FullName), null, null);

				if (id == 0)
				{
					var communityViewModel = new CommunityViewModel() { Districts = districts, Users = users };
					return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", communityViewModel) });
				}
				else
				{
					var response = await _mediator.Send(new GetCommunityByIdNotCacheQuery() { Id = id });
					if (response.Succeeded)
					{
						var communityViewModel = _mapper.Map<CommunityViewModel>(response.Data);
						communityViewModel.Districts = districts;
						communityViewModel.Users = users;
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
			if (ModelState.IsValid && !string.IsNullOrEmpty(community.Name))
			{
				if (id == 0)
				{
					var createCommunityCommand = _mapper.Map<CreateCommunityCommand>(community);
					var result = await _mediator.Send(createCommunityCommand);

					if (result.Succeeded)
					{
						Log log = new Log()
						{
							UserId = _userService.UserId,
							Action = "Create",
							TableName = "Community",
							NewValues = community
						};

						await _mediator.Send(new AddLogCommand() { Log = log });

						_notify.Success($"Громада {community.Name} створена");
					}
					else
						_notify.Error("Помилка створення");
				}
				else
				{
					var old = await _mediator.Send(new GetCommunityByIdQuery() { Id = id });

					Log log = new Log()
					{
						UserId = _userService.UserId,
						Action = "Update",
						TableName = "Community",
						OldValues = old.Data,
						Key = id.ToString()
					};

					var updateCommunityCommand = _mapper.Map<UpdateCommunityCommand>(community);
					var result = await _mediator.Send(updateCommunityCommand);

					if (result.Succeeded)
					{
						log.NewValues = community;

						await _mediator.Send(new AddLogCommand() { Log = log });
						_notify.Success($"Громада {community.Name} змінена");
					}
				}

				var response = await _mediator.Send(new GetAllCommunitiesCachedQuery());

				if (response.Succeeded)
				{
					var viewModel = _mapper.Map<List<CommunityViewModel>>(await FillUserName(response.Data));
					var htmlTable = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
					return new JsonResult(new { isValid = true, html = htmlTable });
				}
				
			}
			_notify.Error("Не вказано ім'я громади");
			var html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", community);
			return new JsonResult(new { isValid = false, html = html });
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
					Log log = new Log()
					{
						UserId = _userService.UserId,
						Action = "Delete",
						TableName = "Community",
						OldValues = community,
						Key = community.Id.ToString()
					};

					await _mediator.Send(new AddLogCommand() { Log = log });

					var viewModel = _mapper.Map<List<CommunityViewModel>>(await FillUserName(response.Data));
					var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
					return new JsonResult(new { isValid = true, html = html });
				}
			}
			_notify.Error("Помилка видалення");
			return new JsonResult(new { isValid = false });
		}

		private async Task<List<GetAllCommunitiesCachedResponse>> FillUserName(List<GetAllCommunitiesCachedResponse> list)
		{
			foreach (var el in list)
			{
				ApplicationUser user = await userManager.FindByIdAsync(el.ApplicationUserId);
				if (user == null)
					el.ApplicationUserName = "Громада вільна";
				else
					el.ApplicationUserName = user.MiddleName + " " + user.FirstName + " " + user.LastName;
			}
			return list;
		}
	}
}

