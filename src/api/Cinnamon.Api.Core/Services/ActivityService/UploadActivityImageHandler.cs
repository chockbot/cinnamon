using Cinnamon.Api.Core.Modules.UploadDriver.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class UploadActivityImageHandler : IUploadActivityImageHandler
{
    private readonly IDeleteAzureBlob deleteAzureBlob;
    private readonly IUploadAzureBlob uploadAzureBlob;
    private readonly IGetOwnedActivityHandler getOwnedActivityHandler;

    public UploadActivityImageHandler(IDeleteAzureBlob deleteAzureBlob, IUploadAzureBlob uploadAzureBlob,
        IGetOwnedActivityHandler getOwnedActivityHandler)
    {
        this.deleteAzureBlob = deleteAzureBlob;
        this.uploadAzureBlob = uploadAzureBlob;
        this.getOwnedActivityHandler = getOwnedActivityHandler;
    }

    public AppResult<UploadActivityImageResult> Execute(UploadActivityImageArgs args)
    {
        throw new NotImplementedException();
    }

    public async Task<AppResult<UploadActivityImageResult>> ExecuteAsync(UploadActivityImageArgs args)
    {
        try
        {
            // limit to 10mb for all image files
            const int maxFilSize = 10000000;
            var totalImageSize = args.Image1.Length + args.Image2.Length + args.Image3.Length;

            if(totalImageSize > maxFilSize)
            {
                return AppResult<UploadActivityImageResult>.CreateFailed(new ApplicationException("Can only upload 10mb for all images"), "Can only upload 10mb for all images");
            }

            // check activity
            var activity = await getOwnedActivityHandler.ExecuteAsync(new GetOwnedActivityArgs {ActivityId = args.ActivityId, IncludeActivityImages = true});
            if(!activity.Succeeded || activity.Result == null)
            {
                return AppResult<UploadActivityImageResult>.CreateFailed(new ApplicationException(activity.Message), activity.Message);
            }
            
            var images = activity.Result.Images;

            return null;
        }
        catch (Exception ex)
        {
            return AppResult<UploadActivityImageResult>.CreateFailed(ex, "An error occured in UploadActivityImageHandler");
        }
    }
}