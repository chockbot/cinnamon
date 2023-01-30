using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;

public class UploadActivityImageArgs 
{
    [Required]
    public int ActivityId {get; set;}
    public IFormFile? Image1 {get; set;}
    public IFormFile? Image2 {get; set;}
    public IFormFile? Image3 {get; set;}
}