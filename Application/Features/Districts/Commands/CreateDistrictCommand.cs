using Application.Interfaces.Repositories;
using Domain.Entities;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;

namespace Application.Features.Communities.Commands
{
	public partial class CreateDistrictCommand : IRequest<Result<int>>
	{
		public string Name { get; set; }

		public class CreateDistrictCommandHandler : IRequestHandler<CreateDistrictCommand, Result<int>>
		{
			private readonly IDistrictRepository districtRepository;
			private readonly IMapper mapper;

			private IUnitOfWork unitOfWork { get; set; }

			public CreateDistrictCommandHandler(IDistrictRepository districtRepository, IUnitOfWork unitOfWork, IMapper mapper)
			{
				this.districtRepository = districtRepository;
				this.unitOfWork = unitOfWork;
				this.mapper = mapper;
			}

			public async Task<Result<int>> Handle(CreateDistrictCommand request, CancellationToken cancellationToken)
			{
				var district = mapper.Map<District>(request);
				await districtRepository.InsertAsync(district);
				await unitOfWork.Commit(cancellationToken);
				return Result<int>.Success(district.Id);
			}
		}
	}
}
