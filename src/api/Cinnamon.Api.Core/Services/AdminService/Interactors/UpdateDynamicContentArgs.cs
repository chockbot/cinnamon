using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AdminService.Interactors;

public class UpdateDynamicContentArgs : IInteractor
{
    public string Identifier {get; set;}
    public string Title {get; set;}
    public string Description {get; set;}
    public string Content {get; set;}
    public DateTime DateLastUpdated {get; set;}
}