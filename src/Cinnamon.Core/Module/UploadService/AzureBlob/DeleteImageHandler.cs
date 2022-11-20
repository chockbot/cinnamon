using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Cinnamon.Core.Common;
using Cinnamon.Core.Module.UploadService.Interactors;
using Cinnamon.Core.Module.UploadService.Interactors.Results;
using Cinnamon.Core.Module.UploadService.Handler;

public class DeleteImageHandler : IDeleteImage
{
    private readonly IConfiguration configuration;
    public DeleteImageHandler(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
    public AppResult<DeleteImageResult> Execute(DeleteImage args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<DeleteImageResult>.CreateFailed(ex, "An error occured in DeleteImageHandler");
        }
    }
    public async Task<AppResult<DeleteImageResult>> ExecuteAsync(DeleteImage args)
    {
        try
        {
            var connectionString = configuration.GetConnectionString("AzureConnectionString");
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);

            CloudBlobClient blobClient = account.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(args.Container);

            var blockBlob = container.GetBlockBlobReference(args.FileName);

            if (blockBlob !=null)
            {
                await blockBlob.DeleteIfExistsAsync();
            }
            return AppResult<DeleteImageResult>.CreateSucceeded(new DeleteImageResult { Result = "Images successfull uploaded" }, "Images successfull uploaded");
        }
        catch (Exception ex)
        {
            return AppResult<DeleteImageResult>.CreateFailed(ex, "An error occured in UploadImagesHandler");
        }
    }
}
