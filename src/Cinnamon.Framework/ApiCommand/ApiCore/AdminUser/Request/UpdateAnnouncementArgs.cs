using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.AdminUser.Request;

public class UpdateAnnouncementArgs
{
    [Required]
    public int Id {get; set;}

    public string? Title {get; set;}

    public string? Description {get; set;}

    public string? ButtonLabel {get; set;}

    public string? Link {get; set;} = string.Empty;

    public string? Status {get; set;} = string.Empty;
}