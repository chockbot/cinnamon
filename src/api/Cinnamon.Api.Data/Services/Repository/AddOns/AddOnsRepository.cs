using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.AddOns;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Schedule;
using Cinnamon.Framework.Common;
using System.Linq.Expressions;
using Entities = Cinnamon.Api.Data.Repository.Entities;

namespace Cinnamon.Api.Data.Services.Repository.AddOns;
public class AddOnsRepository: IAddOnsRepository
{
    private readonly IDataStore _dataStore;
    public AddOnsRepository(IDataStore dataStore)
    {
        _dataStore = dataStore;
    }
    public async Task<AppResult<AddOnsDTO>> CreateAddOn(int ActivityId, string Name, decimal Price, string UnitPrice, string Description, int Order)
    {
        try
        {
            var checkActivity = await _dataStore.Activity.GetByIdAsync(ActivityId);
            if (!checkActivity.Succeeded)
            {
                return AppResult<AddOnsDTO>.CreateFailed(checkActivity.Error.Exception, checkActivity.Message);
            }
            var result = await _dataStore.AddOns.Add(new Data.Repository.Entities.AddOns()
            {
                ActivityId  = ActivityId,
                Name        = Name,
                Price       = Price,
                UnitPrice   = UnitPrice,
                Description = Description,
                Order       = Order
            });
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<AddOnsDTO>.CreateFailed(result.Error.Exception, result.Message);
            }

            return AppResult<AddOnsDTO>.CreateSucceeded(new AddOnsDTO()
            {
                Id          = ActivityId,
                ActivityId  = ActivityId,
                Name        = Name,
                Price       = Price,
                UnitPrice   = UnitPrice,
                Description = Description,
                Order       = Order
            },
            result.Message);
        }
        catch (Exception ex)
        {
            return AppResult<AddOnsDTO>.CreateFailed(ex.InnerException, ex.Message);
        }
    }
    public async Task<AppResult<IEnumerable<AddOnsDTO>>> CreateAddOns(int ActivityId, IEnumerable<AddOnsDTO> addons)
    {
        try
        {
            // check activity id if existed
            var activity = await _dataStore.Activity.GetByIdAsync(ActivityId);
            List<Entities.AddOns> dtoList = new List<Entities.AddOns>();

            if (!activity.Succeeded || activity.Result == null)
            {
                return AppResult<IEnumerable<AddOnsDTO>>.CreateFailed(
                    new ApplicationException("Can't find provided activity id"), "Can't find provided activity id");
            }
            
            foreach (var addon in addons)
            {
                var entity = new Entities.AddOns
                {
                    ActivityId  = ActivityId,
                    Name        = addon.Name,
                    Price       = addon.Price,
                    UnitPrice   = addon.UnitPrice,
                    Description = addon.Description,
                    Order       = addon.Order
                };

                var result = await _dataStore.AddOns.Add(entity);

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<IEnumerable<AddOnsDTO>>.CreateFailed(new ApplicationException(result.Message), "An error occurred when creating multiple add-on");
                }

                dtoList.Add(result.Result);
            }

            var dtos = dtoList.Select(s =>
            {
                return new AddOnsDTO
                {
                    Id          = s.Id,
                    ActivityId  = s.ActivityId,
                    Name        = s.Name,
                    Price       = s.Price,
                    UnitPrice   = s.UnitPrice,
                    Description = s.Description,
                    Order       = s.Order
                };
            });
            return AppResult<IEnumerable<AddOnsDTO>>.CreateSucceeded(dtos, "Successfully create multiple add-ons");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<AddOnsDTO>>.CreateFailed(ex, "An error occurred when creating multiple add-ons");
        }
    }
    public async Task<AppResult<bool>> DeleteManyAddOns(IEnumerable<int> addonIds)
    {
        try
        {
            if (addonIds == null || addonIds.Count() == 0)
            {
                return AppResult<bool>.CreateFailed(new ApplicationException("No add-ons to delete"), "No add-ons to delete");
            }
            addonIds = addonIds.Where(i => i > 0);
            if (addonIds.Count() == 0)
            {
                return AppResult<bool>.CreateFailed(new ApplicationException("No add-ons to delete"), "No add-ons to delete");
            }

            var result = await _dataStore.AddOns.RemoveRange(addonIds.Select(i => {
                return new Entities.AddOns
                {
                    Id = i
                };
            }));

            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<bool>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            return AppResult<bool>.CreateSucceeded(true, "Successfully deleted add-ons");
        }
        catch (Exception ex)
        {
            return AppResult<bool>.CreateFailed(ex, "An error occured in deleting many add-ons");
        }
    }
    public async Task<AppResult<IEnumerable<AddOnsDTO>>> GetAllAsync()
    {
        try
        {
            var result = await _dataStore.AddOns.GetAllAsync();
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<AddOnsDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }

            var AddOns = result.Result.Select(a =>
            {
                return new AddOnsDTO
                {
                    Id          = a.Id,
                    ActivityId  = a.ActivityId,
                    Name        = a.Name,
                    Price       = a.Price,
                    UnitPrice   = a.UnitPrice,
                    Description = a.Description,
                    Order       = a.Order
                };
            });

            return AppResult<IEnumerable<AddOnsDTO>>.CreateSucceeded(AddOns, "Success");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<AddOnsDTO>>.CreateFailed(ex.InnerException, ex.Message);
        }
    }
    public async Task<AppResult<AddOnsDTO>> GetByIdAsync(int id)
    {
        try
        {
            var includes = new List<Expression<Func<Entities.AddOns, object>>>();
            var result = await _dataStore.AddOns.FindFirstAsync(s => s.Id == id);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<AddOnsDTO>.CreateFailed(result.Error.Exception, result.Message);
            }

            return AppResult<AddOnsDTO>.CreateSucceeded(new AddOnsDTO()
            {
                Id          = result.Result.Id,
                ActivityId  = result.Result.ActivityId,
                Name        = result.Result.Name,
                Price       = result.Result.Price,
                UnitPrice   = result.Result.UnitPrice,
                Description = result.Result.Description,
                Order       = result.Result.Order
            }, result.Message);
        }
        catch (Exception ex)
        {
            return AppResult<AddOnsDTO>.CreateFailed(ex, ex.Message);
        }
    }

    public async Task<AppResult<AddOnsDTO>> UpdateAddOn(int? AddOnId, int? ActivityId, string? Name, decimal? Price, string? UnitPrice, string? Description, int? Order)
    {
        try
        {
            var checkAddOn = await _dataStore.AddOns.GetByIdAsync(AddOnId.GetValueOrDefault());
            if (!checkAddOn.Succeeded || checkAddOn.Result == null)
            {
                return AppResult<AddOnsDTO>.CreateFailed(checkAddOn.Error.Exception, checkAddOn.Message);
            }

            var addOn = checkAddOn.Result;

            addOn.Id          = AddOnId.GetValueOrDefault();
            addOn.ActivityId  = addOn.ActivityId;
            addOn.Name        = Name ?? addOn.Name;
            addOn.Price       = Price ?? addOn.Price;
            addOn.UnitPrice   = UnitPrice ?? addOn.UnitPrice; 
            addOn.Description = Description ?? addOn.Description;
            addOn.Order       = Order ?? addOn.Order;

            var result = await _dataStore.AddOns.Update(addOn);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<AddOnsDTO>.CreateFailed(result.Error.Exception, result.Message);
            }

            return AppResult<AddOnsDTO>.CreateSucceeded(new AddOnsDTO()
            {
                Id          = result.Result.Id,
                ActivityId  = result.Result.ActivityId,
                Name        = result.Result.Name,
                Price       = result.Result.Price,
                UnitPrice   = result.Result.UnitPrice,
                Description = result.Result.Description,
                Order       = result.Result.Order
            }, 
            result.Message);
        }
        catch (Exception ex)
        {
            return AppResult<AddOnsDTO>.CreateFailed(ex.InnerException, ex.Message);
        }
    }

    public async Task<AppResult<IEnumerable<AddOnsDTO>>> UpdateAddOns(IEnumerable<AddOnsDTO> addons)
    {
        try
        {
            if (addons.Count() <= 0)
            {
                return AppResult<IEnumerable<AddOnsDTO>>.CreateFailed(new ApplicationException("No add-ons to update"), "No add-ons to update");
            }
            var addonsToUpdae = addons.Select(x =>
            {
                return new Entities.AddOns
                {
                    Id          = x.Id,
                    ActivityId  = x.ActivityId,
                    Name        = x.Name,
                    Price       = x.Price,
                    UnitPrice   = x.UnitPrice,
                    Description = x.Description,
                    Order       = x.Order
                };
            });

            var updated = await _dataStore.AddOns.UpdateRange(addonsToUpdae);
            if (!updated.Succeeded || updated.Result == null)
            {
                return AppResult<IEnumerable<AddOnsDTO>>.CreateFailed(new ApplicationException(updated.Message), updated.Message);
            }

            return AppResult<IEnumerable<AddOnsDTO>>.CreateSucceeded(
                updated.Result.Select(s =>
                {
                    return new AddOnsDTO
                    {
                        Id          = s.Id,
                        ActivityId  = s.ActivityId,
                        Name        = s.Name,
                        Price       = s.Price,
                        UnitPrice   = s.UnitPrice,
                        Description = s.Description,
                        Order       = s.Order
                    };
                }), "Successfully update add-ons");

        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<AddOnsDTO>>.CreateFailed(ex, "An error occurred in updating add-ons");
        }
    }
}
