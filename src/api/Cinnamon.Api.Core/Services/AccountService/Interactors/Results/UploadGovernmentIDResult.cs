namespace Cinnamon.Api.Core.Services.AccountService.Interactors.Results;

public class UploadGovernmentIDResult 
{
    public UploadedFile FrontImage {get; set;}
    public UploadedFile BackImage {get; set;}

    public class UploadedFile 
    {
        public string UploadedPath {get; set;}
        public string FileName {get; set;}
    }
}