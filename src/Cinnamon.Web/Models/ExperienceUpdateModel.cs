using Cinnamon.Core;
using Cinnamon.Core.Models;

namespace Cinnamon.Web.Models;

public class ExperienceUpdateModel
{
    public ActivityModel Activity { get; set; }
    public IList<ExperienceTypeModel> ExperienceTypes {get; set;}
    public IList<ExperienceCategoryModel> ExperienceCategories { get; set; }
    public IList<string> ActivityLevels { get; set; } = new List<string> { "Beginner", "Intermediate", "Advance" };
    public IList<string> SkillLevels { get; set; } = new List<string> { "No experience", "Little experience", "Expert" };
    public ActivityImages Images { get; set; } = new();
    public IList<SearchTag> SearchTags { get; set; }
    public bool HasError { get; set; }

    public class ActivityImage 
    {
        public int Id { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageSrc { get; set; }
        public bool IsLoading { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class ActivityImages 
    {
        public ActivityImage FirstImage { get; set; } = new();
        public ActivityImage SecondImage { get; set; } = new();
        public ActivityImage ThridImage { get; set; } = new();
    }

    public class SearchTag 
    {
        public int Key {get; set;}
        public string Value {get; set;}
    }
}