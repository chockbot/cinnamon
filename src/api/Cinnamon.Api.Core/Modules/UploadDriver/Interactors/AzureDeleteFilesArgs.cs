using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Modules.UploadDriver.Interactors;

public class AzureDeleteFilesArgs : IInteractor 
{

    public IEnumerable<string> FileNames {get; set;}
    public string Container {get; set;}
}