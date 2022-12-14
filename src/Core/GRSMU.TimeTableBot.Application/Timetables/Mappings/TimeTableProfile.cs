using AutoMapper;
using GRSMU.TimeTableBot.Common.Models;
using GRSMU.TimeTableBot.Data.Documents;
using GRSMU.TimeTableBot.Domain.Dtos;

namespace GRSMU.TimeTableBot.Application.Timetables.Mappings
{
    public class TimeTableProfile : Profile
    {
        public TimeTableProfile()
        {
            CreateMap<TimeTableParsedModel, TimeTableDocument>();
            CreateMap<TimeTableLineParsedModel, TimeTableLineDocument>();

            CreateMap<TimeTableParsedModel, TimeTableDto>();
            CreateMap<TimeTableLineParsedModel, TimeTableLineDto>();

            CreateMap<TimeTableDocument, TimeTableDto>();
            CreateMap<TimeTableLineDocument, TimeTableLineDto>();
        }
    }
}
