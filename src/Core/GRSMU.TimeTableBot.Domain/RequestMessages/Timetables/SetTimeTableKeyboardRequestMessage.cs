using GRSMU.TimeTableBot.Common.Contexts;
using GRSMU.TimeTableBot.Common.RequestMessages;

namespace GRSMU.TimeTableBot.Domain.RequestMessages.Timetables;

public class SetTimeTableKeyboardRequestMessage : RequestMessageBase
{
    public SetTimeTableKeyboardRequestMessage(UserContext userContext) : base(userContext)
    {
    }
}