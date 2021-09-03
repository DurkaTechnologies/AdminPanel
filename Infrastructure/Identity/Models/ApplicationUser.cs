using Domain.Common.Interfaces;
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
		}

		#region Worker fields
		public string FirstName { get; set; }

		public string MiddleName { get; set; }

		public string LastName { get; set; }

		public string ProfilePicture { get; set; }

		public int? CommunityId { get; set; }

		public string Description { get; set; }

		public Community Community { get; set; }
		#endregion

		public bool IsActive { get; set; } = false;

        public virtual ICollection<Correspondence> Correspondences { get; set; }
    }
}
