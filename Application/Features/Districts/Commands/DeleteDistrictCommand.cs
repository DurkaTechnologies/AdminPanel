using AdminPanel.Application.Common.Models;
using AdminPanel.Application.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AdminPanel.Application.Features.Communities.Commands
{
	public class DeleteDistrictCommand : IRequest<Result<int>>
	{
		public int Id { get; set; }

		public class DeleteDistrictCommandHandler : IRequestHandler<DeleteDistrictCommand, Result<int>>
		{
			private readonly IDistrictRepository districtRepository;
			private readonly IUnitOfWork unitOfWork;

			public DeleteDistrictCommandHandler(IDistrictRepository districtRepository, IUnitOfWork unitOfWork)
			{
				this.districtRepository = districtRepository;
				this.unitOfWork = unitOfWork;
			}

			public async Task<Result<int>> Handle(DeleteDistrictCommand command, CancellationToken cancellationToken)
			{
				var product = await districtRepository.GetByIdAsync(command.Id);
				await districtRepository.DeleteAsync(product);
				await unitOfWork.Commit(cancellationToken);
				return Result<int>.Success(product.Id);
			}
		}
	}
}
