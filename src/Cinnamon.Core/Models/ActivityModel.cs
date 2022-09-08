namespace Cinnamon.Core
{
    public class ActivityModel : BaseModel
    {
        public int Id { get; set; }
        public int ActivityTypeId { get; set; }
        public int ExperienceTypeId { get; set; }
        public string Title { get; set; } = "";
        public string Location { get; set; } = "";
        public string Price { get; set; } = "";
        public string Subtitle { get; set; } = "";
        public string Description { get; set; } = "";
        public string Schedules { get; set; } = "";
        public string MapDetails { get; set; } = "";
        public string Guarantee { get; set; } = "";
        public string Remarks { get; set; } = "";

        public virtual ActivityTypeModel? ActivityType { get; set; }
        public virtual ExperienceTypeModel? ExperienceType { get; set; }
    }
}
