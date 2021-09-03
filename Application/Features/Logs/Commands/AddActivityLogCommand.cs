using Application.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;

namespace Application.Features.ActivityLog.Commands
{
	public partial class AddActivityLogCommand : IRequest<Result<int>>
	{
		public string Action { get; set; }
		public string userId { get; set; }
	}

	public class AddActivityLogCommandHandler : IRequestHandler<AddActivityLogCommand, Result<int>>
	{
		private readonly ILogRepository logRepository;

		private IUnitOfWork unitOfWork { get; set; }

		public AddActivityLogCommandHandler(ILogRepository logRepository, IUnitOfWork unitOfWork)
		{
			this.logRepository = logRepository;
			this.unitOfWork = unitOfWork;
		}

		public async Task<Result<int>> Handle(AddActivityLogCommand request, CancellationToken cancellationToken)
		{
			await logRepository.AddLogAsync(request.Action, request.userId);
			await unitOfWork.CommitApplicationDb(cancellationToken);
			return Result<int>.Success(1);
		}
	}
}
