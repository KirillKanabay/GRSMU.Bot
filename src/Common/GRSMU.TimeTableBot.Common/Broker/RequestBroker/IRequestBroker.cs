using GRSMU.TimeTableBot.Common.Broker.Messages;
using GRSMU.TimeTableBot.Common.Broker.Responses;
using GRSMU.TimeTableBot.Common.Messages;
using GRSMU.TimeTableBot.Common.Responses;

namespace GRSMU.TimeTableBot.Common.Broker.RequestBroker
{
    public interface IRequestBroker
    {
        public Task<TResponse> Publish<TResponse>(IRequestMessage<TResponse> request) 
            where TResponse : ResponseBase;
    }
}
