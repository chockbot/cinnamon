namespace Cinnamon.Core
{
    public class ActivityImagesModels: BaseModel
    {
        public int Id { get; set; }
        public int ImageId { get; set; }
        public string ImageLocation { get; set; } = "";
    }
}
