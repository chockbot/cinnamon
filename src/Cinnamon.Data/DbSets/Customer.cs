using Cinnamon.Core;
using Cinnamon.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Cinnamon.Data;

public class Customer : BaseDbSet<CustomerModel>, ICustomer 
{
     public Customer(DataStoreDbContext dbContext) : base(dbContext) { }
     protected override DbSet<CustomerModel> Table => mDbContext.Customers;
}