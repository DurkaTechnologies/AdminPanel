﻿using Microsoft.AspNetCore.Mvc.Rendering;
using WebUI.Areas.Entities.Models;

namespace WebUI.Areas.Admin.Models
{
	public class UserViewModel
	{
		public string Id { get; set; }

		public string UserName { get; set; }

		public string FirstName { get; set; }

		public string MiddleName { get; set; }

		public string LastName { get; set; }

		public string ProfilePicture { get; set; }

		public string Email { get; set; }

		public bool IsActive { get; set; } = true;

		public string Password { get; set; }

		public string ConfirmPassword { get; set; }

		public bool EmailConfirmed { get; set; }

		public string PhoneNumber { get; set; }

		public int CommunityId { get; set; }

		public string Description { get; set; }

		public CommunityViewModel Community { get; set; }

		public SelectList Communities { get; set; }
	}
}
