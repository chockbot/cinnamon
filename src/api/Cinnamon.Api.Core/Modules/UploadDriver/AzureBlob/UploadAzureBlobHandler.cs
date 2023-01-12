using Cinnamon.Api.Core.Modules.UploadDriver.Handlers;
using Cinnamon.Api.Core.Modules.UploadDriver.Interactors;
using Cinnamon.Api.Core.Modules.UploadDriver.Interactors.Results;
using Cinnamon.Framework.Common;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Cinnamon.Api.Core.Modules.UploadDriver.AzureBlob;

public class UploadAzureBlobHandler : IUploadAzureBlob
{
    private readonly IConfiguration configuration;

    public UploadAzureBlobHandler(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public AppResult<AzureUploadResult> Execute(AzureUploadArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<AzureUploadResult>.CreateFailed(ex, "An error occured in UploadAzureBlobHandler");
        }
    }

    public async Task<AppResult<AzureUploadResult>> ExecuteAsync(AzureUploadArgs args)
    {
        try
        {
            var connectionString = configuration.GetConnectionString("AzureConnectionString");
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);

            CloudBlobClient blobClient = account.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(args.Container);

            var tasks = args.Images.Select(i => UploadImages(container,i.File,i.ImageName));

            var tasksResult = await Task.WhenAll(tasks);

            return AppResult<AzureUploadResult>.CreateSucceeded(new AzureUploadResult 
                {FilePaths = tasksResult.Select(s => {
                    return new AzureUploadResult.Sources {
                        FileName = s.Item1,
                        FileSrc = s.Item2
                    };
                })}, "Files successfull uploaded");
        }
        catch (Exception ex)
        {
            return AppResult<AzureUploadResult>.CreateFailed(ex, "An error occured in UploadAzureBlobHandler");
        }
    }

    private async Task<Tuple<string,string>> UploadImages(CloudBlobContainer container, IFormFile file, string name)
    {
        CloudBlockBlob blockBlob = container.GetBlockBlobReference(name);
        await blockBlob.UploadFromStreamAsync(file.OpenReadStream());

        return Tuple.Create(name, blockBlob.Uri.ToString());
    }
}