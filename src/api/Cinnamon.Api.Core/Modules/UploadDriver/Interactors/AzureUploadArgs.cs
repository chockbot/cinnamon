using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Modules.UploadDriver.Interactors;

public class AzureUploadArgs : IInteractor 
{

    public IEnumerable<Image> Images {get; set;}
    public string Container {get; set;}

    public class Image 
    {
        public IFormFile File {get; set;}
        public string ImageName {get; set;}
    }
}