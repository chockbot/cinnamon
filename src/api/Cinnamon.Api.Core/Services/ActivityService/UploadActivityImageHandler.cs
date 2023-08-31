using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Modules.UploadDriver.Handlers;
using Cinnamon.Api.Core.Modules.UploadDriver.Interactors;
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
    private readonly IActivityImagesData activityImagesData;

    public UploadActivityImageHandler(IDeleteAzureBlob deleteAzureBlob, IUploadAzureBlob uploadAzureBlob,
        IGetOwnedActivityHandler getOwnedActivityHandler, IActivityImagesData activityImagesData)
    {
        this.deleteAzureBlob = deleteAzureBlob;
        this.uploadAzureBlob = uploadAzureBlob;
        this.getOwnedActivityHandler = getOwnedActivityHandler;
        this.activityImagesData = activityImagesData;
    }

    public AppResult<UploadActivityImageResult> Execute(UploadActivityImageArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<UploadActivityImageResult>.CreateFailed(ex, "An error occured in UploadActivityImageHandler");
        }
    }

    public async Task<AppResult<UploadActivityImageResult>> ExecuteAsync(UploadActivityImageArgs args)
    {
        try
        {
            // limit to 25mb for all image files
            const int maxFilSize = 25000000;
            var goodImages = args.Images.Where(i => i is not null).ToList();
            var totalImagesSize = goodImages.Sum(i => i.Length);

            if(totalImagesSize > maxFilSize)
            {
                return AppResult<UploadActivityImageResult>.CreateFailed(
                    new ApplicationException("Can only upload 25mb for all images"), "Can only upload 25mb for all images");
            }

            // check activity
            var activity = await getOwnedActivityHandler.ExecuteAsync(new GetOwnedActivityArgs {ActivityId = args.ActivityId, IncludeActivityImages = true});
            if(!activity.Succeeded || activity.Result == null)
            {
                return AppResult<UploadActivityImageResult>.CreateFailed(new ApplicationException("Invalid request."), "Invalid request.");
            }
            
            var images = activity.Result.Images.OrderBy(i => i.Id).ToList();
            var listImagesToUpload = new List<AzureUploadArgs.Image>();

            foreach(var uploadedImage in goodImages)
            {
                var validateImage = IsValidType(uploadedImage.ContentType);
                if(!validateImage.Succeeded || validateImage.Result is null)
                {
                    return AppResult<UploadActivityImageResult>.CreateFailed(
                        new ApplicationException("Invalid image file formats. Can only accept png and jpg"), "Invalid image file formats. Can only accept png and jpg");
                }

                var imageName = $"{Guid.NewGuid().ToString()}-activity-image.{validateImage.Result}";
                listImagesToUpload.Add(new AzureUploadArgs.Image {File = uploadedImage, ImageName = imageName});
            }

            // minimum of 3 images and maximum of 7 images including cover
            if(listImagesToUpload.Count > 7 || listImagesToUpload.Count < 3)
            {
                return AppResult<UploadActivityImageResult>.CreateFailed(
                    new ApplicationException("Minimum of 3 images and maximum of 7 images."), "Minimum of 3 images and maximum of 7 images.");
            }

            // delete activity images
            var previousImages = images.Select(i => i.Name);
            var deleteImagesRes = await activityImagesData.RemoveActivityImages(new Framework.ApiCommand.ApiData.ActivityImage.Request.RemoveActivityImageArgs {
                ActivityId = args.ActivityId
            });
            if(!deleteImagesRes.Succeeded || deleteImagesRes.Result is null || !deleteImagesRes.Result.IsSuccess)
            {
                return AppResult<UploadActivityImageResult>.CreateFailed(new ApplicationException("An error occured when updating images."), "An error occured when updating images.");
            }

            // upload to azure blob
            var createBlob = await uploadAzureBlob.ExecuteAsync(new Modules.UploadDriver.Interactors.AzureUploadArgs {
                Container = "upload-container",
                Images = listImagesToUpload
            });

            if(!createBlob.Succeeded || createBlob.Result == null)
            {
                return AppResult<UploadActivityImageResult>.CreateFailed(
                    new ApplicationException("An error occured when uploading images."), "An error occured when uploading images.");
            }

            var saveImageSrc = await activityImagesData.CreateManyActivityImage(new Framework.ApiCommand.ApiData.ActivityImage.Request.CreateManyActivityImageArgs {
                Images = createBlob.Result.FilePaths.Select((s,index) => {
                    return new Framework.ApiCommand.ApiData.ActivityImage.Request.CreateManyActivityImageArgs.CreateImage {
                        ActivityId = args.ActivityId,
                        ImageName = s.FileName,
                        ImageSrc = s.FileSrc,
                        Order = index
                    };
                })
            });

            if(!saveImageSrc.Succeeded || saveImageSrc.Result == null)
            {
                return AppResult<UploadActivityImageResult>.CreateFailed(new ApplicationException(saveImageSrc.Message), saveImageSrc.Message);
            }

            if(saveImageSrc.Succeeded && !saveImageSrc.Result.IsSuccess)
            {
                return AppResult<UploadActivityImageResult>.CreateFailed(
                    new ApplicationException(saveImageSrc.Result.ErrorInfo?.Message), "An error occured in UploadActivityImageHandler");
            }
            var savedImage = saveImageSrc.Result.Result;

            // delete previous images to azure blob and don't check if successful or not
            var deleteBlob = await deleteAzureBlob.ExecuteAsync(new AzureDeleteFilesArgs {
                Container = "upload-container",
                FileNames = previousImages
            });

            return AppResult<UploadActivityImageResult>.CreateSucceeded(new UploadActivityImageResult {
                UploadedPaths = createBlob.Result.FilePaths.Select(i => i.FileSrc).ToList()
            }, "Successfully uploaded activity images");
        }
        catch (Exception ex)
        {
            return AppResult<UploadActivityImageResult>.CreateFailed(ex, "An error occured in UploadActivityImageHandler");
        }
    }

    private AppResult<string> IsValidType(string contentType)
    {
        try
        {
            // accepted file types
            var ext = new string[] {"jpg", "jpeg", "jpe", "png"};

            var extenstion = contentType.Split("/")[1];
            var result =  ext.Contains(extenstion);

            if(!result)
            {
                return AppResult<string>.CreateFailed(new ApplicationException("Invalid mime type"), "Invalid mime type");
            }

            return AppResult<string>.CreateSucceeded(extenstion, "valid mime type");
        }
        catch (Exception ex)
        {
            return AppResult<string>.CreateFailed(ex, "An error occured when tried to check file type");
        }
    }
}