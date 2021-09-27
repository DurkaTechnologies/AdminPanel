using AdminPanel.Application.Interfaces.Repositories;
using Domain.Entities;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AdminPanel.Application.Common.Models;

namespace AdminPanel.Application.Features.Communities.Commands
{
	public partial class CreateCommunityCommand : IRequest<Result<int>>
	{
		public string Name { get; set; }

		public int? DistrictId { get; set; }

		public class CreateCommunityCommandHandler : IRequestHandler<CreateCommunityCommand, Result<int>>
		{
			private readonly ICommunityRepository communityRepository;
			private readonly IMapper mapper;

			private IUnitOfWork unitOfWork { get; set; }

			public CreateCommunityCommandHandler(ICommunityRepository communityRepository, IUnitOfWork unitOfWork, IMapper mapper)
			{
				this.communityRepository = communityRepository;
				this.unitOfWork = unitOfWork;
				this.mapper = mapper;
			}

			public async Task<Result<int>> Handle(CreateCommunityCommand request, CancellationToken cancellationToken)
			{
				var community = mapper.Map<Community>(request);
				await communityRepository.InsertAsync(community);
				await unitOfWork.Commit(cancellationToken);
				return Result<int>.Success(community.Id);
			}
		}
	}
}
