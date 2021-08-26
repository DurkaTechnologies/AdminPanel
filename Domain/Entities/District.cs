using System.Collections.Generic;

namespace Domain.Entities
{
	public class District
	{
		public District()
		{
			Communities = new HashSet<Community>();
		}

		public int Id { get; set; }

		public string Name { get; set; }

		public virtual ICollection<Community> Communities { get; set; }
	}
}
