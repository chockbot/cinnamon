using Cinnamon.Core.Common;
using Cinnamon.Core.Module.CustomerService.Interactors;
using Cinnamon.Core.Module.CustomerService.Interactors.Results;
using Cinnamon.Core.Module.UploadService.Handler;

namespace Cinnamon.Core.Module.CustomerService.Handler.Profile;

public class UploadCustomerGovernmentIdHandler : IUploadCustomerGovernmentId
{
    private readonly IUploadImages uploadImagesHandler;

    public UploadCustomerGovernmentIdHandler(IUploadImages uploadImagesHandler)
    {
        this.uploadImagesHandler = uploadImagesHandler;
    }

    public AppResult<UpdloadGovernmentIdResult> Execute(UploadGovernmenId args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<UpdloadGovernmentIdResult>.CreateFailed(ex, "An error occured in UploadCustomerGovernmentIdHandler");
        }
    }

    public async Task<AppResult<UpdloadGovernmentIdResult>> ExecuteAsync(UploadGovernmenId args)
    {
        try
        {
            var customer = await CoreDI.DataStore.Customers.GetCustomerById(args.CustomerId);
            if(customer == null)
            {
                return AppResult<UpdloadGovernmentIdResult>.CreateFailed(new ApplicationException("Can't find customer data"), "Can't find customer data");
            }

            if(args.BackImageData == null)
            {
                return AppResult<UpdloadGovernmentIdResult>.CreateFailed(new ApplicationException("Provide back image data"), "Provide back image data");
            }

            if(string.IsNullOrEmpty(args.BackFileName))
            {
                return AppResult<UpdloadGovernmentIdResult>.CreateFailed(new ApplicationException("Provide back image filename"), "Provide back image filename");
            }

            if(args.FrontImageData == null)
            {
                return AppResult<UpdloadGovernmentIdResult>.CreateFailed(new ApplicationException("Provide front image data"), "Provide front image data");
            }

            if(string.IsNullOrEmpty(args.FrontFileName))
            {
                return AppResult<UpdloadGovernmentIdResult>.CreateFailed(new ApplicationException("Provide front image filename"), "Provide front image filename");
            }

            var uploadResult = await uploadImagesHandler.ExecuteAsync(
                new UploadService.Interactors.ImageUpload
                {
                    Container = "upload-container",
                    Images = new List<Tuple<byte[],string>>
                        {
                            new Tuple<byte[], string>(args.FrontImageData, args.FrontFileName),
                            new Tuple<byte[], string>(args.BackImageData, args.BackFileName)
                        }
                });
            
            if(!uploadResult.Succeeded)
            {
                return AppResult<UpdloadGovernmentIdResult>.CreateFailed(new ApplicationException("An error occured when uploading images"), "An error occured when uploading images");
            }

            // save changes to customer
            if(uploadResult.Result != null)
            {
                customer.FrontIdImagePath = uploadResult.Result.ImagesPath[0];
                customer.BackIdImagePath = uploadResult.Result.ImagesPath[1];
                var updatedCustomer = await CoreDI.DataStore.Customers.SaveDataAsync(customer);

                if(!updatedCustomer.Message.ToLower().Contains("saved"))
                {
                    return AppResult<UpdloadGovernmentIdResult>.CreateFailed(new ApplicationException("An error occured when saving image path"), "An error occured when saving image path");
                }

                return AppResult<UpdloadGovernmentIdResult>.CreateSucceeded(new UpdloadGovernmentIdResult(),"Government ID's successfully submitted");
            }

            return AppResult<UpdloadGovernmentIdResult>.CreateFailed(new ApplicationException("An error occured when uploading images"), "An error occured when uploading images");
        }
        catch (Exception ex)
        {
            return AppResult<UpdloadGovernmentIdResult>.CreateFailed(ex, "An error occured in UploadCustomerGovernmentIdHandler");
        }
    }
}