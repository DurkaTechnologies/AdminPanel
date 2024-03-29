﻿using Application.Common.Models;
using Application.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Communities.Commands
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
				var district = await districtRepository.GetIncludeByIdAsync(command.Id);
				if (district.Communities.Count == 0)
				{
					await districtRepository.DeleteAsync(district);
					await unitOfWork.Commit(cancellationToken);
					return Result<int>.Success(district.Id);
				}
				return Result<int>.Failure();

			}
		}
	}
}
