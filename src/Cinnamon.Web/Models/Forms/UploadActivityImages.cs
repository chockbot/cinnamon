using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Web.Models.Forms;

public class UploadActivityImages 
{
    [Required]
    public int ActivityId {get; set;}
    public IList<IFormFile>? Images {get; set;}
    public IList<int>? Orders {get; set;}
    public IList<int>? DeletedIds {get; set;}
}