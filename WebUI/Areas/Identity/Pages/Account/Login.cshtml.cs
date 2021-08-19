using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using AdminPanel.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using AspNetCoreHero.ToastNotification.Abstractions;
using AdminPanel.Web.Abstractions;

namespace WebUI.Areas.Identity.Pages.Account
{
	[AllowAnonymous]
	public class LoginModel : BasePageModel<LoginModel>
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly ILogger<LoginModel> _logger;

		public LoginModel(SignInManager<ApplicationUser> signInManager,
			ILogger<LoginModel> logger,
			UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_logger = logger;
		}

		[BindProperty]
		public InputModel Input { get; set; }

		public IList<AuthenticationScheme> ExternalLogins { get; set; }

		public string ReturnUrl { get; set; }

		[TempData]
		public string ErrorMessage { get; set; }

		public class InputModel
		{
			[Required]
			[EmailAddress]
			public string Email { get; set; }

			[Required]
			[DataType(DataType.Password)]
			public string Password { get; set; }

			[Display(Name = "Remember me?")]
			public bool RememberMe { get; set; }
		}

		public async Task OnGetAsync(string returnUrl = null)
		{
			if (!string.IsNullOrEmpty(ErrorMessage))
			{
				ModelState.AddModelError(string.Empty, ErrorMessage);
			}

			returnUrl ??= Url.Content("~/");

			// Clear the existing external cookie to ensure a clean login process
			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

			ReturnUrl = returnUrl;
		}

		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
		{
			returnUrl = returnUrl ?? Url.Content("~/");

			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

			if (ModelState.IsValid)
			{
				var userName = Input.Email;
				if (IsValidEmail(Input.Email))
				{
					var userCheck = await _userManager.FindByEmailAsync(Input.Email);
					if (userCheck != null)
					{
						userName = userCheck.UserName;
					}
				}

				var user = await _userManager.FindByNameAsync(userName);

				var result = await _signInManager.PasswordSignInAsync(userName, Input.Password, Input.RememberMe, lockoutOnFailure: false);

				if (result.Succeeded)
				{
					_logger.LogInformation("User logged in.");

					if (user.FirstName == null || user.LastName == null)
						_notyf.Success($"Вітаю {userName}.");
					else
						_notyf.Success($"Вітаю {user.FirstName + " " + user.LastName}.");

					return LocalRedirect(returnUrl);
				}
				if (result.RequiresTwoFactor)
				{
					return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
				}
				if (result.IsLockedOut)
				{
					_notyf.Warning("User account locked out.");
					_logger.LogWarning("User account locked out.");
					return RedirectToPage("./Lockout");
				}
				else
				{
					_notyf.Error("Invalid login attempt.");
					ModelState.AddModelError(string.Empty, "Invalid login attempt.");
					return Page();
				}
			}

			// If we got this far, something failed, redisplay form
			return Page();
		}

		public async Task LoginBySuperAdmin()
		{
			Console.WriteLine("Dsdsadsadsadas");
			Input = new InputModel();
			Input.Email = "superadmin@gmail.com";
			Input.Password = "123Pa$$word!";
			Input.RememberMe = false;
			await OnPostAsync();
		}


		public bool IsValidEmail(string emailaddress)
		{
			try
			{
				MailAddress m = new MailAddress(emailaddress);

				return true;
			}
			catch (FormatException)
			{
				return false;
			}
		}
	}
}
