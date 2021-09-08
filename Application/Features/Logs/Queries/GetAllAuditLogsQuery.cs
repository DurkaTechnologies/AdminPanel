using Application.Common.Models;
using Application.DTOs;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Logs.Queries
{
	public class GetAllAuditLogsQuery : IRequest<Result<List<AuditLogResponse>>>
	{
		public GetAllAuditLogsQuery()
		{

		}
	}

	public class GetAllAuditLogsQueryHandler : IRequestHandler<GetAllAuditLogsQuery, Result<List<AuditLogResponse>>>
	{
		private readonly ILogRepository logRepository;
		private readonly IMapper mapper;

		public GetAllAuditLogsQueryHandler(ILogRepository logRepository, IMapper mapper)
		{
			this.logRepository = logRepository;
			this.mapper = mapper;
		}

		public async Task<Result<List<AuditLogResponse>>> Handle(GetAllAuditLogsQuery request, CancellationToken cancellationToken)
		{
			var logs = await logRepository.GetAllAuditLogsAsync();
			return Result<List<AuditLogResponse>>.Success(logs);
		}
	}
}
