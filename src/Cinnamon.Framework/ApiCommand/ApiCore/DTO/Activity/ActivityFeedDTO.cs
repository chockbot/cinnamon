namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.Activity;

public class ActivityFeedDTO 
{
    public int ActivityId {get; set;}
    public string Title {get; set;}
    public string Handler {get; set;}
    public int ExperienceTypeId {get; set;}
    public int ExperienceCreationTypeId {get; set;}
    public string Price {get; set;}
    public bool IsNew {get; set;}
    public DateTime To { get; set; }
    public DateTime From { get; set; }
    public DateTime Date { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public string StartTime { get; set; }
    public Location Address {get; set;}
    public Summary SummaryDetails {get; set;}

    public class Location 
    {
        public string City {get; set;}
        public string Region {get; set;}
        public string PinnedLocation {get; set;}
    }

    public class Summary 
    {
        public string ImageSrc {get; set;}
        public int Ongoing {get; set;}
        public int Completed {get; set;}
        public int TotalReviews {get; set;}
        public decimal ReviewAccumulated {get; set;}
        public int TotalParticipants {get; set;}
    }
}