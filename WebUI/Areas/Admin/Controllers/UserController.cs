using Application.Enums;
using Application.Features.Communities.Queries.GetAllCached;
using WebUI.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using WebUI.Areas.Admin.Models;
using WebUI.Areas.Entities.Models;
using WebUI.Services;
using Infrastructure.AuditModels;
using Application.Features.Logs.Commands;
using Infrastructure.Identity.Models;
using System.IO;
using System;
using Application.Features.Communities.Commands;

namespace WebUI.Areas.Admin
{
	[Area("Admin")]
	public class UserController : BaseController<UserController>
	{
		#region Fields

		private readonly UserManager<ApplicationUser> _userManager;

		private readonly IWebHostEnvironment _webHostEnvironment;

		public object ApplicationUser { get; private set; }

		#endregion

		public UserController(UserManager<ApplicationUser> userManager,
			IWebHostEnvironment webHostEnvironment)
		{
			_userManager = userManager;
			_webHostEnvironment = webHostEnvironment;

			ImageService.RootPass = ENV.RootPath;
		}

		#region Main Controller Methods

		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> LoadAll()
		{
			return PartialView("_ViewAll", await GetUsersExceptCurrentAsync());
		}

		[Authorize(Roles = "SuperAdmin")]
		public async Task<IActionResult> OnGetCreate()
		{
			var response = await _mediator.Send(new GetFreeCommunitiesQuery());

			if (response.Succeeded)
			{
				if (response.Data.Count == 0)
					_notify.Warning("Немає вільних громад");

				var data = _mapper.Map<IEnumerable<CommunityViewModel>>(response.Data);

				var freeCommunities = new SelectList(data.OrderBy(x => x.District.Name).ThenBy(x => x.Name),
					nameof(CommunityViewModel.Id), nameof(CommunityViewModel.Name), null, "District.Name");

				return new JsonResult(new
				{
					isValid = true,
					html = await _viewRenderer.RenderViewToStringAsync("_Create",
					new UserViewModel() { CommunitiesList = freeCommunities })
				});
			}

			_notify.Error("Помилка завантаження громад");
			return null;
		}

		[HttpPost]
		[Authorize(Roles = "SuperAdmin")]
		public async Task<IActionResult> OnPostCreate(UserViewModel userModel, string fileName, IFormFile blob)
		{
			if (ModelState.IsValid)
			{
				string imagePath;
				userModel.Password ??= "1";

				try
				{
					imagePath = ImageService.UploadImageToServer(blob, Path.GetExtension(fileName));
				}
				catch (Exception)
				{
					return await FileUploadError(userModel);
				}

				if (String.IsNullOrEmpty(imagePath))
					return await FileUploadError(userModel);

				MailAddress address = new MailAddress(userModel.Email);
				string userName = address.User;
				var user = new ApplicationUser
				{
					Email = userModel.Email,
					UserName = userModel.Email,
					FirstName = userModel.FirstName,
					MiddleName = userModel.MiddleName,
					LastName = userModel.LastName,
					PhoneNumber = userModel.PhoneNumber,
					Chat = userModel.Chat,
					ProfilePicture = imagePath,
					EmailConfirmed = true,
					IsActive = true,
					Description = userModel.Description
				};

				var result = await _userManager.CreateAsync(user, userModel.Password);

				if (result.Succeeded)
				{
					await _userManager.AddToRoleAsync(user, Roles.Worker.ToString());

					var commands = userModel.CommunitiesSelected.Select(id => new UpdateCommunityCommand()
					{
						Id = id,
						ApplicationUserId = user.Id
					});

					// ?
					foreach (var command in commands.ToList())
					{
						var communityRes = await _mediator.Send(command);

						if (communityRes.Succeeded)
						{
							_notify.Information($"Громада {communityRes.Data} змінена");
						}
					}

					_notify.Success($"Аккаунт {user.Email} створено");

					//userModel.Id = user.Id;

					Log log = new Log()
					{
						Action = "Create",
						UserId = _userService.UserId,
						TableName = "Users",
						NewValues = new AuditUserModel(userModel),
						Key = user.Id
					};

					await _mediator.Send(new AddLogCommand() { Log = log });

					var htmlData = await _viewRenderer.RenderViewToStringAsync("_ViewAll", await GetUsersExceptCurrentAsync());
					return new JsonResult(new { isValid = true, html = htmlData });
				}

				foreach (var error in result.Errors)
					_notify.Error(error.Description);

				ImageService.RemoveImageFromServer(imagePath);
				var html = await _viewRenderer.RenderViewToStringAsync("_Create", userModel);
				return new JsonResult(new { isValid = false, html = html });
			}
			return default;
		}

		[HttpPost]
		[Authorize(Roles = "SuperAdmin")]
		public async Task<JsonResult> OnPostDelete(string id)
		{
			var user = await _userManager.FindByIdAsync(id);

			if (user == null)
				_notify.Error($"Користувача не знайдено");
			else if (user.FirstName != "Super" && user.FirstName != "Default")
			{
				if (ImageService.RemoveImageFromServer(user.ProfilePicture))
				{
					var response = await _mediator.Send(new GeUserCommunitiesQuery() { UserId = user.Id });

					// ?
					if (response.Succeeded)
					{
						var commands = response.Data.Select(c => new UpdateCommunityCommand()
						{
							Id = c.Id,
							ApplicationUserId = null
						});


						foreach (var command in commands.ToList())
						{
							var communityRes = await _mediator.Send(command);

							if (communityRes.Succeeded)
							{
								_notify.Information($"Громада {communityRes.Data} змінена");
							}
						}
					}
					else
					{
						_notify.Error($"Помилка при отримані громад працівника");
						var htmlData = await _viewRenderer.RenderViewToStringAsync("_ViewAll", await GetUsersExceptCurrentAsync());
						return new JsonResult(new { isValid = true, html = htmlData });
					}
					

					var result = await _userManager.DeleteAsync(user);

					if (result.Succeeded)
					{
						_notify.Success($"Користувач {user.FirstName + " " + user.LastName} був успішно видалений");

						Log log = new Log()
						{
							Action = "Delete",
							UserId = _userService.UserId,
							TableName = "Users",
							OldValues = new AuditUserModel(_mapper.Map<UserViewModel>(user)),
							Key = user.Id
						};

						await _mediator.Send(new AddLogCommand() { Log = log });
					}
					else
						_notify.Error($"Помилка при видалені користувача");
				}
				else
					_notify.Error($"Помилка при видалені фото профілю з сервера.");
			}
			else
				_notify.Error($"Не можна видалити базових користувачів");

			var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", await GetUsersExceptCurrentAsync());
			return new JsonResult(new { isValid = true, html = html });
		}

		[HttpPost]
		[Authorize(Roles = "SuperAdmin")]
		public async Task<JsonResult> OnPostDeactivate(string id)
		{
			var user = await _userManager.FindByIdAsync(id);

			if (user == null)
				_notify.Error($"Користувача не знайдено");
			else if (user.FirstName != "Super" && user.FirstName != "Default")
			{
				user.IsActive = !user.IsActive;
				var result = await _userManager.UpdateAsync(user);

				if (result.Succeeded)
				{
					_notify.Success($"Користувач {user.FirstName + " " + user.LastName} " +
					(user.IsActive ? "активований" : "деактивований"));

					Log log = new Log()
					{
						UserId = _userService.UserId,
						Key = user.Id,
						TableName = "Users",
						NewValues = new AuditUserModel(_mapper.Map<UserViewModel>(user)),
						Action = user.IsActive ? "Activated" : "Deactivated"
					};

					await _mediator.Send(new AddLogCommand() { Log = log });
				}
				else
					_notify.Error($"Помилка збереження змін.");
			}
			else
				_notify.Error($"Не можна деактивувати базових користувачів");

			var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", await GetUsersExceptCurrentAsync());
			return new JsonResult(new { isValid = true, html = html });
		}

		#endregion

		#region Other Methods
		private async Task<JsonResult> FileUploadError(UserViewModel userModel)
		{
			_notify.Error("Помилка про завантаженні фото профілю");
			return new JsonResult(new
			{
				isValid = false,
				html = await _viewRenderer.RenderViewToStringAsync("_Create", userModel)
			});
		}

		private async Task<IEnumerable<UserViewModel>> GetUsersExceptCurrentAsync()
		{
			var currentUser = await _userManager.GetUserAsync(HttpContext.User);
			var allUsersExceptCurrentUser = await _userManager.Users.Where(a => a.Id != currentUser.Id).ToListAsync();
			return _mapper.Map<IEnumerable<UserViewModel>>(allUsersExceptCurrentUser);
		}
		#endregion
	}
}
