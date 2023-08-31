using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Web.Models.Forms;

public class UploadActivityImages 
{
    [Required]
    public int ActivityId {get; set;}
    [Required]
    public IList<IFormFile> Images {get; set;}
}