namespace Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;

public class GetEnrolledActivitiesResult 
{
    public IEnumerable<Activity> Activities { get; set; }
    
    public class Activity 
    {
        public int Id {get; set;}
        public int ExperienceTypeId {get; set;}
        public int ExperienceCategoryId {get; set;}
        public int SubCategoryId {get; set;}
        public string Title {get; set;}
        public string Description {get; set;}
        public string Price {get; set;}
        public string ScheduleIndicator {get; set;}
        public string Remarks {get; set;}
        public bool IsPublished {get; set;}
        public string Address1 {get; set;}
        public string Address2 {get; set;}
        public string District {get; set;}
        public string City {get; set;}
        public string Subdivision { get; set; }
        public string Region { get; set; }
        public string Barangay { get; set; }
        public string PostalCode { get; set; }
        public string PinnedLocation { get; set; }
        public string SpecificsYouWillProvide {get; set;}
        public string CustomerBringWithThem {get; set;}
        public string? AdditionalRequirements {get; set;}
        public string ActivityLevel {get; set;}
        public string SkillLevel {get; set;}
        public int MinimumAge {get; set;}
        public bool CanAdultsJoin {get; set;}
        public int CreatedBy { get; set; }
        public string MarDetails { get; set; }
        public bool IsSetSession { get; set; }
        public string SessionName { get; set; }
        public int OngoingStudents { get; set; }
        public int CompletedStudents { get; set; }
        public IEnumerable<string> SearchTags {get; set;}
        public IEnumerable<ActivitySchedule> ActivitySchedules {get; set;}
        public IEnumerable<ActivityImage> Images {get; set;}
        public CustomerOwner? Owner {get; set;}
        public OteSchedule? Schedule { get; set; }
    }

    public class ActivitySchedule 
    {
        public int Id {get; set;}
        public string Name {get; set;}
        public string DateTime {get; set;}
        public decimal Price {get; set;}
        public string UnitPrice {get; set;}
        public int PerUnit1 {get; set;}
        public string PriceUnit1 {get; set;}
        public int PerUnit2 {get; set;}
        public string PriceUnit2 {get; set;}
        public int Order {get; set;}
        public bool IsActiveSchedule { get; set; }
        public bool IsSetSession { get; set; }
        public string SessionName { get; set; }
        public int HasExpiration { get; set; }
        public DateTime? StartDate { get; set; }
    }

    public class ActivityImage 
    {
        public int Id {get; set;}
        public int Order {get; set;}
        public string ImageSrc {get; set;}
        public string Name {get; set;}
    }

    public class CustomerOwner 
    {
        public int Id { get; set; }
        public string Handler { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ImageSrc { get; set; }
        public int IsVerified { get; set; }
        public bool IsOG { get; set; }
        public bool IsOfficial { get; set; }
    }

    public class OteSchedule
    {
        public DateTime To { get; set; }
        public DateTime From { get; set; }
    }
}