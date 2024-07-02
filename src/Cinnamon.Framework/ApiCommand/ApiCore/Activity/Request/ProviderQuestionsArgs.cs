using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;

public class ProviderQuestionsArgs
{
    [Required]
    public int ActivityId {get; set;}
}
