namespace Cinnamon.Api.Data.Repository.Entities
{
    public class ExperienceCreationType : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public bool IsActive { get; set; }
        public virtual IList<Activity> Activities { get; set; }
    }
}
