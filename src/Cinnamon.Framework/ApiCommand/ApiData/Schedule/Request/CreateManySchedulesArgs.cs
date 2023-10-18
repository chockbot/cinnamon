using Cinnamon.Framework.ApiCommand.ApiData.DTO.Schedule;
using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Schedule.Request;

public class CreateManySchedulesArgs
{
    [Required]
    public int ActivityId {get; set;}
    [Required]
    public IEnumerable<Schedule> Schedules {get; set;}

    public class Schedule 
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string DateTime { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string UnitPrice { get; set; } = "PHP";
        public int PerUnit1 { get; set; } = 1;
        public string PriceUnit1 { get; set; } = "Head";
        public int PerUnit2 { get; set; } = 1;
        public string PriceUnit2 { get; set; } = "Session";
        [Required]
        public int Order {get; set;}
        public bool IsActiveSchedule { get; set; }
        public bool IsSetSession { get; set; } = false;
        public string SessionName { get; set; }
        public int HasExpiration { get; set; } = 0;
        public DateTime? StartDate { get; set; }
        public Enums.Enums.ScheduleType ScheduleType { get; set; }
        public Enums.Enums.PriceType PriceType { get; set; }
        public string? SchedulingUrl { get; set; }
        public IEnumerable<ActivityScheduleTime>? ActivityScheduleTimes { get; set; }
    }

    public class ActivityScheduleTime
    {
        public int ActivityScheduleId { get; set; }
        public int DayOfWeek { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool IsEnabled { get; set; }
    }
}