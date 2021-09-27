using Domain.Common.Models;
using Domain.Entities;

namespace Domain.Common.Interfaces
{
	public interface IWorker
	{
		public string FirstName { get; set; }

		public string MiddleName { get; set; }

		public string LastName { get; set; }

		public string ProfilePicture { get; set; }

		public string Description { get; set; }

		public string Chat { get; set; }
	}
}
