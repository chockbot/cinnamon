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
    public IList<ImageOrder>? ImageOrders {get; set;}

    public class ImageOrder 
    {
        [Required]
        public int OldOrder {get; set;}
        [Required]
        public int NewOrder {get; set;}
    }
}