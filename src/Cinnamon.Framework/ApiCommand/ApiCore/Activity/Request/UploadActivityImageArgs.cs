using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;

public class UploadActivityImageArgs 
{
    [Required]
    public int ActivityId {get; set;}
    [Required]
    public IList<IFormFile> Images {get; set;}
    [Required]
    public IList<int> Orders {get; set;}
    public IList<int> DeletedIds {get; set;}
}