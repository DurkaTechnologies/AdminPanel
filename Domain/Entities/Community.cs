using AdminPanel.Domain.Common.Models;
using System.Collections.Generic;

namespace AdminPanel.Domain.Entities
{
	public class Community : AuditableEntity, IHasDomainEvent
	{
		public Community()
		{
			DomainEvents = new List<DomainEvent>();
		}

		public int Id { get; set; }

		public string Name { get; set; }

		public List<DomainEvent> DomainEvents { get; set; }
	}
}
