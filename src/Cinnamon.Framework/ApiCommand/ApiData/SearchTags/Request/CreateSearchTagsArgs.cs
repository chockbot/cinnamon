using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.SearchTags.Request;

public class CreateSearchTagsArgs
{
    [Required]
    public int SearchTagId { get; set; }
    [Required]  
    public int ActivityId { get; set; } 
    public string? SearchTag1 { get; set; }
    public string? SearchTag2 { get; set; }
    public string? SearchTag3 { get; set; }
    public string? SearchTag4 { get; set; }
    public string? SearchTag5 { get; set; }
}
