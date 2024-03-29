﻿using System;

namespace Domain.Common.Interfaces
{
	public interface IAuditableEntity
	{
		public DateTime Created { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? LastModified { get; set; }

		public string LastModifiedBy { get; set; }
	}
}
