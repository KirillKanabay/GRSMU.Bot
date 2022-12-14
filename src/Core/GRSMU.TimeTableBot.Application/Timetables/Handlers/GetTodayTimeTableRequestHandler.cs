using AngleSharp.Io;
using AutoMapper;
using GRSMU.TimeTableBot.Common.Broker.Handlers;
using GRSMU.TimeTableBot.Common.Broker.Responses;
using GRSMU.TimeTableBot.Common.Contexts;
using GRSMU.TimeTableBot.Common.Extensions;
using GRSMU.TimeTableBot.Common.Models;
using GRSMU.TimeTableBot.Core.DataLoaders;
using GRSMU.TimeTableBot.Core.Immutable;
using GRSMU.TimeTableBot.Core.Presenters;
using GRSMU.TimeTableBot.Data.Repositories.TimeTables;
using GRSMU.TimeTableBot.Data.Repositories.TimeTables.Filters;
using GRSMU.TimeTableBot.Domain.Dtos;
using GRSMU.TimeTableBot.Domain.RequestMessages.Timetables;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace GRSMU.TimeTableBot.Application.Timetables.Handlers;

public class GetTodayTimeTableRequestHandler : GetTimeTableRequestHandlerBase<GetTodayTimeTableRequestMessage>
{
    public GetTodayTimeTableRequestHandler(
        ITelegramBotClient client, 
        ITimeTableRepository timeTableRepository, 
        TimeTablePresenter timeTablePresenter, 
        IMapper mapper, 
        ITimeTableLoader timeTableLoader) : base(client, timeTableRepository, timeTablePresenter, mapper, timeTableLoader)
    {
    }
    
    protected override TimeTableFilter CreateFilter(UserContext context)
    {
        var today = DateTime.Today;

        if (today.DayOfWeek is DayOfWeek.Saturday)
        {
            today = today.AddDays(2);
        }
        else if (today.DayOfWeek is DayOfWeek.Sunday)
        {
            today = today.AddDays(1);
        }

        var filter = new TimeTableFilter
        {
            GroupId = context.GroupId,
            Date = today
        };

        return filter;
    }

    protected override async Task<List<TimeTableDto>> GetFromLoader(UserContext user, TimeTableFilter filter)
    {
        var grabbedTimeTables = await TimeTableLoader.GrabTimeTableModels(new TimetableQuery
        {
            CourseId = user.CourseId,
            FacultyId = user.FacultyId,
            GroupId = user.GroupId,
            Week = filter.Date?.StartOfWeek().ToString("dd.MM.yyyy 0:00:00") 
                   ?? DateTime.Today.StartOfWeek().ToString("dd.MM.yyyy 0:00:00")
        });

        if (!grabbedTimeTables.Any())
        {
            return new List<TimeTableDto>();
        }

        grabbedTimeTables = grabbedTimeTables.Where(x => x.Date.Date.Equals(filter.Date.Value.Date)).ToList();

        var dtos = Mapper.Map<List<TimeTableDto>>(grabbedTimeTables);

        return dtos;
    }
}