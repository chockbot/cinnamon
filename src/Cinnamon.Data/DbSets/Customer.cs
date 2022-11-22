using System;
using Cinnamon.Core;
using Cinnamon.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Cinnamon.Data;

public class Customer : BaseDbSet<CustomerModel>, ICustomer 
{
     public Customer(DataStoreDbContext dbContext) : base(dbContext) { }
     protected override DbSet<CustomerModel> Table => mDbContext.Customers;

     public async Task<CustomerModel> GetCustomerByEmailAsync(string email)
         => await mDbContext.Customers.FirstOrDefaultAsync(i => i.Email == email);

     public async Task<CustomerModel> GetCustomerByUserId(string userId)
         => await mDbContext.Customers.FirstOrDefaultAsync(i => i.UserId == userId);

    public async Task<CustomerModel> GetCustomerById(int id)
         => await mDbContext.Customers.FirstOrDefaultAsync(i => i.Id == id);
    
    public async Task<CustomerModel> UpdateFamilyMembersAsync(int customerId, IList<FamilyMemberModel> members)
    {
        var getCustomer = await mDbContext.Customers.FirstOrDefaultAsync(i => i.Id == customerId);
        if(getCustomer == null) return getCustomer;

        // remove items
        for (int i = getCustomer.FamilyMembers.Count - 1; i >= 0; i--)
        {
            if(!members.Any(t => t.Id == getCustomer.FamilyMembers[i].Id))
            {
                getCustomer.FamilyMembers.RemoveAt(i);
            }
        }

        // update/add items
        if(members != null)
        {
            foreach(var item in members)
            {
                // new item
                if(item.Id == 0)
                {
                    getCustomer.FamilyMembers.Add(item);
                }
                else
                {
                    var member = getCustomer.FamilyMembers.FirstOrDefault(i => i.Id == item.Id);
                    if(member != null)
                    {
                        member.Name = item.Name;
                        member.BirthMonth = item.BirthMonth;
                        member.BirthYear = item.BirthYear;
                        member.Gender = item.Gender;
                    }
                }
            }
        }

        // save the changes
        await mDbContext.SaveChangesAsync();

        return getCustomer;
    }
}