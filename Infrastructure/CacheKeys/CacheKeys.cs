namespace Infrastructure.CacheKeys
{
	public static class CommunityCacheKeys
	{
		public static string ListKey => "CommunityList";

		public static string SelectListKey => "CommunitySelectList";

		public static string GetKey(int communityId) => $"Community-{communityId}";
	}

	public static class DistrictCacheKeys
	{
		public static string ListKey => "DistrictList";

		public static string SelectListKey => "DistrictSelectList";

		public static string GetKey(int districtId) => $"District-{districtId}";
	}
}
