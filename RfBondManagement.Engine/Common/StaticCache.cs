using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RfBondManagement.Engine.Common
{
    public class StaticCache<T>
    {
        public readonly int Timeout;

        protected readonly Dictionary<string, Tuple<DateTime, T>> _cache;

        public StaticCache(int timeout)
        {
            Timeout = timeout;
            _cache = new Dictionary<string, Tuple<DateTime, T>>();
        }

        public async Task<T> GetOrRetrieve(string key, Func<Task<T>> retrieve)
        {
            Tuple<DateTime, T> result;

            if (_cache.ContainsKey(key))
            {
                result = _cache[key];
                if (result.Item1 < DateTime.Now)
                {
                    return result.Item2;
                }
            }

            var value = await retrieve();

            result = new Tuple<DateTime, T>(DateTime.Now.AddMilliseconds(Timeout), value);

            _cache[key] = result;
            return result.Item2;
        }
    }
}