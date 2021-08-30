using AdminPanel.Application.Enums;
using AdminPanel.Application.Features.Communities.Queries.GetAllCached;
using AdminPanel.Web.Abstractions;
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
using Newtonsoft.Json;
using AdminPanel.Infrastructure.AuditModels;
using Application.Features.Logs.Commands;
using Infrastructure.Identity.Models;

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

            _notify.Error("Не заповнено форму");
            return null;
        }

        [HttpPost]
        public async Task<IActionResult> OnPostCreate(UserViewModel userModel)
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
                    UserName = userModel.UserName,
                    FirstName = userModel.FirstName,
                    MiddleName = userModel.MiddleName,
                    LastName = userModel.LastName,
                    ProfilePicture = imagePath,
                    EmailConfirmed = true,
                    IsActive = true,
                    CommunityId = userModel.CommunityId == 0 ? null : userModel.CommunityId,
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

                    userModel.Id = user.Id;

                    Log log = new Log()
                    {
                        Action = "Create",
                        UserId = _userService.UserId,
                        TableName = "Users",
                        NewValues = new AuditUserModel(userModel),
                        Key = user.Id
                    };

                    await _mediator.Send(new AddLogCommand() { Log = log });

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

                    Log log = new Log()
                    {
                        Action = "Delete",
                        UserId = _userService.UserId,
                        TableName = "Users",
                        OldValues = new AuditUserModel(_mapper.Map<UserViewModel>(user)),
                        Key = user.Id
                    };

                    await _mediator.Send(new AddLogCommand() { Log =  log });
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

                    Log log = new Log()
                    {
                        UserId = _userService.UserId,
                        Key = user.Id,
                        TableName = "Users",
                        NewValues = new AuditUserModel(_mapper.Map<UserViewModel>(user))
                    };

                    if (user.IsActive)
                    {
                        _notify.Success($"Користувач {user.FirstName + " " + user.LastName} активований");
                        log.Action = "Activated";
                    }
                    else
                    {
                        _notify.Success($"Користувач {user.FirstName + " " + user.LastName} деактивований");
                        log.Action = "Deactivated";
                    }

                    await _userManager.UpdateAsync(user);
                    await _mediator.Send(new AddLogCommand() { Log = log });
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
    }
}
