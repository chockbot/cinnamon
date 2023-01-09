namespace Cinnamon.Api.Core.Modules.UploadDriver.Interactors.Results;

public class AzureUploadResult 
{
    public IEnumerable<Sources> FilePaths {get; set;}

    public class Sources 
    {
        public string FileName {get; set;}
        public string FileSrc {get; set;}
    }
}