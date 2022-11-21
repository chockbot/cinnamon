using Cinnamon.Core.Common;
using Cinnamon.Core.Module.CustomerService.Interactors;
using Cinnamon.Core.Module.CustomerService.Interactors.Results;

namespace Cinnamon.Core.Module.CustomerService.Handler.Profile;

public class UpdateCustomerAboutHandler : IUpdateCustomerAbout
{
    public AppResult<UpdateAboutResult> Execute(UpdateAbout args)
    {
        throw new NotImplementedException();
    }

    public async Task<AppResult<UpdateAboutResult>> ExecuteAsync(UpdateAbout args)
    {
        try
        {
            // get customer data
            var customer = await CoreDI.DataStore.Customers.GetCustomerById(args.CustomerId);
            if(customer == null)
            {
                return AppResult<UpdateAboutResult>.CreateFailed(new ApplicationException("Can't customer data"), "Can't customer data");
            }

            customer.About = args.About;
            var res = await CoreDI.DataStore.Customers.SaveDataAsync(customer);
            if(!res.Message.ToLower().Contains("saved"))
            {
                return AppResult<UpdateAboutResult>.CreateFailed(new ApplicationException(res.Message), res.Message);
            }

            return AppResult<UpdateAboutResult>.CreateSucceeded(new UpdateAboutResult { Customer = customer }, "Customer successfuly updated");
        }
        catch (Exception ex)
        {
            return AppResult<UpdateAboutResult>.CreateFailed(ex, "An error occured in UpdateAboutHandler");
        }
    }
}