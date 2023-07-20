namespace Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;

public class RecommendedActivityResult 
{
    public IEnumerable<Activity> RecommendedActivities { get; set; }

    public class Activity 
    {
        public int Id { get; set; }
        public string Handler { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public IEnumerable<ActivityImage> Images { get; set; }

        public class ActivityImage
        {
            public int Id { get; set; }
            public int Order {get; set;}
            public string ImageSrc { get; set; }
            public string Name { get; set; }
        }
    }
}