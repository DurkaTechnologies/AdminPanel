﻿using Application.Common.Models;
using Application.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Communities.Commands
{
	public class DeleteCommunityCommand : IRequest<Result<int>>
	{
		public int Id { get; set; }

		public class DeleteCommunityCommandHandler : IRequestHandler<DeleteCommunityCommand, Result<int>>
		{
			private readonly ICommunityRepository communityRepository;
			private readonly IUnitOfWork unitOfWork;

			public DeleteCommunityCommandHandler(ICommunityRepository communityRepository, IUnitOfWork unitOfWork)
			{
				this.communityRepository = communityRepository;
				this.unitOfWork = unitOfWork;
			}

			public async Task<Result<int>> Handle(DeleteCommunityCommand command, CancellationToken cancellationToken)
			{
				var community = await communityRepository.GetByIdAsync(command.Id);
				await communityRepository.DeleteAsync(community);
				await unitOfWork.Commit(cancellationToken);
				return Result<int>.Success(community.Id);
			}
		}
	}
}
