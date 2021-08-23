using AdminPanel.Domain.Common.Models;
using System.Collections.Generic;

namespace AdminPanel.Domain.Entities
{
	public class Community : AuditableEntity
	{
		public int Id { get; set; }

		public string Name { get; set; }
	}
}
