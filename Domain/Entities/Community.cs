using Domain.Common.Interfaces;
using Domain.Common.Models;
using System.Collections.Generic;

namespace Domain.Entities
{
	public class Community : AuditableEntity
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public int? DistrictId { get; set; }

		public virtual District District { get; set; }
	}
}
