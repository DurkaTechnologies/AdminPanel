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

		public string ApplicationUserId { get; set; }

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
					community.Name = command.Name.NullIfEmpty() ?? community.Name;
					community.DistrictId = command.DistrictId ?? community.DistrictId;
					community.ApplicationUserId = command.ApplicationUserId;
					await communityRepository.UpdateAsync(mapper.Map<Community>(command));
					await unitOfWork.Commit(cancellationToken);
					return Result<int>.Success(command.Id);
				}
			}
		}
	}

	public static class StringExtensions
	{
		public static string NullIfEmpty(this string s)
		{
			return string.IsNullOrEmpty(s) ? null : s;
		}
		public static string NullIfWhiteSpace(this string s)
		{
			return string.IsNullOrWhiteSpace(s) ? null : s;
		}
	}
}
