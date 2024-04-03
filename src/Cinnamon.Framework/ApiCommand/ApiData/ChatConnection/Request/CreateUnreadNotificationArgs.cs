using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.ChatConnection.Request;

public class CreateUnreadNotificationArgs
{
    [Required]
    public int ChatHistoryId {get; set;}

    [Required]
    public int FromUserId {get; set;}

    [Required]
    public int ToUserId {get; set;}

    [Required]
    public DateTime ChatDate {get; set;}

    [Required]
    [EmailAddress]
    public string CustomerEmail {get; set;}

    [Required]
    public string Repeated {get; set;}
}
