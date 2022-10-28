using System.ComponentModel.DataAnnotations.Schema;

namespace Cinnamon.Core
{
    public class ActivityImagesModels: BaseModel
    {
        public int Id { get; set; }
        [ForeignKey("Activity")]
        public int ImageId { get; set; }
        public string ImageName { get; set; } = "";
        public string ImageLocation { get; set; } = "";
        public virtual ActivityModel Activity { get; set; }
    }
}
