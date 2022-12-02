using Cinnamon.Core.Common;
using Cinnamon.Core.Module.CustomerService.Interactors;
using Cinnamon.Core.Module.CustomerService.Interactors.Results;

namespace Cinnamon.Core.Module.CustomerService.Handler.Profile;

public class UpdateBirthDateHandler : IUpdateBirthDate
{
    public AppResult<UpdateBirthDateResult> Execute(UpdateBirthDateArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<UpdateBirthDateResult>.CreateFailed(ex, "An error occured in UpdateAboutHandler");
        }
    }

    public async Task<AppResult<UpdateBirthDateResult>> ExecuteAsync(UpdateBirthDateArgs args)
    {
        try
        {
            // get customer data
            var customer = await CoreDI.DataStore.Customers.GetCustomerById(args.CustomerId);
            if(customer == null)
            {
                return AppResult<UpdateBirthDateResult>.CreateFailed(new ApplicationException("Can't find customer data"), "Can't find customer data");
            }

            customer.Birthdate = args.BirthDate;
            var res = await CoreDI.DataStore.Customers.SaveDataAsync(customer);
            if(!res.Message.ToLower().Contains("saved"))
            {
                return AppResult<UpdateBirthDateResult>.CreateFailed(new ApplicationException(res.Message), res.Message);
            }

            return AppResult<UpdateBirthDateResult>.CreateSucceeded(new UpdateBirthDateResult { Customer = customer }, "Customer successfuly updated");
        }
        catch (Exception ex)
        {
            return AppResult<UpdateBirthDateResult>.CreateFailed(ex, "An error occured in UpdateAboutHandler");
        }
    }
}