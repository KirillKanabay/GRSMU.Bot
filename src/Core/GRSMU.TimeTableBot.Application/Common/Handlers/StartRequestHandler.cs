using GRSMU.TimeTableBot.Common.Broker.Handlers;
using GRSMU.TimeTableBot.Common.Broker.RequestBroker;
using GRSMU.TimeTableBot.Common.Broker.Responses;
using GRSMU.TimeTableBot.Common.Contexts;
using GRSMU.TimeTableBot.Common.Responses;
using GRSMU.TimeTableBot.Domain.RequestMessages.Common;
using Telegram.Bot;
using GRSMU.TimeTableBot.Common.Extensions;
using GRSMU.TimeTableBot.Core.Immutable;
using GRSMU.TimeTableBot.Domain.RequestMessages.Users;
using GRSMU.TimeTableBot.Domain.RequestMessages.Users.Settings;

namespace GRSMU.TimeTableBot.Application.Common.Handlers
{
    public class StartRequestHandler : RequestHandlerBase<StartRequestMessage>
    {
        private readonly IRequestBroker _requestBroker;

        public StartRequestHandler(ITelegramBotClient client, IRequestBroker requestBroker) : base(client)
        {
            _requestBroker = requestBroker ?? throw new ArgumentNullException(nameof(requestBroker));
        }

        protected override async Task<EmptyResponse> ExecuteAsync(StartRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new EmptyResponse(request.UserContext, ResponseStatus.Finished);

            var user = request.UserContext;

            if (user.RegistrationCompleted)
            {
                await Client.SendTextMessageWithMarkup(request.UserContext, $"Привет, {user.FirstName}!", Markups.DefaultMarkup);
            }
            else
            {
                await Client.SendTextMessage(request.UserContext, $"Привет, {user.FirstName}! Этот бот предназначен для того, чтобы показывать твое актуальное расписание, для этого мне необходимо узнать некоторые данные о тебе:");

                await _requestBroker.Publish(new RunSettingsRequestMessage(user));
            }

            return response;
        }
    }
}
