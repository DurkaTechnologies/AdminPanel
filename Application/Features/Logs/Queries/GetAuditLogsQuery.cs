using Application.Common.Models;
using Application.DTOs;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ActivityLog.Queries
{
	public class GetAuditLogsQuery : IRequest<Result<List<AuditLogResponse>>>
	{
		public string userId { get; set; }

		public GetAuditLogsQuery()
		{
		}
	}

	public class GetAuditLogsQueryHandler : IRequestHandler<GetAuditLogsQuery, Result<List<AuditLogResponse>>>
	{
		private readonly ILogRepository logRepository;
		private readonly IMapper mapper;

		public GetAuditLogsQueryHandler(ILogRepository logRepository, IMapper mapper)
		{
			this.logRepository = logRepository;
			this.mapper = mapper;
		}

		public async Task<Result<List<AuditLogResponse>>> Handle(GetAuditLogsQuery request, CancellationToken cancellationToken)
		{
			var logs = await logRepository.GetAuditLogsAsync(request.userId);
			return Result<List<AuditLogResponse>>.Success(logs);
		}
	}
}
