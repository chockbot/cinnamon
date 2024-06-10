using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.OteDate.Request;


public class GetFirstArgs 
{
    [Required]
    public int ActivityId {get; set;}
}