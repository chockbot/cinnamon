namespace Cinnamon.Api.Core.Services.AdminService.Interactors.Results;

public class GetAnnouncementsResult
{
    public IEnumerable<Announcement> Announcements {get; set;}
    
    public class Announcement 
    {
        public int Id {get; set;}
        public string Title {get; set;}
        public string Description {get; set;}
        public string ButtonLabel {get; set;}
        public string Link {get; set;}
        public string Status {get; set;}
    }
}