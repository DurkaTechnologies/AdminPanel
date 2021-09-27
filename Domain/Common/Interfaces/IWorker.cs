using AdminPanel.Domain.Common.Models;
using Domain.Entities;

namespace AdminPanel.Domain.Common.Interfaces
{
	public interface IWorker
	{
		public string FirstName { get; set; }

		public string MiddleName { get; set; }

		public string LastName { get; set; }

		public string ProfilePicture { get; set; }

		public string Description { get; set; }

		public int? CommunityId { get; set; }

		public Community Community { get; set; }
	}
}
