using System;

namespace Infrastructure.Identity.Models
{
	public class Message
	{
		public int Id { get; set; }

		public string Content { get; set; }

		public bool IsWorker { get; set; }

		public DateTime Date { get; set; }

		public int CorrespondenceId { get; set; }

		public virtual Correspondence Correspondence { get; set; }
	}
}
