using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Activity.DTO;
using Cinnamon.Api.Data.Services.Repository.ActivityAddress.DTO;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.ActivityAddress
{
    public class AddressRepository : IAddressRepository
    {
        private readonly IDataStore _dataStore;

        public AddressRepository(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public async Task<AppResult<AddressDTO>> CreateAddress(int ActivityId, string Address1, string Address2, string District, string City)
        {
            try
            {
                var checkActivity = await _dataStore.Activity.GetByIdAsync(ActivityId);
                if(!checkActivity.Succeeded)
                {
                    return AppResult<AddressDTO>.CreateFailed(checkActivity.Error.Exception, checkActivity.Message);
                }

                var result = await _dataStore.ActivityAddress.Add(new Data.Repository.Entities.ActivityAddress()
                {
                    ActivityId = ActivityId,
                    Address1 = Address1,
                    Address2 = Address2,
                    District = District,
                    City = City
                });

                var createdAddressDTO = new AddressDTO()
                {
                    Id = result.Result.Id,
                    ActivityId = result.Result.ActivityId,
                    Address1 = result.Result.Address1,
                    Address2 = result.Result.Address2,
                    District = result.Result.District,
                    City = result.Result.City
                };

                return AppResult<AddressDTO>.CreateSucceeded(createdAddressDTO, "Address successfully created");

            }catch(Exception ex)
            {
                return AppResult<AddressDTO>.CreateFailed(ex, "An error occured when creating address");
            }
        }

        public async Task<AppResult<IEnumerable<AddressDTO>>> GetAllAsync(int? count, int? skip)
        {
            try
            {
                count = count.HasValue ? count.Value : 0;
                skip = skip.HasValue ? skip.Value : 0;

                var result = await _dataStore.ActivityAddress.FindAsync(a=>a.Id != -1,count.Value, skip.Value);
                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<IEnumerable<AddressDTO>>.CreateFailed(result.Error.Exception, result.Message);
                }

                var activitieAddress = result.Result.Select(a =>
                {
                    return new AddressDTO
                    {
                        Id = a.Id,
                        ActivityId = a.ActivityId,
                        Address1 = a.Address1,
                        Address2 = a.Address2,
                        District = a.District,
                        City = a.City
                    };
                });

                return AppResult<IEnumerable<AddressDTO>>.CreateSucceeded(activitieAddress, "Successfully get activities");
            }
            catch (Exception ex)
            {
                return AppResult<IEnumerable<AddressDTO>>.CreateFailed(ex, "An error occured in getting activities");
            }
        }

        public async Task<AppResult<AddressDTO>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _dataStore.ActivityAddress.GetByIdAsync(id);
                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<AddressDTO>.CreateFailed(result.Error.Exception, result.Message);
                }

                var activitieAddress = new AddressDTO
                {
                    Id = result.Result.Id,
                    ActivityId = result.Result.ActivityId,
                    Address1 = result.Result.Address1,
                    Address2 = result.Result.Address2,
                    District = result.Result.District,
                    City = result.Result.City
                };

                return AppResult<AddressDTO>.CreateSucceeded(activitieAddress, "Successfully getting activity by id");
            }
            catch (Exception ex)
            {
                return AppResult<AddressDTO>.CreateFailed(ex, "An error occured when getting activity by id");
            }
        }

        public async Task<AppResult<AddressDTO>> RemoveAddress(int AddressId)
        {
            try
            {
                var result = await _dataStore.ActivityAddress.Remove(new Data.Repository.Entities.ActivityAddress()
                {
                    Id = AddressId
                });

                return AppResult<AddressDTO>.CreateSucceeded(new AddressDTO()
                {
                    Id = result.Result.Id,
                    ActivityId = result.Result.ActivityId,
                    Address1 = result.Result.Address1,
                    Address2 = result.Result.Address2,
                    District = result.Result.District,
                    City = result.Result.City
                },"Deleted Successfully");
            }
            catch(Exception ex)
            {
                return AppResult<AddressDTO>.CreateFailed(ex, "There was an error deleting the address");
            }
        }

        public async Task<AppResult<AddressDTO>> UpdateAddress(int AddressId, string Address1,string Address2, string District, string City)
        {
            try
            {
                // check first activity if exist
                var AddressRes = await _dataStore.ActivityAddress.GetByIdAsync(AddressId);
                if (!AddressRes.Succeeded || AddressRes.Result == null)
                {
                    return AppResult<AddressDTO>.CreateFailed(AddressRes.Error.Exception, AddressRes.Message);
                }

                var address = AddressRes.Result;
                address.Address1 = Address1;
                address.Address2 = Address2;
                address.District = District;
                address.City = City;

                var result = await _dataStore.ActivityAddress.Update(address);

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<AddressDTO>.CreateFailed(result.Error.Exception, result.Message);
                }

                var activitieAddress = new AddressDTO
                {
                    Id = result.Result.Id,
                    ActivityId = result.Result.ActivityId,
                    Address1 = result.Result.Address1,
                    Address2 = result.Result.Address2,
                    District = result.Result.District,
                    City = result.Result.City
                };

                return AppResult<AddressDTO>.CreateSucceeded(activitieAddress, "Successfully updated");
            }
            catch (Exception ex)
            {
                return AppResult<AddressDTO>.CreateFailed(ex, "An error occured when updating the address");
            }
        }
    }
}
