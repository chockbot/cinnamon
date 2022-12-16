using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.Common;
using Microsoft.EntityFrameworkCore;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class CustomerEntity : GenericEntity<Customer>, ICustomer
{
    private readonly ApplicationContext applicationContext;

    public CustomerEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }

    public async Task<AppResult<Customer>> GetCustomerByEmail(string email)
    {
        try
        {
            var result = await applicationContext.Customers.FirstOrDefaultAsync(w => w.Email == email);
            if(result == null)
            {
                return AppResult<Customer>.CreateFailed(new ApplicationException("Can't find customer by email"), "Can't find customer by email");
            }

            return AppResult<Customer>.CreateSucceeded(result, "Successfully find customer by email");
        }
        catch (Exception ex)
        {
            return AppResult<Customer>.CreateFailed(ex, "An error occured when getting customer by email");
        }
    }
}