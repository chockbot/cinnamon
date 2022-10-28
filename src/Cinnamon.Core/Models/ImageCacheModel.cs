using static System.Net.Mime.MediaTypeNames;

namespace Cinnamon.Core
{
    public class ImageCacheModel : BaseModel
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public byte[]? ImageData { get; set; }
        public string blankImageSource => "https://i1.wp.com/www.slntechnologies.com/wp-content/uploads/2017/08/ef3-placeholder-image.jpg";
        public bool isLoading { get; set; } = false;
        public int uploadPercentage { get; set; } = 0;

        public ResultModel? Status { get; set; }

        public string imageSource() {
            if (isLoading) { 
                return "/images/loading_indicator.gif";
            }

            if (IsBlank()) { 
                return blankImageSource; 
            }

            var imagesrc = Convert.ToBase64String(ImageData);
            var imageUrl = string.Format("data:image/jpeg;base64,{0}", imagesrc);

            return imageUrl;
        }

        public bool IsBlank() {
            if (isLoading) {
                return false; 
            }

            if (ImageData == null)
            {
                return true;
            }

            if (Status != null && Status.Type != MessageType.Success)
            {
                return true;
            }

            return false;
        }
    }
}
