namespace Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;

public class GetExperienceCategoriesResult 
{
    public IEnumerable<ExperienceCategory> ExperienceCategories {get; set;}

    public class ExperienceCategory 
    {
        public int Id { get; set; } 
        public string Category { get; set; }
        public string IconPath { get; set; }
    }
}