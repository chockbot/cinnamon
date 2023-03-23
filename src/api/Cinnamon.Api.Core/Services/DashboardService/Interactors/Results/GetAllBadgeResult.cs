namespace Cinnamon.Api.Core.Services.DashboardService.Interactors.Results;
public class GetAllBadgeResult
{
    public IEnumerable<Badge> Badges { get; set; }
    public class Badge
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfStudent { get; set; }
        public int NumberOfCompleted { get; set; }
        public int NumberOfReviews { get; set; }
        public string ImgScr { get; set; }
    }
}
