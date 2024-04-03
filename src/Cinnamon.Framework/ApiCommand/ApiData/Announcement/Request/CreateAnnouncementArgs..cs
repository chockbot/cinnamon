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

    public string? ButtonLabel {get; set;} = string.Empty;

    public string? Link {get; set;} = string.Empty;

    [Required]
    public string Status {get; set;}   
}