using AdminPanel.Application.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AdminPanel.Application.Common.Models;
using Domain.Common.Interfaces;
using System;

namespace Application.Features.Logs.Commands
{
    public partial class AddLogCommand : IRequest<Result<int>>
    {
        public ILog Log { get; set; }
    }

    public class AddActivityLogCommandHandler : IRequestHandler<AddLogCommand, Result<int>>
    {
        private readonly ILogRepository logRepository;

        private IUnitOfWork unitOfWork { get; set; }

        public AddActivityLogCommandHandler(ILogRepository logRepository, IUnitOfWork unitOfWork)
        {
            this.logRepository = logRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(AddLogCommand request, CancellationToken cancellationToken)
        {
            await logRepository.AddLogAsync(request.Log);
            await unitOfWork.CommitApplicationDb(cancellationToken);
            return Result<int>.Success(1);
        }
    }
}
