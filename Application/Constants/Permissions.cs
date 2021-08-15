using System.Collections.Generic;

namespace AdminPanel.Application.Constants
{
	public static class Permissions
	{
		public static List<string> GeneratePermissionsForModule(string module)
		{
			return new List<string>()
			{
				$"Permissions.{module}.Create",
				$"Permissions.{module}.View",
				$"Permissions.{module}.Edit",
				$"Permissions.{module}.Delete",
			};
		}

		public static class Users
		{
			public const string View = "Permissions.Users.View";
			public const string Create = "Permissions.Users.Create";
			public const string Edit = "Permissions.Users.Edit";
			public const string Delete = "Permissions.Users.Delete";
		}

		public static class Communities
		{
			public const string View = "Permissions.Communities.View";
			public const string Create = "Permissions.Communities.Create";
			public const string Edit = "Permissions.Communities.Edit";
			public const string Delete = "Permissions.Communities.Delete";
		}
	}
}
