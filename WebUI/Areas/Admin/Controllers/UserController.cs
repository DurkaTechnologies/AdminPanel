using AdminPanel.Application.Enums;
using AdminPanel.Application.Features.Communities.Queries.GetAllCached;
using AdminPanel.Infrastructure.Identity.Models;
using AdminPanel.Web.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebUI.Areas.Admin.Models;
using WebUI.Areas.Entities.Models;
using WebUI.Services;
using System.IO;
using System;

namespace WebUI.Areas.Admin
{
    [Area("Admin")]
    public class UserController : BaseController<UserController>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public object ApplicationUser { get; private set; }

        public UserController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _webHostEnvironment = webHostEnvironment;

            ImageService.RootPass = _webHostEnvironment.WebRootPath;
        }

        //[Authorize(Policy = Permissions.Users.View)]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> LoadAll()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var allUsersExceptCurrentUser = await _userManager.Users.Where(a => a.Id != currentUser.Id).ToListAsync();
            var model = _mapper.Map<IEnumerable<UserViewModel>>(allUsersExceptCurrentUser);

            return PartialView("_ViewAll", model);
        }

        public async Task<IActionResult> OnGetCreate()
        {
            var response = await _mediator.Send(new GetAllCommunitiesCachedQuery());

            if (response.Succeeded)
            {
                var data = _mapper.Map<IEnumerable<CommunityViewModel>>(response.Data);
                var communities = new SelectList(data, nameof(CommunityViewModel.Id), nameof(CommunityViewModel.Name), null, null);
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_Create", new UserViewModel() { Communities = communities }) });
            }

            _notify.Success("Не заповнено форму");
            return null;
        }

        [HttpPost]
        public async Task<IActionResult> OnPostCreate(UserViewModel userModel, string image)
        {
            if (ModelState.IsValid)
            {
                string imagePath = null;

                if (Request.Form.Files.Count > 0)
                    imagePath = ImageService.SaveImage(Request.Form.Files);

                MailAddress address = new MailAddress(userModel.Email);
                string userName = address.User;
                var user = new ApplicationUser
                {
                    Email = userModel.Email,
                    UserName = userModel.Email,
                    FirstName = userModel.FirstName,
                    MiddleName = userModel.MiddleName,
                    LastName = userModel.LastName,
                    ProfilePicture = imagePath,
                    EmailConfirmed = true,
                    IsActive = true,
                    СommunityId = userModel.CommunityId,
                    Description = userModel.Description
                };

                var result = await _userManager.CreateAsync(user, userModel.Password);
                if (result.Succeeded)
                {

                    await _userManager.AddToRoleAsync(user, Roles.Worker.ToString());
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                    var allUsersExceptCurrentUser = await _userManager.Users.Where(a => a.Id != currentUser.Id).ToListAsync();
                    var users = _mapper.Map<IEnumerable<UserViewModel>>(allUsersExceptCurrentUser);
                    var htmlData = await _viewRenderer.RenderViewToStringAsync("_ViewAll", users);

                    _notify.Success($"Аккаунт {user.Email} створено");
                    return new JsonResult(new { isValid = true, html = htmlData });
                }
                foreach (var error in result.Errors)
                {
                    _notify.Error(error.Description);
                }
                var html = await _viewRenderer.RenderViewToStringAsync("_Create", userModel);
                return new JsonResult(new { isValid = false, html = html });
            }
            return default;
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<JsonResult> OnPostDelete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                if (user.FirstName != "Super" && user.FirstName != "Default")
                {
                    _notify.Success($"Користувач {user.FirstName + " " + user.LastName} був успішно видалений");
                    await _userManager.DeleteAsync(user);
                }
                else
                    _notify.Error($"Не можна видалити базових користувачів");

            }
            else
                _notify.Error($"Користувача не знайдено");

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var allUsersExceptCurrentUser = await _userManager.Users.Where(a => a.Id != currentUser.Id).ToListAsync();
            var model = _mapper.Map<IEnumerable<UserViewModel>>(allUsersExceptCurrentUser);
            var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", model);

            return new JsonResult(new { isValid = true, html = html });
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<JsonResult> OnPostDeactivate(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                if (user.FirstName != "Super" && user.FirstName != "Default")
                {
                    user.IsActive = !user.IsActive;

                    if (user.IsActive)
                        _notify.Success($"Користувач {user.FirstName + " " + user.LastName} активований");
                    else
                        _notify.Success($"Користувач {user.FirstName + " " + user.LastName} деактивований");

                    await _userManager.UpdateAsync(user);
                }
                else
                    _notify.Error($"Не можна деактивувати базових користувачів");

            }
            else
                _notify.Error($"Користувача не знайдено");

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var allUsersExceptCurrentUser = await _userManager.Users.Where(a => a.Id != currentUser.Id).ToListAsync();
            var model = _mapper.Map<IEnumerable<UserViewModel>>(allUsersExceptCurrentUser);
            var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", model);

            return new JsonResult(new { isValid = true, html = html });
        }

        public async Task<IActionResult> Profile(string id)
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

            return RedirectToAction("Profile", id);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                /*Image*/
                string imagePath = null;

                if (Request.Form.Files.Count > 0)
                {
                    imagePath = ImageService.SaveImage(Request.Form.Files);
                    user.ProfilePicture = imagePath;
                }

                /*Claims Get*/
                ApplicationUser appUser;

                if (user.Id == null)
                    appUser = await _userManager.GetUserAsync(User);
                else
                    appUser = await _userManager.FindByIdAsync(user.Id);

                if (imagePath != null)
                {
                    if(appUser.ProfilePicture != null)
                        ImageService.DeleteImage(appUser.ProfilePicture);

                    appUser.ProfilePicture = imagePath;
                }
                else
                    user.ProfilePicture = appUser.ProfilePicture;

                /*User Set*/
                user.Id = null;
                appUser.СommunityId = user.CommunityId;
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

            return View(user);
        }
    }
}
