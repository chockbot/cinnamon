using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.DynamicContent.Request;

public class UpdateDynamicContentArgs 
{
    [Required]
    public int Id {get; set;}

    public string? Identifier {get; set;}

    public string? Title {get; set;}

    public string? Description {get; set;}

    public string? Content {get; set;}

    public DateTime? DateLastUpdated {get; set;}
}