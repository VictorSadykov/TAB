using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAB.Models;

namespace TAB.Services
{
    public class MemoryStorage : IStorage
    {
        private readonly ConcurrentDictionary<long, Session> _sessions = new ConcurrentDictionary<long, Session>();

        public Session GetSession(long chatId)
        {
            if (_sessions.ContainsKey(chatId))
                return _sessions[chatId];

            var newSession = new Session()
            {
                CounterMode = "words"
            };

            _sessions.TryAdd(chatId, newSession);
            return newSession;
        }
    }
}
