using Application.DTOs;
using Domain.Common.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
	public interface ILogRepository
	{
		Task<List<AuditLogResponse>> GetAuditLogsAsync(string userId);
		Task<List<AuditLogResponse>> GetAllAuditLogsAsync();

		Task AddLogAsync(string action, string userId);

		Task AddLogAsync(ILog log);
	}
}
