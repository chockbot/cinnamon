namespace Cinnamon.Api.Data.Models.ExperienceCategory.Request;

public class GetAllCategoryArgs
{
    public bool? IsActive { get; set; }
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
}