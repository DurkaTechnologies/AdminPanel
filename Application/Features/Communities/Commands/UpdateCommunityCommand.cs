using AdminPanel.Application.Common.Models;
using AdminPanel.Application.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AdminPanel.Application.Features.Communities.Commands
{
	public class UpdateCommunityCommand : IRequest<Result<int>>
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public class UpdateCommunityCommandHandler : IRequestHandler<UpdateCommunityCommand, Result<int>>
		{
			private readonly IUnitOfWork unitOfWork;
			private readonly ICommunityRepository communityRepository;

			public UpdateCommunityCommandHandler(ICommunityRepository communityRepository, IUnitOfWork unitOfWork)
			{
				this.communityRepository = communityRepository;
				this.unitOfWork = unitOfWork;
			}

			public async Task<Result<int>> Handle(UpdateCommunityCommand command, CancellationToken cancellationToken)
			{
				var brand = await communityRepository.GetByIdAsync(command.Id);

				if (brand == null)
				{
					return Result<int>.Failure($"Brand Not Found.");
				}
				else
				{
					brand.Name = command.Name ?? brand.Name;
					await communityRepository.UpdateAsync(brand);
					await unitOfWork.Commit(cancellationToken);
					return Result<int>.Success(brand.Id);
				}
			}
		}
	}
}
