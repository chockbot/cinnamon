using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;
using Cinnamon.Api.Core.Modules.UploadDriver.Handlers;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Modules.UploadDriver.Interactors;
using System.Security.Claims;

namespace Cinnamon.Api.Core.Services.AccountService;

public class SubmitUploadGovernmentHandler : IUploadGovernmentIdHandler
{
    private readonly IDeleteAzureBlob deleteAzureBlob;
    private readonly IUploadAzureBlob uploadAzureBlob;
    private readonly IGetGovernmentIdsHandler governmentIdsHandler;
    private readonly ICustomerData customerData;
    private readonly IHttpContextAccessor httpContext;

    public SubmitUploadGovernmentHandler(IDeleteAzureBlob deleteAzureBlob, IUploadAzureBlob uploadAzureBlob,
        IGetGovernmentIdsHandler governmentIdsHandler, ICustomerData customerData, IHttpContextAccessor httpContext)
    {
        this.deleteAzureBlob = deleteAzureBlob;
        this.uploadAzureBlob = uploadAzureBlob;
        this.governmentIdsHandler = governmentIdsHandler;
        this.customerData = customerData;
        this.httpContext = httpContext;
    }


    public AppResult<UploadGovernmentIDResult> Execute(UploadGovernmentIDArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<UploadGovernmentIDResult>.CreateFailed(ex, "An error occured in SubmitUploadGovernmentHandler");
        }
    }

    public async Task<AppResult<UploadGovernmentIDResult>> ExecuteAsync(UploadGovernmentIDArgs args)
    {
        try
        {
            // limit to 5mb per image file
            const int maxFilSize = 5000000;

            // get profile details
            var governmentIds = await governmentIdsHandler.ExecuteAsync(new GetGovernmentIdsArgs {});
            if(!governmentIds.Succeeded || governmentIds.Result == null)
            {
                return AppResult<UploadGovernmentIDResult>.CreateFailed(new ApplicationException(governmentIds.Message), governmentIds.Message);
            }
            var ids = governmentIds.Result;

            if(string.IsNullOrEmpty(ids.FrontImageSrc) && args.FrontImage == null)
            {
                return AppResult<UploadGovernmentIDResult>.CreateFailed(new ApplicationException("Invalid Request. Front image required."),"Invalid Request. Front image required.");
            }

            if(string.IsNullOrEmpty(ids.BackImageSrc) && args.BackImage == null)
            {
                return AppResult<UploadGovernmentIDResult>.CreateFailed(new ApplicationException("Invalid Request. Back image required."),"Invalid Request. Back image required.");
            }

            if(args.FrontImage?.Length > maxFilSize || args.BackImage?.Length > maxFilSize)
            {
                return AppResult<UploadGovernmentIDResult>.CreateFailed(new ApplicationException("Can only upload less than 5mb per file"), "Can only upload less than 5mb per file");
            }

            var isFronValid = args.FrontImage != null ? IsValidType(args.FrontImage.ContentType) : AppResult<string>.CreateSucceeded(string.Empty, string.Empty);
            var isBackValid = args.BackImage != null ? IsValidType(args.BackImage.ContentType) : AppResult<string>.CreateSucceeded(string.Empty, string.Empty);

            if(!isFronValid.Succeeded || !isBackValid.Succeeded)
            {
                return AppResult<UploadGovernmentIDResult>.CreateFailed(
                    new ApplicationException("Invalid image file formats. Can only accept png and jpg"), "Invalid image file formats. Can only accept png and jpg");
            }

            // get customer id saved in claims
            var customerId = httpContext.HttpContext?.User.FindFirstValue("UserId");
            if(customerId == null)
            {
                return AppResult<UploadGovernmentIDResult>.CreateFailed(
                    new ApplicationException("Unable to determine current account login"), "Unable to determine current account login");
            }
            int id = Convert.ToInt32(customerId);

            // create unique name
            var frontUniqueName = args.FrontImage != null ? $"{Guid.NewGuid().ToString()}-front-id.{isFronValid.Result}" : string.Empty;
            var backUniqueName = args.BackImage != null ? $"{Guid.NewGuid().ToString()}-back-id.{isBackValid.Result}" : string.Empty;

            var images = new List<AzureUploadArgs.Image>();

            if(args.FrontImage != null)
            {
                images.Add(new AzureUploadArgs.Image {File = args.FrontImage, ImageName = frontUniqueName});
            }
            if(args.BackImage != null)
            {
                images.Add(new AzureUploadArgs.Image {File = args.BackImage, ImageName = backUniqueName});
            }

            var uploadResult = await uploadAzureBlob.ExecuteAsync(new AzureUploadArgs {
                Container = "upload-container",
                Images = images
            });

            if(!uploadResult.Succeeded || uploadResult.Result == null)
            {
                return AppResult<UploadGovernmentIDResult>.CreateFailed(
                    new ApplicationException("An error occured when uploading images"), "An error occured when uploading images");
            }

            // if have existing front and back ids then delete in azure blob
            // to prevent multiple uplaod causing to blob azure storage full
            var imageNames = new List<string>();
            if(!string.IsNullOrEmpty(ids.BackImageSrc) && args.BackImage != null)
            {
                imageNames.Add(ids.BackImageSrc);
            }
            if(!string.IsNullOrEmpty(ids.FrontImageSrc) && args.FrontImage != null)
            {
                imageNames.Add(ids.FrontImageSrc);
            }

            var deleteAzureBlobRes = await deleteAzureBlob.ExecuteAsync(new AzureDeleteFilesArgs {
                Container = "upload-container",
                FileNames = imageNames
            });

            if(!deleteAzureBlobRes.Succeeded || deleteAzureBlobRes.Result == null)
            {
                return AppResult<UploadGovernmentIDResult>.CreateFailed(
                    new ApplicationException("An error occured when deleting file in azure blob"), "An error occured when deleting file in azure blob");
            }

            var paths = uploadResult.Result.FilePaths.ToList();
            var uploadedFrontImageSrc = args.FrontImage != null ? paths[0].FileSrc : ids.FrontImageSrc;
            var uploadedBackImageSrc = args.BackImage != null ? (paths.Count > 1 ? paths[1].FileSrc : paths[0].FileSrc) : ids.BackImageSrc;

            // update image government file names
            var updateIds = await customerData.UpdateCustomer(new Framework.ApiCommand.ApiData.Customer.Request.UpdateCustomerArgs {
                FrontIdImagePath = uploadedFrontImageSrc,
                BackIdImagePath = uploadedBackImageSrc,
                CustomerId = id,
                // IsVerified = 1
            });;

            if(!updateIds.Succeeded || updateIds.Result == null)
            {
                return AppResult<UploadGovernmentIDResult>.CreateFailed(new ApplicationException(updateIds.Message), updateIds.Message);
            }

            if(updateIds.Succeeded && !updateIds.Result.IsSuccess)
            {
                return AppResult<UploadGovernmentIDResult>.CreateFailed(
                    new ApplicationException(updateIds.Result.ErrorInfo?.Message), "An error occured in SubmitUploadGovernmentHandler");
            }

            return AppResult<UploadGovernmentIDResult>.CreateSucceeded(new UploadGovernmentIDResult {
                BackImage = new UploadGovernmentIDResult.UploadedFile {
                    FileName = backUniqueName,
                    UploadedPath = uploadedBackImageSrc
                },
                FrontImage = new UploadGovernmentIDResult.UploadedFile {
                    FileName = frontUniqueName,
                    UploadedPath = uploadedBackImageSrc
                }
            }, "Successfully upload government ids");
        }
        catch (Exception ex)
        {
            return AppResult<UploadGovernmentIDResult>.CreateFailed(ex, "An error occured in SubmitUploadGovernmentHandler");
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