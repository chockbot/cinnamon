using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Announcement.Request;

public class DeleteAnnouncementArgs 
{
    [Required]
    public int Id {get; set;}
}