using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class CustomerEntity : GenericEntity<Customer>, ICustomer
{
    private readonly ApplicationContext applicationContext;

    public CustomerEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }

    public async Task<AppResult<IEnumerable<Customer>>> FindCustomerAsync(Expression<Func<Customer, bool>> expression, string searchValue, int? take = 100, int? skip = 0)
    {
        try
        {
            int limitCount = take.HasValue ? take.Value : int.MaxValue;
            int skipCount = skip.HasValue ? skip.Value : 0;

            var query = applicationContext.Set<Customer>().Include(c => c.CustomerPricing).OrderBy(a => a.Id).Where(expression);

            if (!string.IsNullOrEmpty(searchValue))
            {
                string[] keywords = searchValue.ToLower().Trim().Split(' ');

                foreach (string keyword in keywords)
                {
                    query = query.Where(a => (string.IsNullOrEmpty(keyword) ? true : a.Email.ToLower().Trim().Contains(keyword)));
                }
            }
            query = query.Skip(skipCount).Take(limitCount);
         
            var results = await query.ToListAsync();
            return AppResult<IEnumerable<Customer>>.CreateSucceeded(results, "Successfully find entities");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<Customer>>.CreateFailed(ex, "An error occured when finding entities");
        }
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