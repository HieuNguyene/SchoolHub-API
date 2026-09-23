using Microsoft.Extensions.Caching.Memory;

namespace SchoolHub.Application.Common
{
    public static class CacheKeys
    {
        // Class Keys
        public const string ClassesAll = "classes:all";
        public static string ClassById(string id) => $"classes:{id}";
        // Subject Keys
        public const string SubjectsAll = "subjects:all";
        public static string SubjectById(string id) => $"subjects:{id}";

        // Default Cache Expiration Configuration
        public static readonly TimeSpan DefaultAbsoluteExpiration = TimeSpan.FromMinutes(5);
        public static readonly TimeSpan DefaultSlidingExpiration = TimeSpan.FromMinutes(2);

        public static MemoryCacheEntryOptions DefaultOptions => new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(DefaultAbsoluteExpiration)
            .SetSlidingExpiration(DefaultSlidingExpiration);
    }
}
