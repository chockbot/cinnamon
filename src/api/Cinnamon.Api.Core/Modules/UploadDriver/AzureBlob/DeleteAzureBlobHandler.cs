using Cinnamon.Api.Core.Modules.UploadDriver.Handlers;
using Cinnamon.Api.Core.Modules.UploadDriver.Interactors;
using Cinnamon.Api.Core.Modules.UploadDriver.Interactors.Results;
using Cinnamon.Framework.Common;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Cinnamon.Api.Core.Modules.UploadDriver.AzureBlob;

public class DeleteAzureBlobHandler : IDeleteAzureBlob
{
    private readonly IConfiguration configuration;

    public DeleteAzureBlobHandler(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public AppResult<AzureDeleteFilesResult> Execute(AzureDeleteFilesArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<AzureDeleteFilesResult>.CreateFailed(ex, "An error occured in DeleteAzureBlobHandler");
        }
    }

    public async Task<AppResult<AzureDeleteFilesResult>> ExecuteAsync(AzureDeleteFilesArgs args)
    {
        try
        {
            var connectionString = configuration.GetConnectionString("AzureConnectionString");
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);

            CloudBlobClient blobClient = account.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(args.Container);

            var tasks = args.FileNames.Select(f => DeleteFile(f, container));
            
            var tasksResult = await Task.WhenAll(tasks);

            return AppResult<AzureDeleteFilesResult>.CreateSucceeded(new AzureDeleteFilesResult {Success = true}, "Successfully delete file from azure storage");
        }
        catch (Exception ex)
        {
            return AppResult<AzureDeleteFilesResult>.CreateFailed(ex, "An error occured in DeleteAzureBlobHandler");
        }
    }

    private async Task<bool> DeleteFile(string filename, CloudBlobContainer container)
    {
        var blockBlob = container.GetBlockBlobReference(filename);
        if(blockBlob != null)
        {
            await blockBlob.DeleteIfExistsAsync();
        }

        return true;
    }
}