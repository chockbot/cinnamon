using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Web.Models.Forms;

public class UploadGovernmentIds 
{
    [Required]
    public IFormFile FrontId {get; set;}
    [Required]
    public IFormFile BackId {get; set;}
}