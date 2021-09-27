﻿using System;
using System.Collections.Generic;

namespace AdminPanel.Domain.Common.Models
{
	public interface IHasDomainEvent
	{
		public List<DomainEvent> DomainEvents { get; set; }
	}

	public abstract class DomainEvent
	{
		protected DomainEvent()
		{
			DateOccurred = DateTimeOffset.UtcNow;
		}

		public bool IsPublished { get; set; }
		public DateTimeOffset DateOccurred { get; protected set; } = DateTime.UtcNow;
	}
}
