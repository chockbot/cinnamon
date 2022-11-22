using Cinnamon.Core.Models;
using Cinnamon.Core.Common;
using Cinnamon.Core.Module.CustomerService.Interactors;
using Cinnamon.Core.Module.CustomerService.Interactors.Results;

namespace Cinnamon.Core.Module.CustomerService.Handler.Profile;

public class UpdateCustomerFamilyMemberHandler : IUpdateCustomerFamilyMembers
{
    public AppResult<UpdateFamilyMemberResult> Execute(UpdateFamilyMember args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<UpdateFamilyMemberResult>.CreateFailed(ex, "An error occured in UpdateAboutHandler");
        }
    }

    public async Task<AppResult<UpdateFamilyMemberResult>> ExecuteAsync(UpdateFamilyMember args)
    {
        try
        {
            if(args.FamilyMembers == null || args.FamilyMembers.Count == 0)
            {
                return AppResult<UpdateFamilyMemberResult>.CreateFailed(new ApplicationException("Don't have family members to update"), "Don't have family members to update");
            }

            // get customer data
            var customer = await CoreDI.DataStore.Customers.GetCustomerById(args.CustomerId);
            if(customer == null)
            {
                return AppResult<UpdateFamilyMemberResult>.CreateFailed(new ApplicationException("Can't find customer data"), "Can't find customer data");
            }

            var result = await CoreDI.DataStore.Customers.UpdateFamilyMembersAsync(args.CustomerId, args.FamilyMembers);
            if(result == null)
            {
                return AppResult<UpdateFamilyMemberResult>.CreateFailed(
                    new ApplicationException("An error occured when updating customer family members"), "An error occured when updating customer family members");
            }

            // get again customer data
            var updatedCustomer = await CoreDI.DataStore.Customers.GetCustomerById(args.CustomerId);
            if(updatedCustomer == null)
            {
                return AppResult<UpdateFamilyMemberResult>.CreateFailed(new ApplicationException("Can't find updated customer data"), "Can't find updated customer data");
            }

            return AppResult<UpdateFamilyMemberResult>.CreateSucceeded(new UpdateFamilyMemberResult { Customer = updatedCustomer }, "Customer successfuly updated");
        }
        catch (Exception ex)
        {
            return AppResult<UpdateFamilyMemberResult>.CreateFailed(ex, "An error occured in UpdateAboutHandler");
        }
    }
}