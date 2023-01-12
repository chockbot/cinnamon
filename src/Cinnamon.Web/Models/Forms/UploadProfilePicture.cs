using System.ComponentModel.DataAnnotations;
namespace Cinnamon.Web.Models.Forms;

public class UploadProfilePicture
{
    [Required]
    public IFormFile ProfilePicture { get; set; }
}
