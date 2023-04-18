using Cinnamon.Api.Core.Modules.DataAccess.Customer;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Modules.UploadDriver.Handlers;
using Cinnamon.Api.Core.Modules.UploadDriver.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;
using System.Security.Claims;

namespace Cinnamon.Api.Core.Services.AccountService
{
    public class DeleteProfilePictureHandler : IDeleteProfilePictureHandler
    {
        private readonly IDeleteAzureBlob deleteAzureBlob;
        private readonly IGetProfilePictureHandler profilePictureHandler;
        private readonly IHttpContextAccessor httpContext;
        private readonly ICustomerData customerData;

        public DeleteProfilePictureHandler(IDeleteAzureBlob deleteAzureBlob, IGetProfilePictureHandler profilePictureHandler, IHttpContextAccessor httpContext, ICustomerData customerData)
        {
            this.deleteAzureBlob = deleteAzureBlob;
            this.profilePictureHandler = profilePictureHandler;
            this.httpContext = httpContext;
            this.customerData = customerData;
        }
        public AppResult<DeleteProfilePictureResult> Execute(DeleteProfilePictureArgs args)
        {
            try
            {
                return ExecuteAsync(args).Result;
            }
            catch (Exception ex)
            {
                return AppResult<DeleteProfilePictureResult>.CreateFailed(ex, "An error occured in DeleteProfilePictureResult");
            }
        }

        public async Task<AppResult<DeleteProfilePictureResult>> ExecuteAsync(DeleteProfilePictureArgs interactor)
        {
            var customerId = httpContext.HttpContext?.User.FindFirstValue("UserId");
            if (customerId == null)
            {
                return AppResult<DeleteProfilePictureResult>.CreateFailed(
                    new ApplicationException("Unable to determine current account login"), "Unable to determine current account login");
            }
            int id = Convert.ToInt32(customerId);

            var profilePicture = await profilePictureHandler.ExecuteAsync(new GetProfilePictureArgs { });
            if (!profilePicture.Succeeded || profilePicture.Result == null)
            {
                return AppResult<DeleteProfilePictureResult>.CreateFailed(new ApplicationException(profilePicture.Message), profilePicture.Message);
            }

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
                return AppResult<DeleteProfilePictureResult>.CreateFailed(
                    new ApplicationException("An error occured when deleting file in azure blob"), "An error occured when deleting file in azure blob");
            }

            var updateProfile = await customerData.UpdateCustomer(new Framework.ApiCommand.ApiData.Customer.Request.UpdateCustomerArgs
            {
                ProfilePath = "/images/Profile/user.png",
                CustomerId = id
            });
            if (!updateProfile.Succeeded || updateProfile.Result == null)
            {
                return AppResult<DeleteProfilePictureResult>.CreateFailed(new ApplicationException(updateProfile.Message), updateProfile.Message);
            }

            return AppResult<DeleteProfilePictureResult>.CreateSucceeded(new DeleteProfilePictureResult
            {
               
            }, "Successfully deleted profile picture");
        }
    }
}
