using Application.Features.Communities.Queries.GetAllCached;
using Infrastructure.AuditModels;
using WebUI.Abstractions;
using Application.Features.Logs.Commands;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WebUI.Areas.Admin.Models;
using WebUI.Areas.Entities.Models;
using WebUI.Services;
using System.Linq;
using Application.Features.Communities.Commands;

namespace WebUI.Areas.Admin
{
	[Area("Admin")]
	public class ProfileController : BaseUserController<ProfileController>
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public ProfileController(UserManager<ApplicationUser> userManager,
			IWebHostEnvironment webHostEnvironment)
		{
			_userManager = userManager;
			_webHostEnvironment = webHostEnvironment;

			ImageService.RootPass = _webHostEnvironment.WebRootPath;
		}

		public async Task<IActionResult> Index(string id)
		{
			UserViewModel user = _mapper.Map<UserViewModel>(await GetCurrentUser(id));
			var response = await _mediator.Send(new GeUserCommunitiesQuery() { UserId = user.Id });

			if (response.Succeeded)
				user.Communities = _mapper.Map<List<CommunityViewModel>>(response.Data);
			else
				_notify.Success($"Помилка при завантажені громад користувача");

			return View(user);
		}

		public async Task<IActionResult> Edit(string id)
		{
			UserViewModel user = _mapper.Map<UserViewModel>(await GetCurrentUser(id));
			var response = await _mediator.Send(new GeUserCommunitiesQuery() { UserId = user.Id });

			if (response.Succeeded)
			{
				var responseFree = await _mediator.Send(new GetFreeCommunitiesQuery());
				if (responseFree.Succeeded)
				{
					var freeData = _mapper.Map<IEnumerable<CommunityViewModel>>(responseFree.Data).ToList();
					var data = _mapper.Map<IEnumerable<CommunityViewModel>>(response.Data);
					freeData.AddRange(data);
					user.CommunitiesSelected = data.Select(c => c.Id).ToList();

					user.CommunitiesList = new SelectList(freeData.OrderBy(x => x.District.Name).ThenBy(x => x.Name),
						nameof(CommunityViewModel.Id), nameof(CommunityViewModel.Name),
						null, "District.Name");
				}
				return View(user);
			}

			_notify.Error("Помилка завантаження громад");
			return null;
		}

		[HttpPost]
		public async Task<IActionResult> Edit(UserViewModel user)
		{
			if (ModelState.IsValid)
			{
				ApplicationUser appUser = await GetCurrentUser(user.Id);

				Log log = new Log()
				{
					UserId = _userService.UserId,
					Action = "Update",
					TableName = "Users",
					Key = appUser.Id,
					OldValues = new AuditUserModel(_mapper.Map<UserViewModel>(appUser)),
					NewValues = new AuditUserModel(user)
				};

				appUser.Description = user.Description;
				appUser.Chat = user.Chat;
				appUser.FirstName = user.FirstName;
				appUser.MiddleName = user.MiddleName;
				appUser.LastName = user.LastName;

				var response = await _mediator.Send(new GeUserCommunitiesQuery() { UserId = user.Id });
				if (response.Succeeded)
				{
					if (user.CommunitiesSelected != null)
					{
						var currentCommunitiesIds = response.Data.Select(c => c.Id);

						var newCommunities = user.CommunitiesSelected.Except(currentCommunitiesIds);
						var delCommunities = currentCommunitiesIds.Except(user.CommunitiesSelected);
						await ExecuteUpdateCommands(GenerateUpdate(newCommunities, user.Id));
						await ExecuteUpdateCommands(GenerateUpdate(delCommunities, null));
					}
					else
					{
						var delAll = response.Data.Select(c => new UpdateCommunityCommand()
						{
							Id = c.Id,
							ApplicationUserId = null
						});
						await ExecuteUpdateCommands(delAll);
					}
				}

				try
				{
					await _userManager.UpdateAsync(appUser);

					if (user.PhoneNumber != appUser.PhoneNumber)
						await _userManager.SetPhoneNumberAsync(appUser, user.PhoneNumber);

					if (user.Email.ToLower() != appUser.Email.ToLower())
					{
						await _userManager.SetEmailAsync(appUser, user.Email);
						appUser.EmailConfirmed = true;
						await _userManager.UpdateAsync(appUser);
					}

					_notify.Success($"Користувач {user.UserName} був успішно змінений");

					await _mediator.Send(new AddLogCommand() { Log = log });
					return RedirectToAction(nameof(Index), new { Id = user.Id });
				}
				catch (Exception ex)
				{
					_notify.Error(ex.Message);
				}
			}

			var freeData = await GetAllFreeData(user.Id);

			user.CommunitiesList = new SelectList(freeData.OrderBy(x => x.District.Name).ThenBy(x => x.Name),
				nameof(CommunityViewModel.Id), nameof(CommunityViewModel.Name),
				null, "District.Name");

			return View(user);
		}

		public async Task<IActionResult> DeleteImage(string id)
		{
			ApplicationUser appUser = await GetCurrentUser(id);

			if (appUser != null && appUser.ProfilePicture != null)
			{
				if (ImageService.DeleteImageLocal(appUser.ProfilePicture))
				{
					appUser.ProfilePicture = null;

					if ((await _userManager.UpdateAsync(appUser)).Succeeded)
						_notify.Success($"Фото профілю успішно змінено");
					else
						_notify.Success($"Помилка при видалені фото");
				}
			}
			return RedirectToAction(nameof(Edit), new { Id = id });
		}

		public async Task<IActionResult> ChangeProfileImage(string id)
		{
			return new JsonResult(new
			{
				isValid = true,
				html = await _viewRenderer.RenderViewToStringAsync("_ChangeImage", id)
			});
		}

		[HttpPost]
		public async Task<IActionResult> ChangeProfileImage(string id, string fileName, IFormFile blob)
		{
			try
			{
				ApplicationUser appUser = await GetCurrentUser(id);

				if (appUser != null)
				{
					string imagePath = ImageService.SaveImageLocal(blob, Path.GetExtension(fileName));
					if (!String.IsNullOrEmpty(imagePath))
					{
						string oldImage = appUser.ProfilePicture;
						appUser.ProfilePicture = imagePath;
						if ((await _userManager.UpdateAsync(appUser)).Succeeded)
						{
							ImageService.DeleteImageLocal(oldImage);
							_notify.Success($"Фото профілю успішно змінено");
						}
						else
							ImageService.DeleteImageLocal(imagePath);
					}
				}
				return new JsonResult(new { isValid = true });
			}
			catch (Exception)
			{
				return new JsonResult(new { isValid = false });
			}
		}

		private async Task<ApplicationUser> GetCurrentUser(string userId)
		{
			if (String.IsNullOrEmpty(userId))
				return await _userManager.GetUserAsync(User);
			return await _userManager.FindByIdAsync(userId);
		}

		private async Task<IEnumerable<CommunityViewModel>> GetAllFreeData(string userId)
		{
			var response = await _mediator.Send(new GeUserCommunitiesQuery() { UserId = userId });

			if (response.Succeeded)
			{
				var responseFree = await _mediator.Send(new GetFreeCommunitiesQuery());
				if (responseFree.Succeeded)
				{
					var freeData = _mapper.Map<IEnumerable<CommunityViewModel>>(responseFree.Data).ToList();
					var data = _mapper.Map<IEnumerable<CommunityViewModel>>(response.Data);
					freeData.AddRange(data);
					return freeData;
				}
			}
			return new List<CommunityViewModel>();
		}
	}
}
