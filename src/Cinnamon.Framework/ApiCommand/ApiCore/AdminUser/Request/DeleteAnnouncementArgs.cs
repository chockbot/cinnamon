using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.AdminUser.Request;

public class DeleteAnnouncementArgs
{
    [Required]
    public int Id {get; set;}
}