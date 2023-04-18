using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;

public class UpdateActivityImageOrderArgs 
{
    [Required]
    public int ActivityId {get; set;}
    [Required]
    public IList<ImageOrder> ImageOrders {get; set;}

    public class ImageOrder 
    {
        [Required]
        public int OldOrder {get; set;}
        [Required]
        public int NewOrder {get; set;}
    }
}