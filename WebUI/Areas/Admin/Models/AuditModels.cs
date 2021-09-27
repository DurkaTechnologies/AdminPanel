namespace WebUI.Areas.Admin.Models
{
	public class AuditUserModel
	{
		public string Id { get; set; }

		public string FullName { get; set; }

		public string UserName { get; set; }

		public string Email { get; set; }

		public AuditUserModel(UserViewModel user)
		{
			Id = user.Id;
			FullName = user.MiddleName + " " + user.FirstName + " " + user.LastName;
			UserName = user.UserName;
			Email = user.Email;
		}
	}
}
