using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.OteTicket.Request;

public class CreateSharedLinkArgs
{
    [Required]
    public int ActivityId {get; set;}

    [Required]
    public int OteDateId {get; set;}

    [Required]
    public string Guid {get; set;}

    [Required]
    public string Token {get; set;}
}