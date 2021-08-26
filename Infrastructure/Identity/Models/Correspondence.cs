using System.Collections.Generic;

namespace Infrastructure.Identity.Models
{
	public class Correspondence
	{
		public Correspondence()
		{
			Messages = new HashSet<Message>();
		}

		public int Id { get; set; }

		public string RequestNumber { get; set; }

		public string AskerNumber { get; set; }

		public string WorkerId { get; set; }

		public virtual ApplicationUser Worker { get; set; }

		public virtual ICollection<Message> Messages { get; set; }
	}
}
