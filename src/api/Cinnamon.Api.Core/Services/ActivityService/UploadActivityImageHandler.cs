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
            // limit to 10mb for all image files
            const int maxFilSize = 10000000;
            var img1Size = args.Image1 != null ? args.Image1.Length : 0;
            var img2Size = args.Image2 != null ? args.Image2.Length : 0;
            var img3Size = args.Image3 != null ? args.Image3.Length : 0;

            var totalImageSize = img1Size + img2Size + img3Size;

            if(totalImageSize > maxFilSize)
            {
                return AppResult<UploadActivityImageResult>.CreateFailed(
                    new ApplicationException("Can only upload 10mb for all images"), "Can only upload 10mb for all images");
            }

            // check activity
            var activity = await getOwnedActivityHandler.ExecuteAsync(new GetOwnedActivityArgs {ActivityId = args.ActivityId, IncludeActivityImages = true});
            if(!activity.Succeeded || activity.Result == null)
            {
                return AppResult<UploadActivityImageResult>.CreateFailed(new ApplicationException(activity.Message), activity.Message);
            }
            
            var images = activity.Result.Images.OrderBy(i => i.Id).ToList();

            // validate file type and create unique image names
            var image1Valid = args.Image1 != null ? IsValidType(args.Image1.ContentType) : AppResult<string>.CreateSucceeded("png","");
            var image2Valid = args.Image2 != null ? IsValidType(args.Image2.ContentType) : AppResult<string>.CreateSucceeded("png","");
            var image3Valid = args.Image3 != null ? IsValidType(args.Image3.ContentType) : AppResult<string>.CreateSucceeded("png","");

            if(!image1Valid.Succeeded || !image2Valid.Succeeded || !image3Valid.Succeeded)
            {
                return AppResult<UploadActivityImageResult>.CreateFailed(
                    new ApplicationException("Invalid image file formats. Can only accept png and jpg"), "Invalid image file formats. Can only accept png and jpg");
            }

            var listImagesToUpload = new List<AzureUploadArgs.Image>();
            if(args.Image1 != null)
            {
                var image1 = $"{Guid.NewGuid().ToString()}-activity-image.{image1Valid.Result}";
                listImagesToUpload.Add(new AzureUploadArgs.Image {File = args.Image1, ImageName = image1});
            }
            if(args.Image2 != null)
            {
                var image2 = $"{Guid.NewGuid().ToString()}-activity-image.{image2Valid.Result}";
                listImagesToUpload.Add(new AzureUploadArgs.Image {File = args.Image2, ImageName = image2});
            }
            if(args.Image3 != null)
            {
                var image3 = $"{Guid.NewGuid().ToString()}-activity-image.{image3Valid.Result}";
                listImagesToUpload.Add(new AzureUploadArgs.Image {File = args.Image3, ImageName = image3});
            }

            // required 3 images to upload when activity images are empty yet
            if(images.Count == 0 && listImagesToUpload.Count < 3)
            {
                return AppResult<UploadActivityImageResult>.CreateFailed(
                    new ApplicationException("Required 3 images to upload"), "Required 3 images to upload");
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

            // create if dont have images else update it
            if(images.Count == 0)
            {
                var saveImageSrc = await activityImagesData.CreateManyActivityImage(new Framework.ApiCommand.ApiData.ActivityImage.Request.CreateManyActivityImageArgs {
                    Images = createBlob.Result.FilePaths.Select(s => {
                        return new Framework.ApiCommand.ApiData.ActivityImage.Request.CreateManyActivityImageArgs.CreateImage {
                            ActivityId = args.ActivityId,
                            ImageName = s.FileName,
                            ImageSrc = s.FileSrc
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

                return AppResult<UploadActivityImageResult>.CreateSucceeded(new UploadActivityImageResult {
                    Image1Path = savedImage.Count() >=1 ? savedImage.ElementAt(0).ImageLocation : string.Empty,
                    Image2Path = savedImage.Count() >=2 ? savedImage.ElementAt(1).ImageLocation : string.Empty,
                    Image3Path = savedImage.Count() >=3 ? savedImage.ElementAt(2).ImageLocation : string.Empty
                }, "Successfully uploaded activity images");
            }
            else 
            {
                // update the image name and sources
                var previousImgs = new List<string>();
                var paths = createBlob.Result.FilePaths.ToList();

                int runningIndex = 0;
                if(args.Image1 != null && images.Count >= 1 && paths.Count >= runningIndex +1)
                {
                    previousImgs.Add(images[0].Name);
                    images[0].ImageSrc = paths[runningIndex].FileSrc;
                    images[0].Name = paths[runningIndex].FileName;
                    runningIndex++;
                }
                if(args.Image2 != null && images.Count >= 2 && paths.Count >= runningIndex + 1)
                {
                    previousImgs.Add(images[1].Name);
                    images[1].ImageSrc = paths[runningIndex].FileSrc;
                    images[1].Name = paths[runningIndex].FileName;
                    runningIndex++;
                }
                if(args.Image3 != null && images.Count >= 3 && paths.Count >= runningIndex + 1)
                {
                    previousImgs.Add(images[2].Name);
                    images[2].ImageSrc = paths[runningIndex].FileSrc;
                    images[2].Name = paths[runningIndex].FileName;
                }

                var saveUpdated = await activityImagesData.UpdateManyActivityImage(new Framework.ApiCommand.ApiData.ActivityImage.Request.UpdateManyActivityImageArgs {
                    Images = images.Select(i => {
                        return new Framework.ApiCommand.ApiData.ActivityImage.Request.UpdateManyActivityImageArgs.UpdateImage { 
                            ActivityId = args.ActivityId,
                            Id = i.Id,
                            ImageName = i.Name,
                            ImageSrc = i.ImageSrc
                        };
                    })
                });

                if(!saveUpdated.Succeeded || saveUpdated.Result == null)
                {
                    return AppResult<UploadActivityImageResult>.CreateFailed(new ApplicationException(saveUpdated.Message), saveUpdated.Message);
                }

                if(saveUpdated.Succeeded && !saveUpdated.Result.IsSuccess)
                {
                    return AppResult<UploadActivityImageResult>.CreateFailed(
                        new ApplicationException(saveUpdated.Result.ErrorInfo?.Message), "An error occured in UploadActivityImageHandler");
                }

                // delete previous images to azure blob and don't check if successful or not
                var deleteBlob = await deleteAzureBlob.ExecuteAsync(new AzureDeleteFilesArgs {
                    Container = "upload-container",
                    FileNames = previousImgs
                });
                var updatedImages = saveUpdated.Result.Result.ToList();

                return AppResult<UploadActivityImageResult>.CreateSucceeded(new UploadActivityImageResult {
                    Image1Path = updatedImages.Count >= 1 ? updatedImages[0].ImageLocation : string.Empty,
                    Image2Path = updatedImages.Count >= 2 ? updatedImages[1].ImageLocation : string.Empty,
                    Image3Path = updatedImages.Count >=3 ? updatedImages[2].ImageLocation : string.Empty
                }, "Successfully update activity images");
            }
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