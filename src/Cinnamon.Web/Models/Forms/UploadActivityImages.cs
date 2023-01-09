using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Web.Models.Forms;

public class UploadActivityImages 
{
    public IFormFile? Image1 {get; set;}
    public IFormFile? Image2 {get; set;}
    public IFormFile? Image3 {get; set;}
    [Required]
    public int ActivityId {get; set;}
}