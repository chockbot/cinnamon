using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.Drive.v3.Data;
using Cinnamon.Web.Models.Entities;


namespace Cinnamon.Web.Modules.Services;

public class GoogleDriveService
{
    private readonly IConfiguration _configuration;
    public readonly DriveService _driveService;

    public GoogleDriveService(IConfiguration configuration)
    {
        // Read Google Drive credentials from appsettings.json
        _configuration = configuration;
        var credentialFilePath = _configuration["AppConfig:Authentication:Google:ServicePath"];

        // Specify the scopes required for accessing Google Drive
        string[] scopes = { DriveService.Scope.Drive };

        // Initialize Google credentials using service account
        var credential = GoogleCredential.FromFile(credentialFilePath)
                                          .CreateScoped(scopes);
        // Initialize the DriveService using the service account credentials
        _driveService = new DriveService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "Cinnamon"
        });
    }

    public async Task<List<Models.Entities.File>> ListFilesInFolder()
    {
        var files = new List<Models.Entities.File>();
        var listRequest = _driveService.Files.List();
        listRequest.Q = $"'1A5iZBZWoF9g3m3F0p8LnmnQcxdANmEWf' in parents"; // Search for files in the given folder
        listRequest.Fields = "files(id, name, mimeType, thumbnailLink)";

        var result = await listRequest.ExecuteAsync();

        foreach (var file in result.Files)
        {
            files.Add(new Models.Entities.File
            {
                Id = file.Id,
                Name = file.Name,
                Type = file.MimeType,
                ThumbnailLink = file.ThumbnailLink
            });
        }

        return files;
    }
    public async Task<byte[]> GetFileContent(string fileId)
    {
        var getRequest = _driveService.Files.Get(fileId);
        var stream = new MemoryStream();
        await getRequest.DownloadAsync(stream);
        return stream.ToArray();
    }
    public async Task<Models.Entities.File> GetFileInfo(string fileId)
    {
        var getRequest = _driveService.Files.Get(fileId);
        getRequest.Fields = "id, name, mimeType, thumbnailLink"; // Specify the fields to retrieve

        var file = await getRequest.ExecuteAsync();
        return new Models.Entities.File
        {
            Id = file.Id,
            Name = file.Name,
            Type = file.MimeType,
            ThumbnailLink = file.ThumbnailLink
        };
    }
    public async Task<List<Models.Entities.File>> SearchFile(string searchQuery)
    {
        var files = new List<Models.Entities.File>();
        var listRequest = _driveService.Files.List();
        listRequest.Q = $"'1A5iZBZWoF9g3m3F0p8LnmnQcxdANmEWf' in parents"; // Search for files in the given folder

        if (!string.IsNullOrEmpty(searchQuery))
        {
            // Add search query to filter by file name
            listRequest.Q += $" and name contains '{searchQuery}'";
        }

        listRequest.Fields = "files(id, name, mimeType, thumbnailLink)";

        var result = await listRequest.ExecuteAsync();

        foreach (var file in result.Files)
        {
            files.Add(new Models.Entities.File
            {
                Id = file.Id,
                Name = file.Name,
                Type = file.MimeType,
                ThumbnailLink = file.ThumbnailLink
            });
        }

        return files;
    }
}

