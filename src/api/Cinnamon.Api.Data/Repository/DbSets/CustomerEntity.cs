using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class CustomerEntity : GenericEntity<Customer>, ICustomer
{
    public CustomerEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }
}