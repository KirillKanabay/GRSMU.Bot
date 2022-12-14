﻿using GRSMU.TimeTableBot.Common.Contexts;
using GRSMU.TimeTableBot.Common.RequestMessages;

namespace GRSMU.TimeTableBot.Domain.Users.TelegramRequests.Settings
{
    public class RunSettingsRequestMessage : TelegramRequestMessageBase
    {
        public RunSettingsRequestMessage(UserContext userContext) : base(userContext)
        {
        }
    }
}