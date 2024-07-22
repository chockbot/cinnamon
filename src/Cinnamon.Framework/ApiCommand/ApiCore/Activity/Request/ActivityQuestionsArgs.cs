using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;

public class ActivityQuestionsArgs
{
    [Required]
    public int ActivityId {get; set;}
}
