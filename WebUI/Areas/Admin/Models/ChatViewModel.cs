using System;

namespace WebUI.Areas.Admin.Models
{
	public class ChatViewModel
	{
		public string AnonymousIndex { get; set; }

		public int MessagesQuantity { get; set; }

		public DateTime LastMessageTime { get; set; }

		public UserViewModel User { get; set; }
	}
}
