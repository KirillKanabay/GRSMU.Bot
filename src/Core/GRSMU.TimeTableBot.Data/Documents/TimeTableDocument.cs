using GRSMU.TimeTable.Common.Data.Documents;
using MongoDB.Bson.Serialization.Attributes;

namespace GRSMU.TimeTableBot.Data.Documents
{
    public class TimeTableDocument : DocumentBase
    {
        public string GroupId { get; set; }

        public string Day { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Date { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Week { get; set; }

        public List<TimeTableLineDocument> Lines { get; set; }
    }
}
