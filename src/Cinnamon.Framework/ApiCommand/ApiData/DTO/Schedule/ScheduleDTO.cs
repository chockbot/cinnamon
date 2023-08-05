namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.Schedule
{
    public class ScheduleDTO
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public string Name { get; set; }
        public string DateTime { get; set; }
        public decimal Price { get; set; }
        public string UnitPrice { get; set; } = "PHP";
        public int PerUnit1 { get; set; } = 1;
        public string PriceUnit1 { get; set; } = "Head";
        public int PerUnit2 { get; set; } = 1;
        public string PriceUnit2 { get; set; } = "Session";
        public int Order {get; set;}
        public bool IsActiveSchedule { get; set; }
        public bool IsSetSession { get; set; } = false;
        public string SessionName { get; set; }
        public int HasExpiration { get; set; } = 0;
        public DateTime? StartDate { get; set; }
        public Enums.Enums.ScheduleType ScheduleType { get; set; }
        public Enums.Enums.PriceType PriceType { get; set; }
        public IList<ActivityScheduleTimeDTO> ActivityScheduleTimes { get; set; } = new List<ActivityScheduleTimeDTO>();
    }

    public class ActivityScheduleTimeDTO
    {
        public int ActivityScheduleTimeId { get; set; }
        public int ActivityScheduleId { get; set; }
        public int DayOfWeek { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool IsAvailable { get; set; }
        public Enums.Enums.ModelStatus ModelStatus { get; set; }
    }
}
