using Cinnamon.Core.Common;
using Cinnamon.Core.Module.CustomerService.Interactors;
using Cinnamon.Core.Module.CustomerService.Interactors.Results;

namespace Cinnamon.Core.Module.CustomerService.Handler.Profile;

public class UpdateCustomerLegalNameHandler : IUpdateCustomerLegalName
{
    public AppResult<UpdateLegaNameResult> Execute(UpdateLegalName args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<UpdateLegaNameResult>.CreateFailed(ex, "An error occured in UpdateAboutHandler");
        }
    }

    public async Task<AppResult<UpdateLegaNameResult>> ExecuteAsync(UpdateLegalName args)
    {
        try
        {
            // check firstname and lastname if empty
            if(string.IsNullOrEmpty(args.FirstName))
            {
                return AppResult<UpdateLegaNameResult>.CreateFailed(new ApplicationException("FirstName cannot be empty"), "FirstName cannot be empty");
            }

            if(string.IsNullOrEmpty(args.LastName))
            {
                return AppResult<UpdateLegaNameResult>.CreateFailed(new ApplicationException("LastName cannot be empty"), "LastName cannot be empty");
            }

            // get customer data
            var customer = await CoreDI.DataStore.Customers.GetCustomerById(args.CustomerId);
            if(customer == null)
            {
                return AppResult<UpdateLegaNameResult>.CreateFailed(new ApplicationException("Can't find customer data"), "Can't find customer data");
            }

            customer.FirstName = args.FirstName;
            customer.LastName = args.LastName;
            var res = await CoreDI.DataStore.Customers.SaveDataAsync(customer);
            if(!res.Message.ToLower().Contains("saved"))
            {
                return AppResult<UpdateLegaNameResult>.CreateFailed(new ApplicationException(res.Message), res.Message);
            }

            return AppResult<UpdateLegaNameResult>.CreateSucceeded(new UpdateLegaNameResult { Customer = customer }, "Customer successfuly updated");
        }
        catch (Exception ex)
        {
            return AppResult<UpdateLegaNameResult>.CreateFailed(ex, "An error occured in UpdateAboutHandler");
        }
    }
}