using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors;

public class UploadProfilePictureArgs: IInteractor
{
    public IFormFile ProfileImage { get; set; }
}
