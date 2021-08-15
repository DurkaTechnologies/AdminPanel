using AdminPanel.Domain.Common.Interfaces;
using AdminPanel.Domain.Common.Models;
using AdminPanel.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace AdminPanel.Infrastructure.Identity.Models
{
	public class ApplicationUser : IdentityUser, IWorker
	{
		#region Worker fields
		public string FirstName { get; set; }

		public string MiddleName { get; set; }

		public string LastName { get; set; }

		public string ProfilePicture { get; set; }

		public int СommunityId { get; set; }

		public Community Community { get; set; }
		#endregion

		public bool IsActive { get; set; } = false;

		#region Auditable Entity
		public DateTime Created { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? LastModified { get; set; }

		public string LastModifiedBy { get; set; }
		#endregion
	}
}
