namespace Cinnamon.Web.Models.Customer;

public class UploadProfilePicture
{
    public string ImageName { get; set; }
    public string ImageFileName { get; set; }
    public string ImageId { get; set; }
    public string ErrorId { get; set; }
    public IFormFile ImageFile { get; set; }
}
