using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace Cinnamon.Web.Modules.Services;

public class GoogleDriveService
{
    private readonly IConfiguration _configuration;
    public readonly DriveService _driveService;

    public GoogleDriveService(IConfiguration configuration)
    {
        // Read Google Drive credentials from appsettings.json
        _configuration = configuration;

        var clientId = _configuration["AppConfig:Authentication:Google:ClientId"];
        var clientSecret = _configuration["AppConfig:Authentication:Google:ClientSecret"];

        // Specify the scopes required for accessing Google Drive
        string[] scopes = { DriveService.Scope.Drive };

        // Initialize Google credentials using OAuth 2.0
        var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            new ClientSecrets
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            },
            scopes, 
            "user",
            CancellationToken.None,
            new FileDataStore("Cinnamon.GoogleDrive.Auth.Store")).Result;

        // Once authorized, retrieve access token and refresh token
        var accessToken = credential.Token.AccessToken;
        var refreshToken = credential.Token.RefreshToken;

        // Initialize the DriveService using the credentials
        var service = new DriveService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "Cinnamon"
        });
    }

    public async Task<List<object>> ListFiles()
    {
        var files = new List<object>();
        // Define parameters for the file list request
        var listRequest = _driveService.Files.List();
        listRequest.Q = "parent in '12XSwbW7YvRNLz39U63L9UZDgiHEpTYO_'";

        listRequest.Fields = "files(id, name, type)";

        // Execute the request asynchronously
        var result = await listRequest.ExecuteAsync();

        // Extract file information from the result
        foreach (var file in result.Files)
        {
            files.Add(new
            {
                Id = file.Id,
                Name = file.Name,
                Type = file.MimeType

            });
        }
        return files;
    }
}

