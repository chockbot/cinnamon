using Cinnamon.Core.Interactor;

namespace Cinnamon.Core.Module.UploadService.Interactors;
public class DeleteImage : IInteractor
{
    public string FileName { get; set; }
    public string Container { get; set; }
}

