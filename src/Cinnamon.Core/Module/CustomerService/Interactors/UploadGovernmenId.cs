using Cinnamon.Core.Interactor;

namespace Cinnamon.Core.Module.CustomerService.Interactors;

public class UploadGovernmenId : IInteractor 
{
    public int CustomerId { get; set; }
    public byte[] FrontImageData { get; set; }
    public byte[] BackImageData { get; set; }
    public string FrontFileName { get; set; }
    public string BackFileName { get; set; }
}