namespace Cinnamon.Api.Data.Repository.Entities;

public class ActivitySummary : BaseEntity
{
    public int ActivityId {get; set;}
    public string ImageBannerSrc {get; set;}
    public int Ongoing {get; set;}
    public int Completed {get; set;}
    public int TotalReviews {get; set;}
    public decimal ReviewAccumulated {get; set;}
    public int TotalParticipants {get; set;}
    public string Location {get; set;}
    public string Provider {get; set;}
}