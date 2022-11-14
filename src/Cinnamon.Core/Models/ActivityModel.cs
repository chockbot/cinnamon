using Cinnamon.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinnamon.Core
{
    public class ActivityModel : BaseModel
    {
        public int Id { get; set; }
        // Default Value is InPerson
        public int ActivityTypeId { get; set; } = 1;
        public int ExperienceTypeId { get; set; } = 1;
        public int? ExperienceCategoryId { get; set; } = null;
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
        public bool IsPublished { get; set; }

        public virtual ActivityTypeModel? ActivityType { get; set; }
        public virtual ExperienceTypeModel? ExperienceType { get; set; }
        public virtual AddressModel? Address { get; set; } = null;
        public virtual DescriptionSectionModel? DescriptionSectionModel { get; set; }
        public virtual SearchTagsModel? SearchTagsModel { get; set; }
        public virtual IList<ScheduleModel> ScheduleList { get; set; }
        public virtual ICollection<ActivityImagesModels> ActivityImages { get; set; }
    }
}
