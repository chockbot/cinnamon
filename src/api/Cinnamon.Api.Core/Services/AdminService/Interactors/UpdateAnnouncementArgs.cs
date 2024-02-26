using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AdminService.Interactors;

public class UpdateAnnouncementArgs : IInteractor
{
    public int Id {get; set;}
    public string? Title {get; set;}
    public string? Description {get; set;}
    public string? ButtonLabel {get; set;}
    public string? Link {get; set;}
    public string? Status {get; set;}
}