namespace Cinnamon.Framework.ApiCommand.ApiData.Activity.Request;

public class GetActivitiesBySubCategoriesArgs
{
    public int[]? SubCategoryId { get; set; }
    public bool? IsActive { get; set; }
    public bool? IncludeAddress { get; set; }
    public bool? IncludeDescription { get; set; }
    public bool? IncludeSearchTags { get; set; }
    public bool? IncludeSchedules { get; set; }
    public bool? IncludeImages { get; set; }
}
