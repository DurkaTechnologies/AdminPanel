using AdminPanel.Application.Interfaces.Shared;
using System;

namespace AdminPanel.Infrastructure.Services
{
	public class DateTimeService : IDateTimeService
	{
		public DateTime NowUtc => DateTime.Now;
	}
}
