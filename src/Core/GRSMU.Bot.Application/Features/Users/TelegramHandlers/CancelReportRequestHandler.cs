﻿using GRSMU.Bot.Common.Telegram.Brokers.Contexts;
using GRSMU.Bot.Common.Telegram.Extensions;
using GRSMU.Bot.Data.Common.Contracts;
using Telegram.Bot;
using GRSMU.Bot.Common.Telegram.Brokers.Handlers;
using GRSMU.Bot.Common.Telegram.Immutable;
using GRSMU.Bot.Common.Telegram.Services;
using GRSMU.Bot.Domain.Reports.TelegramRequests;

namespace GRSMU.Bot.Application.Features.Users.TelegramHandlers;

public class CancelReportRequestHandler : SimpleTelegramRequestHandlerBase<CancelReportRequestMessage>
{
    private readonly ITelegramUserService _userService;
    private readonly IRequestCacheRepository _requestCacheRepository;

    public CancelReportRequestHandler(
        ITelegramBotClient client, 
        ITelegramUserService userService, 
        IRequestCacheRepository requestCacheRepository,
        ITelegramRequestContext context) : base(client, context)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _requestCacheRepository = requestCacheRepository ?? throw new ArgumentNullException(nameof(requestCacheRepository));
    }

    protected override async Task ExecuteAsync(CancelReportRequestMessage request, CancellationToken cancellationToken)
    {
        var user = Context.User;

        await Client.DeleteMessageAsync(user.ChatId, user.LastBotMessageId.Value, cancellationToken);

        user.LastBotMessageId = null;
        await _userService.UpdateUserAsync(user);

        await _requestCacheRepository.Pop(user.TelegramId);

        await Client.SendTextMessageWithMarkup(user, "Команда отменена", Markups.DefaultMarkup);
    }
}