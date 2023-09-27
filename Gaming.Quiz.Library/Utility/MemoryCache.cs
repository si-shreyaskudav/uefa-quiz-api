using Microsoft.Extensions.Caching.Memory;
using System;

namespace  Gaming.Quiz.Library.Utility
{
    public class MemCache
    {
        private IMemoryCache _Cache;

        #region " Cache Tokens "

        public String _ProfanityToken { get { return "profanity_cache_token"; } }

        #endregion

        public MemCache(IMemoryCache cache)
        {
            _Cache = cache;
        }

        public String CacheTryGet(String token)
        {
            String value = "";

            try
            {
                _Cache.TryGetValue(token, out value);
            }
            catch { }

            if (String.IsNullOrEmpty(value))
                value = "";

            return value;
        }

        public void CacheTrySet(String token, String data, TimeSpan time)
        {
            try
            {
                if (!String.IsNullOrEmpty(data))
                    _Cache.Set(token, data, time);
            }
            catch { }
        }
    }
}
