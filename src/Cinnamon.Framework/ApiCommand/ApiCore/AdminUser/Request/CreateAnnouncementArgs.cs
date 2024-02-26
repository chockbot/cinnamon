using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.AdminUser.Request;

public class CreateAnnouncementArgs
{
    [Required]
    public string Title {get; set;}

    [Required]
    public string Description {get; set;}

    [Required]
    public string ButtonLabel {get; set;}

    public string? Link {get; set;} = string.Empty;

    [Required]
    public string Status {get; set;} = string.Empty;
}