using Cinnamon.Framework.ApiCommand.ApiData.DTO.Schedule;

namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.ActivitySchedule;

public class ActivityScheduleDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string DateTime { get; set; }
    public decimal Price { get; set; }
    public string UnitPrice { get; set; }
    public int PerUnit1 { get; set; }
    public string PriceUnit1 { get; set; }
    public int PerUnit2 { get; set; }
    public string PriceUnit2 { get; set; }
    public int Order {get; set;}
    public bool IsActiveSchedule { get; set; }
    public bool IsSetSession { get; set; } = false;
    public string SessionName { get; set; }
    public int HasExpiration { get; set; } = 0;
    public DateTime? StartDate { get; set; }
    public Enums.Enums.ScheduleType ScheduleType { get; set; }
    public Enums.Enums.PriceType PriceType { get; set; }
    public IList<ActivityScheduleTimeModelDTO> ActivityScheduleTimes { get; set; } = new List<ActivityScheduleTimeModelDTO>();
}

public class ActivityScheduleTimeModelDTO
{
    public int ActivityScheduleTimeId { get; set; }
    public int ActivityScheduleId { get; set; }
    public int DayOfWeek { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public bool IsAvailable { get; set; }
}
