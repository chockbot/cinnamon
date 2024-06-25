using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.DynamicContent.Request;

public class CreateEmailTemplateArgs 
{
    [Required]
    public int ActivityId {get; set;}

    [Required]
    public int ProviderId {get; set;}

    [Required]
    public string TemplateType {get; set;}

    public string? Subject {get; set;} = string.Empty;

    [Required]
    public string Body {get; set;}
}