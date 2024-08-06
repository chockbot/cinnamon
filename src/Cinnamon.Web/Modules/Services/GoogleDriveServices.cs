using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.Drive.v3.Data;
using Cinnamon.Web.Models.Entities;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Serilog.Parsing;


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

    public async Task<Files> ListFilesInFolder(string folderId, string nextPageToken = null, int pageSize = 20)
    {
        var files = new List<Models.Entities.File>();
        var listRequest = _driveService.Files.List();
        listRequest.Q = $"'{folderId}' in parents"; // Search for files in the given folder
        listRequest.Fields = "nextPageToken, files(id, name, mimeType, thumbnailLink)";
        listRequest.PageSize = pageSize;
        listRequest.PageToken = nextPageToken;

        var result = await listRequest.ExecuteAsync();

        var nextToken = result.NextPageToken;
        var fileList = result.Files.Select(file => new Models.Entities.File
        {
            Id = file.Id,
            Name = file.Name,
            Type = file.MimeType,
            ThumbnailLink = file.ThumbnailLink,
            ModifiedDate = file.ModifiedTime // Add modified time property
        }).OrderByDescending(file => file.ModifiedDate).ToList();

        return new Files {FileList = fileList, Token = nextToken };
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
    public async Task<Files> SearchFile(string searchQuery, string nextPageToken = null, int pageSize = 20)
    {
        var listRequest = _driveService.Files.List();
        listRequest.Q = $"'1f06LS6xwK4DrhL1nmeGL9HlUJAyJlrvD' in parents"; // Search for files in the given folder

        if (!string.IsNullOrEmpty(searchQuery))
        {
            // Add search query to filter by file name
            listRequest.Q += $" and name contains '{searchQuery}'";
        }

        listRequest.Fields = "nextPageToken, files(id, name, mimeType, thumbnailLink)";
        listRequest.PageSize = pageSize;
        listRequest.PageToken = nextPageToken;

        var result = await listRequest.ExecuteAsync();

        var nextToken = result.NextPageToken;
        var fileList = result.Files.Select(file => new Models.Entities.File
        {
            Id = file.Id,
            Name = file.Name,
            Type = file.MimeType,
            ThumbnailLink = file.ThumbnailLink
        }).ToList();

        return new Files { FileList = fileList, Token = nextToken };
    }
}

