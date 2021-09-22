using FluentValidation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
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

        public string FullName { get => FirstName + " " + MiddleName + " " + LastName; }

        public string ProfilePicture { get; set; }

		public string Email { get; set; }

		public bool IsActive { get; set; } = true;

		public string Password { get; set; }

		public string ConfirmPassword { get; set; }

		public bool EmailConfirmed { get; set; }

		public string PhoneNumber { get; set; }

		public string Description { get; set; }

		public string Chat { get; set; }

		public ICollection<CommunityViewModel> Communities { get; set; }

		public List<int> CommunitiesSelected { get; set; }

		public SelectList CommunitiesList { get; set; }
	}

	public class UserViewModelValidator : AbstractValidator<UserViewModel>
	{
		public UserViewModelValidator()
		{
			RuleFor(x => x.FirstName).NotEmpty().WithMessage("Ім'я не може бути пустим");
			RuleFor(x => x.MiddleName).NotEmpty().WithMessage("Прізвище не може бути пустим");
			RuleFor(x => x.LastName).NotEmpty().WithMessage("По Батькові не може бути пустим");
			RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Номер телефону не дійсний");
			RuleFor(x => x.Email).EmailAddress().WithMessage("Електроний адрес не дійсний");
		}
	}
}
