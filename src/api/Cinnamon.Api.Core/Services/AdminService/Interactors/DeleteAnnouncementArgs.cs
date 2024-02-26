using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AdminService.Interactors;

public class DeleteAnnouncementArgs : IInteractor
{
    public int AnnouncementId {get; set;}
}