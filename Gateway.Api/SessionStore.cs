using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Api
{
    public class SessionStore
    {
        private static ConcurrentDictionary<string, Dictionary<string, string>> _store = new ConcurrentDictionary<string, Dictionary<string, string>>();

        public void SaveSession(string sessionId, Dictionary<string, string> claims)
        {
            if (!_store.ContainsKey(sessionId))
            {
                _store[sessionId] = claims;
            }
            else
            {
                _store.TryAdd(sessionId, claims);
            }
        }

        public Dictionary<string, string> GetSesssion(string sessionId)
        {
            _store.TryGetValue(sessionId, out var result);
            return result ?? new Dictionary<string, string>();
        }
    }
}
