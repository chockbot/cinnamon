using System.ComponentModel.DataAnnotations;
using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;

public class UploadActivityImageArgs : IInteractor
{
    public IList<IFormFile> Images {get; set;}
    public int ActivityId {get; set;}
}