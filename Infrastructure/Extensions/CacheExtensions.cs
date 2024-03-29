﻿using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Extensions
{
	public static class CacheExtensions
	{
		public static async Task<T> GetAsync<T>(this IDistributedCache distributedCache, string cacheKey, CancellationToken token = default)
		{
			byte[] utf8Bytes = await distributedCache.GetAsync(cacheKey, token).ConfigureAwait(false);
			if (utf8Bytes != null)
			{
				return JsonSerializer.Deserialize<T>(utf8Bytes);
			}
			return default;
		}

		public static async Task RemoveAsync(this IDistributedCache distributedCache, string cacheKey, CancellationToken token = default)
		{
			await distributedCache.RemoveAsync(cacheKey, token).ConfigureAwait(false);
		}

		public static async Task SetAsync<T>(this IDistributedCache distributedCache, string cacheKey, T obj, int cacheExpirationInMinutes = 30, CancellationToken token = default)
		{
			DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
			 .SetSlidingExpiration(TimeSpan.FromMinutes(cacheExpirationInMinutes));
			byte[] utf8Bytes = JsonSerializer.SerializeToUtf8Bytes<T>(obj);
			await distributedCache.SetAsync(cacheKey, utf8Bytes, options, token).ConfigureAwait(false);
		}
	}
}
