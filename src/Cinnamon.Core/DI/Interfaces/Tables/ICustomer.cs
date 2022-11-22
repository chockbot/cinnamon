using Cinnamon.Core.Models;

namespace Cinnamon.Core;

public interface ICustomer : IBaseTable<CustomerModel> 
{
    Task<CustomerModel> GetCustomerByEmailAsync(string email);
    Task<CustomerModel> GetCustomerByUserId(string userId);
    Task<CustomerModel> GetCustomerById(int id);
    Task<CustomerModel> UpdateFamilyMembersAsync(int customerId, IList<FamilyMemberModel> members);
}