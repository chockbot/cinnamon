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
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string CityName { get; set; }
        public string Subdivision { get; set; }
        public string Region { get; set; }
        public string RegionName { get; set; }
        public string Barangay { get; set; }
        public int ExperienceTypeId { get; set; }
        public OteSchedule? Schedule { get; set; }
        public IEnumerable<ActivityImage> Images { get; set; }
        public IEnumerable<ActivitySchedule> ActivitySchedules { get; set; }
        public class ActivityImage
        {
            public int Id { get; set; }
            public int Order {get; set;}
            public string ImageSrc { get; set; }
            public string Name { get; set; }
        }
        public class ActivitySchedule
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string DateTime { get; set; }
            public decimal Price { get; set; }
            public string UnitPrice { get; set; }
            public int PerUnit1 { get; set; }
            public string PriceUnit1 { get; set; }
            public int PerUnit2 { get; set; }
            public string PriceUnit2 { get; set; }
            public int Order { get; set; }
            public bool IsActiveSchedule { get; set; }
            public bool IsSetSession { get; set; }
            public string SessionName { get; set; }
            public int HasExpiration { get; set; }
            public DateTime? StartDate { get; set; }
        }
        public class OteSchedule {
            public DateTime From { get; set; }
            public DateTime To { get; set; }
        }
    }
}