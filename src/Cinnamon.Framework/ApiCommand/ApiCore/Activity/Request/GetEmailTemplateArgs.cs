using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;

public class GetEmailTemplateArgs
{
    [Required]
    public int ActivityId {get; set;}

    [Required]
    public string TemplateType {get; set;}
}
