using Cinnamon.Web.Models.Entities;
namespace Cinnamon.Web.Models;

public class ExploreModel
{
    public Activity Activity { get; set; } = new();
    public IEnumerable<Activity> Activities { get; set; }   
    public IEnumerable<ExperienceType> ExperienceTypes { get; set; }
    public IEnumerable<ExperienceCategory> ExperienceCategories { get; set; }
    public IEnumerable<SubCategory> SubCategories { get; set; }
    public IEnumerable<ActivityImage> ActivityImages { get; set; } 
    public IEnumerable<Address> Address { get; set; }
    public string Token { get; set; } = string.Empty;
    public bool HasError { get; set; }
}
