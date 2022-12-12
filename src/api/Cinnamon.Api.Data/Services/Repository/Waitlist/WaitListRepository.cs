using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Waitlist.DTO;
using Cinnamon.Api.Data.Repository.Interfaces;
using Entities = Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Waitlist;

public class WaitListRepository : IWaitListRepository
{
    private readonly IDataStore dataStore;

    public WaitListRepository(IDataStore dataStore)
    {
        this.dataStore = dataStore;
    }

    public async Task<AppResult<WaitListDTO>> Create(string email, string guid, string token, bool isVerified)
    {
        try
        {
            // check if email already existed
            var waitlistCheck = await dataStore.WaitList.FindFirstAsync(w => w.Email == email);
            if (waitlistCheck.Succeeded && waitlistCheck.Result != null)
            {
                return AppResult<WaitListDTO>.CreateFailed(new ApplicationException("Can't create already existed email address"), "Can't create already existed email address");
            }

            var waitlist = new Entities.WaitList
            {
                Email = email,
                Guid = guid,
                Token = token,
                IsVerified = isVerified,
            };

            var createdWaitListRes = await dataStore.WaitList.Add(waitlist);
            if (!createdWaitListRes.Succeeded || createdWaitListRes.Result == null)
            {
                return AppResult<WaitListDTO>.CreateFailed(createdWaitListRes.Error.Exception, createdWaitListRes.Message);
            }

            return AppResult<WaitListDTO>.CreateSucceeded(new WaitListDTO
            {
                Email = email,
                Guid = guid,
                Token = token,
                IsVerified = isVerified,
                Id = createdWaitListRes.Result.Id
            }, "Successfully created waitlist");
        }
        catch (Exception ex)
        {
            return AppResult<WaitListDTO>.CreateFailed(ex, "An error occured when creating waitlist");
        }
    }

    public async Task<AppResult<IEnumerable<WaitListDTO>>> GetAllAsync(int? count, int? skip)
    {
        try
        {
            var result = await dataStore.WaitList.FindAsync(i => true, count, skip);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<WaitListDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }

            var waitlist = result.Result.Select(w =>
            {
                return new WaitListDTO
                {
                    Email = w.Email,
                    Guid = w.Guid,
                    Token = w.Token,
                    IsVerified = w.IsVerified,
                    Id = w.Id,
                };
            });

            return AppResult<IEnumerable<WaitListDTO>>.CreateSucceeded(waitlist, "Successfully get waitlist");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<WaitListDTO>>.CreateFailed(ex, "An error occured in getting waitlist");
        }
    }

    public async Task<AppResult<IEnumerable<WaitListDTO>>> GetAllAsync()
    {
        try
        {
            var result = await dataStore.WaitList.GetAllAsync();
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<WaitListDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }

            var waitlist = result.Result.Select(w =>
            {
                return new WaitListDTO
                {
                    Email = w.Email,
                    Guid = w.Guid,
                    Token = w.Token,
                    IsVerified = w.IsVerified,
                    Id = w.Id
                };
            });

            return AppResult<IEnumerable<WaitListDTO>>.CreateSucceeded(waitlist, "Successfully get waitlist");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<WaitListDTO>>.CreateFailed(ex, "An error occured in getting waitlist");
        }
    }

    public async Task<AppResult<WaitListDTO>> GetByEmailAsync(string email)
    {
        try
        {
            var result = await dataStore.WaitList.GetWaitListByEmailAsync(email);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<WaitListDTO>.CreateFailed(result.Error.Exception, result.Message);
            }

            var waitListDTO = new WaitListDTO
            {
                Email = result.Result.Email,
                Guid = result.Result.Guid,
                Token = result.Result.Token,
                IsVerified = result.Result.IsVerified,
                Id= result.Result.Id
            };

            return AppResult<WaitListDTO>.CreateSucceeded(waitListDTO, "Successfully getting waitlist by email");
        }
        catch (Exception ex)
        {
            return AppResult<WaitListDTO>.CreateFailed(ex, "An error occured when getting waitlist by email");
        }
    }

    public async Task<AppResult<WaitListDTO>> GetByGuidAsync(string guid)
    {
        try
        {
            var result = await dataStore.WaitList.GetWaitListByGuidAsync(guid);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<WaitListDTO>.CreateFailed(result.Error.Exception, result.Message);
            }

            var waitListDTO = new WaitListDTO
            {
                Email = result.Result.Email,
                Guid = result.Result.Guid,
                Token = result.Result.Token,
                IsVerified = result.Result.IsVerified,
                Id = result.Result.Id
            };

            return AppResult<WaitListDTO>.CreateSucceeded(waitListDTO, "Successfully getting waitlist by guid");
        }
        catch (Exception ex)
        {
            return AppResult<WaitListDTO>.CreateFailed(ex, "An errod occured when getting waitlist by guid");
        }
    }

    public async Task<AppResult<WaitListDTO>> GetByIdAsync(int id)
    {
        try
        {
            var result = await dataStore.WaitList.GetByIdAsync(id);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<WaitListDTO>.CreateFailed(result.Error.Exception, result.Message);
            }

            var waitListDTO = new WaitListDTO
            {
                Email = result.Result.Email,
                Guid = result.Result.Guid,
                Token = result.Result.Token,
                IsVerified = result.Result.IsVerified,
                Id = result.Result.Id
            };

            return AppResult<WaitListDTO>.CreateSucceeded(waitListDTO, "Successfully getting waitlist by id");
        }
        catch (Exception ex)
        {
            return AppResult<WaitListDTO>.CreateFailed(ex, "An error occured when getting waitlist by id");
        }
    }

    public async Task<AppResult<WaitListDTO>> Update(string email, string? guid, string? token, bool? isVerified)
    {
        try
        {
            var result = await dataStore.WaitList.GetWaitListByEmailAsync(email);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<WaitListDTO>.CreateFailed(new ApplicationException("Can't find waitlist yo update"), "Can't find waitlist yo update");
            }
            var waitlist = result.Result;

            waitlist.Guid = guid ?? waitlist.Guid;
            waitlist.Token = token ?? waitlist.Token;
            waitlist.IsVerified = isVerified ?? waitlist.IsVerified;

            var updatedWaitlist = await dataStore.WaitList.Update(waitlist);
            if (!updatedWaitlist.Succeeded)
            {
                return AppResult<WaitListDTO>.CreateFailed(updatedWaitlist.Error.Exception, updatedWaitlist.Message);
            }

            return AppResult<WaitListDTO>.CreateSucceeded(new WaitListDTO
            {
                Email = waitlist.Email,
                Guid = waitlist.Guid,
                Token = waitlist.Token,
                IsVerified = waitlist.IsVerified,
                Id = waitlist.Id,
            }, "Successfully updated waitlist");
        }
        catch (Exception ex)
        {
            return AppResult<WaitListDTO>.CreateFailed(ex, "An error occured when updating waitlist");
        }
    }
}