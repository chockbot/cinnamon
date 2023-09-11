namespace Cinnamon.Web.Models.Entities
{
    public class ExperienceCreationType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public bool IsActive { get; set; }
        public Cinnamon.Framework.Enums.Enums.ExperienceCreationType CreationType { get; set; }
    }
}
