﻿using Domain.Common.Interfaces;
using System;

namespace Domain.Common.Models
{
	public class AuditableEntity : IAuditableEntity
	{
		public DateTime Created { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? LastModified { get; set; }
		public string LastModifiedBy { get; set; }
	}
}
