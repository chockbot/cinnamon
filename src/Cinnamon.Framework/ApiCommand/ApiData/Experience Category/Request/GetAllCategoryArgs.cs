namespace Cinnamon.Framework.ApiCommand.ApiData.ExperienceCategory.Request;

public class GetAllCategoryArgs
{
    public bool? IsActive { get; set; }
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
}