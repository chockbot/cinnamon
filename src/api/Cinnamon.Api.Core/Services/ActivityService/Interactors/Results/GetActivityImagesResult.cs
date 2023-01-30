namespace Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;

public class GetActivityImagesResult
{
    public IEnumerable<ActivityImage> ActivityImages { get; set; }
    public class ActivityImage
    {
        public int Id { get; set; }
        public int Order {get; set;}
        public int ActivityId { get; set; }
        public string ImageName { get; set; }
        public string ImageLocation { get; set; }
    }
}
