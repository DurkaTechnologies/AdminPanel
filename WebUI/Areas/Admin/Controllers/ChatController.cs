using Infrastructure.Identity.Models;
using AdminPanel.Web.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Areas.Admin.Models;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ChatController : BaseController<UserController>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public object ApplicationUser { get; private set; }

        public ChatController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index(string id)
        {
            UserViewModel user;

            if (id == null)
                user = _mapper.Map<UserViewModel>(await _userManager.GetUserAsync(User));
            else
            {
                user = _mapper.Map<UserViewModel>(await _userManager.FindByIdAsync(id));
                user.Id = id;
            }

            return View(user);
        }

        public async Task<IActionResult> LoadAll(string id)
		{
            var allUsersExceptCurrentUser = await _userManager.Users.Where(a => a.Id != id).FirstOrDefaultAsync();
            var model = _mapper.Map<UserViewModel>(allUsersExceptCurrentUser);
            var chat = new ChatViewModel()
            {
                AnonymousIndex = "dasdsadsadsa",
                MessagesQuantity = 23,
                LastMessageTime = DateTime.Now,
                User = model
            };
            return PartialView("_ChatView", new ChatViewModel[] { chat, chat, chat, chat, chat, chat });
		}
	}
}
