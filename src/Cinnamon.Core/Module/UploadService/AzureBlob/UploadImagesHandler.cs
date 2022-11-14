using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Cinnamon.Core.Common;
using Cinnamon.Core.Module.UploadService.Interactors;
using Cinnamon.Core.Module.UploadService.Interactors.Results;

namespace Cinnamon.Core.Module.UploadService.Handler.AzureBlob;

public class UploadImagesHandler : IUploadImages 
{
    private readonly IConfiguration configuration;

    public UploadImagesHandler(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public AppResult<ImageUploadResult> Execute(ImageUpload args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<ImageUploadResult>.CreateFailed(ex, "An error occured in UploadImagesHandler");
        }
    }

    public async Task<AppResult<ImageUploadResult>> ExecuteAsync(ImageUpload args)
    {
        try
        {
            var connectionString = configuration.GetConnectionString("AzureConnectionString");
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);

            CloudBlobClient blobClient = account.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(args.Container);

            var tasks = args.Images.Select(i => UploadImages(container,i.Item1,i.Item2));

            var tasksResult = await Task.WhenAll(tasks);

            return AppResult<ImageUploadResult>.CreateSucceeded(new ImageUploadResult {ImagesPath = tasksResult.ToList()}, "Images successfull uploaded");
        }
        catch (Exception ex)
        {
            return AppResult<ImageUploadResult>.CreateFailed(ex, "An error occured in UploadImagesHandler");
        }
    }

    private async Task<string> UploadImages(CloudBlobContainer container, byte[] data, string name)
    {
        CloudBlockBlob blockBlob = container.GetBlockBlobReference(name);
        using (var ms = new MemoryStream(data, false))
        {
            await blockBlob.UploadFromStreamAsync(ms);
        }
        return blockBlob.Uri.ToString();
    }
}