namespace Cinnamon.Api.Core.Services.AccountService.Interactors.Results;

public class UploadProfilePictureResult
{
    public UploadedFile ProfileImage { get; set; }  
    public class UploadedFile
    {
        public string UploadedPath { get; set; }
        public string FileName { get; set; }
    }
}
