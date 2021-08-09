using AdminPanel.Domain.Common.Models;
using AdminPanel.Domain.Entities;

namespace AdminPanel.Domain.Common.Interfaces
{
	public interface IWorker : IAuditableEntity
	{
		public string FirstName { get; set; }

		public string MiddleName { get; set; }

		public string LastName { get; set; }

		public string ProfilePicture { get; set; }

		public int СommunityId { get; set; }

		public Community Community { get; set; }
	}
}
