using GRSMU.TimeTableBot.Common.Broker.Handlers;
using GRSMU.TimeTableBot.Common.Broker.RequestBroker;
using GRSMU.TimeTableBot.Common.Broker.Responses;
using GRSMU.TimeTableBot.Common.Extensions;
using GRSMU.TimeTableBot.Common.Responses;
using GRSMU.TimeTableBot.Domain.RequestMessages.Users.Settings;
using Telegram.Bot;

namespace GRSMU.TimeTableBot.Application.Users.Handlers
{
    public class RunSettingsRequestHandler : RequestHandlerBase<RunSettingsRequestMessage>
    {
        private readonly IRequestBroker _requestBroker;

        public RunSettingsRequestHandler(ITelegramBotClient client, IRequestBroker requestBroker) : base(client)
        {
            _requestBroker = requestBroker ?? throw new ArgumentNullException(nameof(requestBroker));
        }

        protected override async Task<EmptyResponse> ExecuteAsync(RunSettingsRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new EmptyResponse(request.UserContext, ResponseStatus.Finished);

            await Client.RemoveReplyKeyboard(request.UserContext);

            await _requestBroker.Publish(new CourseSettingsRequestMessage(request.UserContext));

            return response;
        }
    }
}
