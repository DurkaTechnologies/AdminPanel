using Application.Common.Models;
using Application.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Communities.Commands
{
	public class UpdateDistrictCommand : IRequest<Result<int>>
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public class UpdateDistrictCommandHandler : IRequestHandler<UpdateDistrictCommand, Result<int>>
		{
			private readonly IUnitOfWork unitOfWork;
			private readonly IDistrictRepository districtRepository;

			public UpdateDistrictCommandHandler(IDistrictRepository districtRepository, IUnitOfWork unitOfWork)
			{
				this.districtRepository = districtRepository;
				this.unitOfWork = unitOfWork;
			}

			public async Task<Result<int>> Handle(UpdateDistrictCommand command, CancellationToken cancellationToken)
			{
				var district = await districtRepository.GetByIdAsync(command.Id);

				if (district == null)
					return Result<int>.Failure($"Brand Not Found.");
				else
				{
					district.Name = command.Name ?? district.Name;
					await districtRepository.UpdateAsync(district);
					await unitOfWork.Commit(cancellationToken);
					return Result<int>.Success(district.Id);
				}
			}
		}
	}
}
