namespace AdminPanel.Infrastructure.CacheKeys
{
	public static class CommunityCacheKeys
	{
		public static string ListKey => "CommunityList";

		public static string SelectListKey => "CommunitySelectList";

		public static string GetKey(int productId) => $"Community-{productId}";

	}
}
