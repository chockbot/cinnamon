using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Activity.Request;

public class UpdateOteDatePayloadArgs
{
    [Required]
    public int Id {get; set;}

    [Required]
    public string Payload {get; set;}
}