﻿using GRSMU.Bot.Common.Telegram.Brokers.RequestCache;
using GRSMU.Bot.Common.Telegram.Immutable;
using GRSMU.Bot.Data.Common.Contracts;

namespace GRSMU.Bot.Application
{
    public class RequestCache : IRequestCache
    {
        private readonly IRequestCacheRepository _repository;
        
        private readonly IReadOnlyList<string> _allowedToCached = new List<string>
        {
            CommandKeys.Reports.Report,
            CommandKeys.Gradebook.SetLogin,
            CommandKeys.Gradebook.SetPassword
        };

        public RequestCache(IRequestCacheRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task Push(string telegramId, string command)
        {
            if (!_allowedToCached.Contains(command, StringComparer.InvariantCultureIgnoreCase))
            {
                return;
            }

            await _repository.Push(telegramId, command);
        }

        public Task<string?> Pop(string telegramId) => _repository.Pop(telegramId);
    }
}
