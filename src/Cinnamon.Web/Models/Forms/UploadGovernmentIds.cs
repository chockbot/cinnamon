using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Web.Models.Forms;

public class UploadGovernmentIds 
{
    public IFormFile? FrontId {get; set;}
    public IFormFile? BackId {get; set;}
}