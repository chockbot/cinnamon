using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Announcement.Request;

public class CreateAnnouncementArgs 
{
    [Required]
    public int AdminId {get; set;}

    [Required]
    public string Title {get; set;}

    [Required]
    public string Description {get; set;}

    [Required]
    public string ButtonLabel {get; set;}

    [Required]
    public string Link {get; set;}

    [Required]
    public string Status {get; set;}   
}