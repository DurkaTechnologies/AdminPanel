﻿using Application.Features.Communities.Queries.GetAllCached;

namespace Application.Features.Communities.Queries.GetById
{
	public class GetCommunityByIdResponse
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public int? DistrictId { get; set; }

		public GetAllDistrictsCachedResponse District { get; set; }

		public string ApplicationUserId { get; set; }
	}
}
