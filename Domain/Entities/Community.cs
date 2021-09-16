namespace Domain.Entities
{
	public class Community
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public int? DistrictId { get; set; }

		public virtual District District { get; set; }

		public string ApplicationUserId { get; set; }
	}
}
