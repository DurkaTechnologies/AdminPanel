﻿using Domain.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Infrastructure.Identity.Models
{
	public class ApplicationUser : IdentityUser, IWorker
	{
		public ApplicationUser()
		{
			Correspondences = new HashSet<Correspondence>();
			Communities = new HashSet<Community>();
		}

		#region Worker fields
		public string FirstName { get; set; }

		public string MiddleName { get; set; }

		public string LastName { get; set; }

		public string ProfilePicture { get; set; }

		public string Description { get; set; }

		public string Chat { get; set; }

		public virtual ICollection<Community> Communities { get; set; }
		#endregion

		public bool IsActive { get; set; } = false;

		public virtual ICollection<Correspondence> Correspondences { get; set; }
	}
}
