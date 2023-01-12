namespace Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;

public class GetSubCategoriesResult 
{
    public IEnumerable<SubCategory> SubCategories {get; set;}

    public class SubCategory 
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } 
    }
}