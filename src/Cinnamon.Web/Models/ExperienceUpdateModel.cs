using Cinnamon.Web.Models.Entities;

namespace Cinnamon.Web.Models;

public class ExperienceUpdateModel
{
    public Activity Activity {get; set;} = new();
    public IEnumerable<ExperienceType> ExperienceTypes {get; set;}
    public IEnumerable<ExperienceCategory> ExperienceCategories {get; set;}
    public IEnumerable<SubCategory> SubCategories {get; set;}
    public IEnumerable<string> ActivityLevels { get; set; } = new List<string> { "Beginner", "Intermediate", "Advance" };
    public IEnumerable<string> SkillLevels { get; set; } = new List<string> { "No experience", "Little experience", "Expert" };
    public IEnumerable<string> SessionPeriods { get; set; } = new List<string> { "2 Weeks", "3 Weeks", "1 Month", "2 Months" };
    public IList<int> DeletedScheduleIds {get; set;} = new List<int>();
    public string Token {get; set;} = string.Empty;
    public bool ExperienceHasError {get; set;}
    public bool ExperienceSetupHasError {get; set;}
    public IEnumerable<Region> Regions { get; set; }
    public IEnumerable<City> Cities { get; set; }
    public IEnumerable<Barangay> Barangays { get; set; }
}