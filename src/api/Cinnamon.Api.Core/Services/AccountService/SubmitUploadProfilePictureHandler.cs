using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Modules.UploadDriver.Handlers;
using Cinnamon.Api.Core.Modules.UploadDriver.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;
using System.Security.Claims;

namespace Cinnamon.Api.Core.Services.AccountService;
public class SubmitUploadProfilePictureHandler: IUploadProfilePictureHandler
{
    private readonly IDeleteAzureBlob deleteAzureBlob;
    private readonly IUploadAzureBlob uploadAzureBlob;
    private readonly IGetProfilePictureHandler profilePictureHandler;
    private readonly ICustomerData customerData;
    private readonly IHttpContextAccessor httpContext;
    public SubmitUploadProfilePictureHandler(IDeleteAzureBlob deleteAzureBlob, IUploadAzureBlob uploadAzureBlob, ICustomerData customerData,IGetProfilePictureHandler profilePictureHandler, IHttpContextAccessor httpContext)
    {
        this.deleteAzureBlob = deleteAzureBlob;
        this.uploadAzureBlob = uploadAzureBlob;
        this.profilePictureHandler = profilePictureHandler;
        this.customerData = customerData;
        this.httpContext = httpContext; 
    }

    public AppResult<UploadProfilePictureResult> Execute(UploadProfilePictureArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<UploadProfilePictureResult>.CreateFailed(ex, "An error occured in SubmitUploadGovernmentHandler");
        }
    }

    public async Task<AppResult<UploadProfilePictureResult>> ExecuteAsync(UploadProfilePictureArgs args)
    {
        try
        {
            // limit to 5mb per image file
            const int maxFilSize = 5000000;

            if (args.ProfileImage.Length > maxFilSize)
            {
                return AppResult<UploadProfilePictureResult>.CreateFailed(new ApplicationException("Can only upload less than 5mb per file"), "Can only upload less than 5mb per file");
            }

            var isProfileValid = IsValidType(args.ProfileImage.ContentType);
            if (!isProfileValid.Succeeded)
            {
                return AppResult<UploadProfilePictureResult>.CreateFailed(
                    new ApplicationException("Invalid image file formats. Can only accept png and jpg"), "Invalid image file formats. Can only accept png and jpg");
            }
            var profilePicture = await profilePictureHandler.ExecuteAsync(new GetProfilePictureArgs { });
            if (!profilePicture.Succeeded || profilePicture.Result == null)
            {
                return AppResult<UploadProfilePictureResult>.CreateFailed(new ApplicationException(profilePicture.Message), profilePicture.Message);
            }
            // get customer id saved in claims
            var customerId = httpContext.HttpContext?.User.FindFirstValue("UserId");
            if (customerId == null)
            {
                return AppResult<UploadProfilePictureResult>.CreateFailed(
                    new ApplicationException("Unable to determine current account login"), "Unable to determine current account login");
            }
            int id = Convert.ToInt32(customerId);

            // create unique name
            var profileUniqueName = $"{Guid.NewGuid().ToString()}-front-id.{isProfileValid.Result}";
            // get image data
            var images = new List<AzureUploadArgs.Image> {
                new AzureUploadArgs.Image {File = args.ProfileImage, ImageName = profileUniqueName},
            };
            //upload to azure blob
            var uploadResult = await uploadAzureBlob.ExecuteAsync(new AzureUploadArgs
            {
                Container = "upload-profile-picture",
                Images = images
            });

            if (!uploadResult.Succeeded || uploadResult.Result == null)
            {
                return AppResult<UploadProfilePictureResult>.CreateFailed(
                    new ApplicationException("An error occured when uploading images"), "An error occured when uploading images");
            }
            // if have existing front and back ids then delete in azure blob
            // to prevent multiple uplaod causing to blob azure storage full
            var imageNames = new List<string>();
            if (!string.IsNullOrEmpty(profilePicture.Result.ProfileImagseSrc))
            {
                imageNames.Add(profilePicture.Result.ProfileImagseSrc);
            }
            var deleteAzureBlobRes = await deleteAzureBlob.ExecuteAsync(new AzureDeleteFilesArgs
            {
                Container = "upload-profile-picture",
                FileNames = imageNames
            });
            if (!deleteAzureBlobRes.Succeeded || deleteAzureBlobRes.Result == null)
            {
                return AppResult<UploadProfilePictureResult>.CreateFailed(
                    new ApplicationException("An error occured when deleting file in azure blob"), "An error occured when deleting file in azure blob");
            }

            //update profile image file name
            var path = uploadResult.Result.FilePaths.ToList();
            var updateProfile = await customerData.UpdateCustomer(new Framework.ApiCommand.ApiData.Customer.Request.UpdateCustomerArgs
            {
                ProfilePath = path[0],
                CustomerId = id
            });
            if (!updateProfile.Succeeded || updateProfile.Result == null)
            {
                return AppResult<UploadProfilePictureResult>.CreateFailed(new ApplicationException(updateProfile.Message), updateProfile.Message);
            }
            if (updateProfile.Succeeded && !updateProfile.Result.IsSuccess)
            {
                return AppResult<UploadProfilePictureResult>.CreateFailed(
                    new ApplicationException(updateProfile.Result.ErrorInfo?.Message), "An error occured in SubmitUploadProfilePictureHandler");
            }

            return AppResult<UploadProfilePictureResult>.CreateSucceeded(new UploadProfilePictureResult
            {
                ProfileImage = new UploadProfilePictureResult.UploadedFile
                {
                    FileName = profileUniqueName,
                    UploadedPath = path[0]
                }
            }, "Successfully upload government ids");
        }
        catch (Exception ex)
        {
            return AppResult<UploadProfilePictureResult>.CreateFailed(ex, "An error occured in SubmitUploadProfilePictureHandler");
        }
    }
    private AppResult<string> IsValidType(string contentType)
    {
        try
        {
            // accepted file types
            var ext = new string[] { "jpg", "jpeg", "jpe", "png" };

            var extenstion = contentType.Split("/")[1];
            var result = ext.Contains(extenstion);

            if (!result)
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
