using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.DynamicContent.Request;

public class CreateDynamicContentArgs 
{
    [Required]
    public string Identifier {get; set;}

    [Required]
    public string Title {get; set;}

    [Required]
    public string Description {get; set;}

    [Required]
    public string Content {get; set;}

    [Required]
    public DateTime DateLastUpdated {get; set;}
}