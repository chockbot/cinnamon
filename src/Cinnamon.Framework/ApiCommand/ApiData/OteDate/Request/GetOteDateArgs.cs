using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.OteDate.Request;


public class GetOteDateArgs 
{
    [Required]
    public int ActivityId {get; set;}

    public string? From {get; set;}

    public string? To {get; set;}
}