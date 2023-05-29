using System.Linq.Expressions;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.CustomerPricing;
using Cinnamon.Framework.Common;
using Entities = Cinnamon.Api.Data.Repository.Entities;

namespace Cinnamon.Api.Data.Services.Repository.CustomerPricing;

public class CustomerPricingRepository : ICustomerPricingRepository
{
    private readonly IDataStore dataStore;

    public CustomerPricingRepository(IDataStore dataStore)
    {
        this.dataStore = dataStore;
    }
    
    public async Task<AppResult<CustomerPricingDTO>> Create(int customerId, string email, decimal rate)
    {
        try
        {
            var entity = new Entities.CustomerPricing {
                CustomerId = customerId,
                Email = email,
                Rate = rate,
            };

            var result = await dataStore.CustomerPricing.Add(entity);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<CustomerPricingDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var includes = new List<Expression<Func<Entities.CustomerPricing, object>>>();
            includes.Add(c => c.Customer);
            
            var getCreatedRes = await dataStore.CustomerPricing.FindFirstAsync(a => a.Id == entity.Id, includes);
            if(!getCreatedRes.Succeeded || getCreatedRes.Result == null)
            {
                return AppResult<CustomerPricingDTO>.CreateFailed(new ApplicationException(getCreatedRes.Message), getCreatedRes.Message);
            }
            var created = getCreatedRes.Result;

            return AppResult<CustomerPricingDTO>.CreateSucceeded(new CustomerPricingDTO {
                Email = created.Email,
                FirstName = created.Customer.FirstName,
                Id = created.Id,
                LastName = created.Customer.LastName,
                Rate = created.Rate
            }, "Successfully created customer pricing");
        }
        catch (Exception ex)
        {
            return AppResult<CustomerPricingDTO>.CreateFailed(ex, "An error occured when creating customer pricing");
        }
    }

    public async Task<AppResult<IEnumerable<CustomerPricingDTO>>> GetAllAsync(int? count, int? skip)
    {
        try
        {
            var result = await dataStore.CustomerPricing.GetAllowedCustomers(count, skip);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<CustomerPricingDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var customerPricings = result.Result.Select(c => {
                return new CustomerPricingDTO {
                    Email = c.Email,
                    FirstName = c.Customer.FirstName,
                    LastName = c.Customer.LastName,
                    Id = c.Id,
                    Rate = c.Rate
                };
            });

            return AppResult<IEnumerable<CustomerPricingDTO>>.CreateSucceeded(customerPricings, "Successfully get customer pricing list");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<CustomerPricingDTO>>.CreateFailed(ex, "An error occured when getting customer pricing");
        }
    }

    public async Task<AppResult<IEnumerable<CustomerPricingDTO>>> GetAllAsync()
    {
        try
        {
            var result = await dataStore.CustomerPricing.GetAllAsync();
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<CustomerPricingDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var customerPricings = result.Result.Select(c => {
                return new CustomerPricingDTO {
                    Email = c.Email,
                    FirstName = c.Customer?.FirstName ?? string.Empty,
                    LastName = c.Customer?.LastName ?? string.Empty,
                    Id = c.Id,
                    Rate = c.Rate
                };
            });

            return AppResult<IEnumerable<CustomerPricingDTO>>.CreateSucceeded(customerPricings, "Successfully get customer pricing list");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<CustomerPricingDTO>>.CreateFailed(ex, "An error occured when getting customer pricing");
        }
    }

    public async Task<AppResult<CustomerPricingDTO>> GetByCustomerIdAsync(int id) 
    {
        try
        {
            var includes = new List<Expression<Func<Entities.CustomerPricing,object>>>();
            includes.Add(c => c.Customer);

            var result = await dataStore.CustomerPricing.FindFirstAsync(c => c.CustomerId == id,includes);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<CustomerPricingDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }
            var customerPricing = result.Result;

            return AppResult<CustomerPricingDTO>.CreateSucceeded(new CustomerPricingDTO {
                Email = customerPricing.Email,
                FirstName = customerPricing.Customer.FirstName,
                Id = customerPricing.Id,
                LastName = customerPricing.Customer.LastName,
                Rate = customerPricing.Rate
            }, "Successfully get customer pricing by customer id");
        }
        catch (Exception ex)
        {
            return AppResult<CustomerPricingDTO>.CreateFailed(ex, "An error occured when getting customer pricing by customer id");
        }
    }

    public async Task<AppResult<CustomerPricingDTO>> GetByIdAsync(int id)
    {
        try
        {
            var includes = new List<Expression<Func<Entities.CustomerPricing,object>>>();
            includes.Add(c => c.Customer);

            var result = await dataStore.CustomerPricing.FindFirstAsync(c => c.Id == id,includes);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<CustomerPricingDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }
            var customerPricing = result.Result;

            return AppResult<CustomerPricingDTO>.CreateSucceeded(new CustomerPricingDTO {
                Email = customerPricing.Email,
                FirstName = customerPricing.Customer.FirstName,
                Id = customerPricing.Id,
                LastName = customerPricing.Customer.LastName,
                Rate = customerPricing.Rate
            }, "Successfully get customer pricing by id");
        }
        catch (Exception ex)
        {
            return AppResult<CustomerPricingDTO>.CreateFailed(ex, "An error occured when getting customer pricing by id");
        }
    }

    public async Task<AppResult<CustomerPricingDTO>> Update(int id, decimal rate)
    {
        try
        {
            var result = await dataStore.CustomerPricing.GetByIdAsync(id);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<CustomerPricingDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }
            result.Result.Rate = rate;

            var updatedRes = await dataStore.CustomerPricing.Update(result.Result);
            if(!updatedRes.Succeeded || updatedRes.Result == null)
            {
                return AppResult<CustomerPricingDTO>.CreateFailed(new ApplicationException(updatedRes.Message), updatedRes.Message);
            }

            return AppResult<CustomerPricingDTO>.CreateSucceeded(new CustomerPricingDTO {
                Email = result.Result.Email,
                Id = result.Result.Id,
                Rate = rate
            }, "Successfuly updated customer pricing");
        }
        catch (Exception ex)
        {
            return AppResult<CustomerPricingDTO>.CreateFailed(ex, "An error occured when updating customer pricing");
        }
    }
}