using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.Common;
using System.Linq.Expressions;

namespace Cinnamon.Api.Data.Repository.Interfaces;

public interface ICustomer : IGenericEntity<Customer>
{
    Task<AppResult<IEnumerable<Customer>>> FindCustomerAsync(Expression<Func<Customer, bool>> expression, string searchValue,
        int? take = 100, int? skip = 0);
    Task<AppResult<Customer>> GetCustomerByEmail(string email);
}