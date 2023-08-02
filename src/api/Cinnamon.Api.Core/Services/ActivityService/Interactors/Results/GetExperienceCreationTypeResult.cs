namespace Cinnamon.Api.Core.Services.ActivityService.Interactors.Results
{
    public class GetExperienceCreationTypeResult
    {
        public IEnumerable<ExperienceCreationType> ExperienceCreationTypes { get; set; }

        public class ExperienceCreationType
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string ImagePath { get; set; }
            public bool IsActive { get; set; }
        }
    }
}
