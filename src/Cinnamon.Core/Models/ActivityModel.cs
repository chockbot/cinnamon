namespace Cinnamon.Core
{
    public class ActivityModel : BaseModel
    {
        public int Id { get; set; }
        // Default Value is InPerson
        public int ActivityTypeId { get; set; } = 1;
        public int ExperienceTypeId { get; set; } = 1;
        public string Title { get; set; } = "";
        public string Location { get; set; } = "";
        public string Price { get; set; } = "";
        public string Subtitle { get; set; } = "";
        public string Description { get; set; } = "";
        public string ScheduleIndicator { get; set; } = "";
        public string Schedules { get; set; } = "";
        public string MapDetails { get; set; } = "";
        public string Guarantee { get; set; } = "";
        public string Remarks { get; set; } = "";

        public virtual ActivityTypeModel? ActivityType { get; set; }
        public virtual ExperienceTypeModel? ExperienceType { get; set; }
    }
}
