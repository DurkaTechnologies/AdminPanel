﻿using System;

namespace Domain.Common.Interfaces
{
	public interface IAudit
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public string Type { get; set; }
		public string TableName { get; set; }
		public DateTime DateTime { get; set; }
		public string OldValues { get; set; }
		public string NewValues { get; set; }
		public string AffectedColumns { get; set; }
		public string PrimaryKey { get; set; }
	}

	public interface ILog
	{
		public string UserId { get; set; }
		public string Action { get; set; }
		public string TableName { get; set; }
		public object OldValues { get; set; }
		public object NewValues { get; set; }
		public string Key { get; set; }
	}
}
