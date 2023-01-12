namespace Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;

public class GetExperienceTypesResult 
{
    public IEnumerable<ExperienceType> ExperienceTypes {get; set;}

    public class ExperienceType 
    {
        public int Id {get; set;}
        public string Name {get; set;}
    }
}