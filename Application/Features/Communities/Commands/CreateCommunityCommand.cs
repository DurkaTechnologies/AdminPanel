using AdminPanel.Application.Interfaces.Repositories;
using AdminPanel.Domain.Entities;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AdminPanel.Application.Common.Models;

namespace TestOnion.Application.Features.Communities.Commands
{
	public partial class CreateCommunityCommand : IRequest<Result<int>>
	{
		public string Name { get; set; }

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
				var product = mapper.Map<Community>(request);
				await communityRepository.InsertAsync(product);
				await unitOfWork.Commit(cancellationToken);
				return Result<int>.Success(product.Id);
			}
		}
	}
}
