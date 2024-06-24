using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;

public class ActivityFeedArgs
{
    [Required]
    public int Take {get; set;}
    
    [Required]
    public int Skip {get; set;}

    public string? Search {get; set;}

    public int? CategoryId {get; set;}

    public int? StarReview {get; set;}

    public int? ExperienceType {get; set;}
    public int? ExperienceCategory {get; set;}
}
