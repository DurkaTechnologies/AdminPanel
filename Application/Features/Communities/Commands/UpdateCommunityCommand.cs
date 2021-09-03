using Application.Common.Models;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Communities.Commands
{
	public class UpdateCommunityCommand : IRequest<Result<int>>
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public int? DistrictId { get; set; }

		public class UpdateCommunityCommandHandler : IRequestHandler<UpdateCommunityCommand, Result<int>>
		{
			private readonly IUnitOfWork unitOfWork;
			private readonly ICommunityRepository communityRepository;
			private readonly IMapper mapper;

			public UpdateCommunityCommandHandler(ICommunityRepository communityRepository, IUnitOfWork unitOfWork, IMapper mapper)
			{
				this.communityRepository = communityRepository;
				this.unitOfWork = unitOfWork;
				this.mapper = mapper;
			}

			public async Task<Result<int>> Handle(UpdateCommunityCommand command, CancellationToken cancellationToken)
			{
				var community = await communityRepository.GetByIdAsync(command.Id);

				if (community == null)
				{
					return Result<int>.Failure($"Brand Not Found.");
				}
				else
				{
					community.Name = command.Name ?? community.Name;
					community.DistrictId = command.DistrictId ?? community.DistrictId;
					await communityRepository.UpdateAsync(mapper.Map<Community>(command));
					await unitOfWork.Commit(cancellationToken);
					return Result<int>.Success(command.Id);
				}

				
			}
		}
	}
}
