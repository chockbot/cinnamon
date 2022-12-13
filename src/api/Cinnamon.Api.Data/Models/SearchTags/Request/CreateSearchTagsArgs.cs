using System.ComponentModel.DataAnnotations;
namespace Cinnamon.Api.Data.Models.SearchTags.Request;

public class CreateSearchTagsArgs
{
    [Required]
    public int searchTagId { get; set; }
    [Required]  
    public int activityId { get; set; } 
    public string searchTag1 { get; set; }
    public string searchTag2 { get; set; }
    public string searchTag3 { get; set; }
    public string searchTag4 { get; set; }
    public string searchTag5 { get; set; }
}
