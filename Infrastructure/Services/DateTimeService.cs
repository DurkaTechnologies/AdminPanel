using Application.Interfaces.Shared;
using System;

namespace Infrastructure.Services
{
	public class DateTimeService : IDateTimeService
	{
		public DateTime NowUtc => DateTime.Now;
	}
}
