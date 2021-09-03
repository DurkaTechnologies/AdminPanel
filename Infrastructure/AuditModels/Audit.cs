using Domain.Common.Interfaces;
using System;

namespace Infrastructure.AuditModels
{
	public class Audit : IAudit
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

	public class Log : ILog
	{
		public string UserId { get; set; }
		public string Action { get; set; }
		public string TableName { get; set; }
		public object OldValues { get; set; }
		public object NewValues { get; set; }
		public string Key { get; set; }
	}
}
