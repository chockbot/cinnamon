using System.ComponentModel.DataAnnotations;
using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;

public class UploadActivityImageArgs : IInteractor
{
    public IFormFile? Image1 {get; set;}
    public IFormFile? Image2 {get; set;}
    public IFormFile? Image3 {get; set;}
    public IList<ImageOrder>? ImageOrders {get; set;}
    public int ActivityId {get; set;}

    public class ImageOrder 
    {
        [Required]
        public int OldOrder {get; set;}
        [Required]
        public int NewOrder {get; set;}
    }
}