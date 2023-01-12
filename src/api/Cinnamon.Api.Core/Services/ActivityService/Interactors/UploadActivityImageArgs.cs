using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;

public class UploadActivityImageArgs : IInteractor
{
    public IFormFile? Image1 {get; set;}
    public IFormFile? Image2 {get; set;}
    public IFormFile? Image3 {get; set;}
    public int ActivityId {get; set;}
}