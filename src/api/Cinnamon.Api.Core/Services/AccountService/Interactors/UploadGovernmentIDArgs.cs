using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors;

public class UploadGovernmentIDArgs : IInteractor
{
    public IFormFile? FrontImage {get; set;}
    public IFormFile? BackImage {get; set;}
}