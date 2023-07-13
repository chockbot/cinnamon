using Cinnamon.Web.Models.Entities;

namespace Cinnamon.Web.Models;

public class ExperienceCreationModel 
{
    public Activity Activity {get; set;} = new();
    public IEnumerable<ExperienceType> ExperienceTypes {get; set;}
    public IEnumerable<ExperienceCategory> ExperienceCategories {get; set;}
    public IEnumerable<SubCategory> SubCategories {get; set;}
    public IEnumerable<Region> Regions {get; set; }
    public IEnumerable<string> ActivityLevels { get; set; } = new List<string> { "Beginner", "Intermediate", "Advance" };
    public IEnumerable<string> SkillLevels { get; set; } = new List<string> { "No experience", "Little experience", "Expert" };
    public IEnumerable<string> SessionPeriods { get; set; } = new List<string> { "2 Weeks", "3 Weeks", "1 Month", "2 Months", "3 Months" };
    public string Token {get; set;} = string.Empty;
    public bool HasError {get; set;}

    public long OverAllImageSize {get; set;}
}