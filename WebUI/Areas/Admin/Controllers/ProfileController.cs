using AdminPanel.Application.Features.Communities.Queries.GetAllCached;
using AdminPanel.Application.Features.Communities.Queries.GetById;
using AdminPanel.Web.Abstractions;
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

            ImageService.RootPass = _webHostEnvironment.WebRootPath;
        }

        public async Task<IActionResult> Index(string id)
        {
            UserViewModel user = new UserViewModel();

            if (id == null)
                user = _mapper.Map<UserViewModel>(await _userManager.GetUserAsync(User));
            else
            {
                user = _mapper.Map<UserViewModel>(await _userManager.FindByIdAsync(id));
                user.Id = id;
            }

            var response = await _mediator.Send(new GetCommunityByIdQuery() { Id = user.СommunityId });

            if (response.Succeeded)
                user.Community = _mapper.Map<CommunityViewModel>(response.Data);

            return View(user);
        }

        public async Task<IActionResult> Edit(string id)
        {
            /*User*/
            UserViewModel user;

            if (id == null)
                user = _mapper.Map<UserViewModel>(await _userManager.GetUserAsync(User));
            else
                user = _mapper.Map<UserViewModel>(await _userManager.FindByIdAsync(id));

            user.Id = id;


            /*Communities*/
            var response = await _mediator.Send(new GetAllCommunitiesCachedQuery());
            var data = _mapper.Map<IEnumerable<CommunityViewModel>>(response.Data);
            var communities = new SelectList(data, nameof(CommunityViewModel.Id), nameof(CommunityViewModel.Name), null, null);
            user.Communities = communities;

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                /*Claims Get*/
                ApplicationUser appUser;

                if (user.Id == null)
                    appUser = await _userManager.GetUserAsync(User);
                else
                    appUser = await _userManager.FindByIdAsync(user.Id);

                /*User Set*/
                appUser.CommunityId = user.CommunityId;
                appUser.Description = user.Description;

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

                    /*Other Set*/

                    if (user.UserName.ToLower() != appUser.UserName.ToLower())
                    {
                        if (await _userManager.FindByNameAsync(user.UserName) == null)
                            await _userManager.SetUserNameAsync(appUser, user.UserName);
                        else
                            _notify.Error($"Користувач {user.UserName} вже існує");
                    }

                    if (user.PhoneNumber != appUser.PhoneNumber)
                        await _userManager.SetPhoneNumberAsync(appUser, user.PhoneNumber);

                    if (user.Email.ToLower() != appUser.Email.ToLower())
                    {
                        await _userManager.SetEmailAsync(appUser, user.Email);
                        appUser.EmailConfirmed = true;
                        await _userManager.UpdateAsync(appUser);
                    }

                    _notify.Success($"Користувач {user.UserName} був успішно змінений");
                }
                catch (Exception ex)
                {
                    _notify.Error(ex.Message);
                }
            }

            /*Communities Get*/

            var response = await _mediator.Send(new GetAllCommunitiesCachedQuery());
            var data = _mapper.Map<IEnumerable<CommunityViewModel>>(response.Data);
            var communities = new SelectList(data, nameof(CommunityViewModel.Id), nameof(CommunityViewModel.Name), null, null);
            user.Communities = communities;

            return RedirectToAction(nameof(Index), new { Id = user.Id });
        }

        public async Task<IActionResult> DeleteImage(string id)
        {
            ApplicationUser appUser;

            if (id == null)
                appUser = await _userManager.GetUserAsync(User);
            else
                appUser = await _userManager.FindByIdAsync(id);


            if (appUser != null && appUser.ProfilePicture != null)
            {
                ImageService.DeleteImage(appUser.ProfilePicture);

                appUser.ProfilePicture = null;
                await _userManager.UpdateAsync(appUser);
            }

            return RedirectToAction(nameof(Index), new { Id = id });
        }

        public async Task<IActionResult> ChangeProfileImage(string id)
        {
            return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_ChangeImage", id) });
        }

        [HttpPost]
        public async Task<IActionResult> ChangeProfileImage(string id, string fileName, IFormFile blob)
        {
            try
            {
                ApplicationUser user;

                if (id == null)
                    user = await _userManager.GetUserAsync(User);
                else
                    user = await _userManager.FindByIdAsync(id);

                string imagePath;
                if (user != null)
                {
                    imagePath = ImageService.SaveImage(blob, Path.GetExtension(fileName));
                    if (!String.IsNullOrEmpty(imagePath))
                    {
                        string oldImage = user.ProfilePicture;
                        user.ProfilePicture = imagePath;
						if ((await _userManager.UpdateAsync(user)).Succeeded)
                            ImageService.DeleteImage(oldImage);
                        else
                            ImageService.DeleteImage(imagePath);

                        _notify.Success($"Фото профілю успішно змінено");

                        return new JsonResult(new { isValid = true });
                    }
                    else
                        return new JsonResult(new { isValid = false });
                }
                return new JsonResult(new { isValid = false });

            }
            catch (Exception)
            {
                return new JsonResult(new { isValid = false });
            }
        }
    }
}
