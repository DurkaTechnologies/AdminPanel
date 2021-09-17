﻿using Application.Features.Communities.Queries.GetAllCached;
using Application.Features.Communities.Queries.GetById;
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

namespace WebUI.Areas.Admin
{
	[Area("Admin")]
	public class ProfileController : BaseController<ProfileController>
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public ProfileController(UserManager<ApplicationUser> userManager,
			IWebHostEnvironment webHostEnvironment)
		{
			_userManager = userManager;
			_webHostEnvironment = webHostEnvironment;

			ImageService.RootPass = ENV.RootPath;
		}

		public async Task<IActionResult> Index(string id)
		{
			UserViewModel user = _mapper.Map<UserViewModel>(await GetCurrentUser(id));
			var response = await _mediator.Send(new GeUserCommunitiesQuery() { UserId = user.Id });

			if (response.Succeeded)
			{
				user.Communities = _mapper.Map<List<CommunityViewModel>>(response.Data);
				//if (user.Community != null)
				//{
				//	var result = await _mediator.Send(new GetDistrictByIdQuery() { Id = (int)user.Community?.DistrictId });
				//	if (result.Succeeded)
				//		user.Community.District = _mapper.Map<DistrictViewModel>(result.Data);
				//}
			}
			return View(user);
		}

		public async Task<IActionResult> Edit(string id)
		{
			UserViewModel user = _mapper.Map<UserViewModel>(await GetCurrentUser(id));

			var response = await _mediator.Send(new GeUserCommunitiesQuery() { UserId = user.Id });

			if (response.Succeeded)
			{
				user.CommunitiesSelected = response.Data.Select(c => c.Id);

				var responseFree = await _mediator.Send(new GetFreeCommunitiesQuery());
				if (responseFree.Succeeded)
				{
					var data = _mapper.Map<IEnumerable<CommunityViewModel>>(responseFree.Data);
					var communities = new SelectList(data, nameof(CommunityViewModel.Id),
						nameof(CommunityViewModel.Name), null, null);

					user.CommunitiesList = communities;
				}
				//else notif

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

				if (!String.IsNullOrEmpty(user.FirstName))
					appUser.FirstName = user.FirstName;
				else
					_notify.Error("Ім'я не може бути пустим");

				if (!String.IsNullOrEmpty(user.MiddleName))
					appUser.MiddleName = user.MiddleName;
				else
					_notify.Error("Прізвище не може бути пустим");

				if (!String.IsNullOrEmpty(user.LastName))
					appUser.LastName = user.LastName;
				else
					_notify.Error("По Батькові не може бути пустим");

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
				}
				catch (Exception ex)
				{
					_notify.Error(ex.Message);
				}
			}

			var response = await _mediator.Send(new GetAllCommunitiesCachedQuery());
			var data = _mapper.Map<IEnumerable<CommunityViewModel>>(response.Data);
			var communities = new SelectList(data, nameof(CommunityViewModel.Id), nameof(CommunityViewModel.Name), null, null);
			user.CommunitiesList = communities;

			return RedirectToAction(nameof(Index), new { Id = user.Id });
		}

		public async Task<IActionResult> DeleteImage(string id)
		{
			ApplicationUser appUser = await GetCurrentUser(id);

			if (appUser != null && appUser.ProfilePicture != null)
			{
				ImageService.RemoveImageFromServer(appUser.ProfilePicture);

				appUser.ProfilePicture = null;
				await _userManager.UpdateAsync(appUser);
			}

			return RedirectToAction(nameof(Index), new { Id = id });
		}

		public async Task<IActionResult> ChangeProfileImage(string id)
		{
			return new JsonResult(new { isValid = true, 
				html = await _viewRenderer.RenderViewToStringAsync("_ChangeImage", id) });
		}

		[HttpPost]
		public async Task<IActionResult> ChangeProfileImage(string id, string fileName, IFormFile blob)
		{
			try
			{
				ApplicationUser appUser = await GetCurrentUser(id);

				string imagePath;
				if (appUser != null)
				{
					imagePath = ImageService.UploadImageToServer(blob, Path.GetExtension(fileName));
					if (!String.IsNullOrEmpty(imagePath))
					{
						string oldImage = appUser.ProfilePicture;
						appUser.ProfilePicture = imagePath;
						if ((await _userManager.UpdateAsync(appUser)).Succeeded)
						{
							ImageService.RemoveImageFromServer(oldImage);
							_notify.Success($"Фото профілю успішно змінено");
						}
						else
							ImageService.RemoveImageFromServer(imagePath);
					}
					else
						return new JsonResult(new { isValid = false });
				}
				return new JsonResult(new { isValid = true });
			}
			catch (Exception)
			{
				return new JsonResult(new { isValid = false });
			}
		}

		private async Task<ApplicationUser> GetCurrentUser(string id)
		{
			if (String.IsNullOrEmpty(id))
				return await _userManager.GetUserAsync(User);
			return await _userManager.FindByIdAsync(id);
		}
	}
}
