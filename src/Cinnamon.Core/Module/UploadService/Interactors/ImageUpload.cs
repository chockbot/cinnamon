using Cinnamon.Core.Interactor;

namespace Cinnamon.Core.Module.UploadService.Interactors;

public class ImageUpload : IInteractor 
{
    public IList<Tuple<byte[],string>> Images { get; set; }
    public string Container { get; set; }
}