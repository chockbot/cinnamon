using System.ComponentModel.DataAnnotations;

using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;

public class UpdateActivityImageOrderArgs : IInteractor 
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