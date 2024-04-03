using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Announcement.Request;

public class UpdateAnnouncementArgs 
{

    [Required]
    public int Id {get; set;}

    [Required]
    public int AdminId {get; set;}

    public string? Title {get; set;}

    public string? Description {get; set;}

    public string? ButtonLabel {get; set;}

    public string? Link {get; set;}

    public string? Status {get; set;} 
}