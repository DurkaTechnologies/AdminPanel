﻿namespace Application.Features.Communities.Queries.GetAllCached
{
	public class GetAllCommunitiesCachedResponse
	{
		public int Id { get; set; }

		public string Name { get; set; }

        public string ApplicationUserId { get; set; }

        public string ApplicationUserName { get; set; }

        public GetAllDistrictsCachedResponse District { get; set; }
	}
}
