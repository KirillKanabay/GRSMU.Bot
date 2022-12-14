﻿using GRSMU.TimeTableBot.Common.Extensions;
using GRSMU.TimeTableBot.Common.Models.Responses;
using GRSMU.TimeTableBot.Common.Services;
using GRSMU.TimeTableBot.Common.Telegram.Extensions;
using GRSMU.TimeTableBot.Common.Telegram.Handlers;
using GRSMU.TimeTableBot.Core.Immutable;
using GRSMU.TimeTableBot.Data.Common.Contracts;
using GRSMU.TimeTableBot.Domain.Reports.Requests;
using Telegram.Bot;

namespace GRSMU.TimeTableBot.Application.Users.TelegramHandlers;

public class CancelReportRequestHandler : TelegramRequestHandlerBase<CancelReportRequestMessage>
{
    private readonly IUserService _userService;
    private readonly IRequestCacheRepository _requestCacheRepository;

    public CancelReportRequestHandler(ITelegramBotClient client, IUserService userService, IRequestCacheRepository requestCacheRepository) : base(client)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _requestCacheRepository = requestCacheRepository ?? throw new ArgumentNullException(nameof(requestCacheRepository));
    }

    protected override async Task<TelegramResponse> ExecuteAsync(CancelReportRequestMessage request, CancellationToken cancellationToken)
    {
        var response = new TelegramResponse(request.UserContext, ResponseStatus.Finished);

        var user = request.UserContext;

        await Client.DeleteMessageAsync(user.ChatId, user.LastBotMessageId.Value, cancellationToken);

        user.LastBotMessageId = null;
        await _userService.UpdateContext(user);

        await _requestCacheRepository.Pop(user.TelegramId);

        await Client.SendTextMessageWithMarkup(user, "Команда отменена", Markups.DefaultMarkup);

        return response;
    }
}