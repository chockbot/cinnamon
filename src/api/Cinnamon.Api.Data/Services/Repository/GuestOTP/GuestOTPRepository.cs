using AutoMapper;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.GuestOTP;
using Cinnamon.Framework.Common;
using System.Linq.Expressions;
using Entities = Cinnamon.Api.Data.Repository.Entities;

namespace Cinnamon.Api.Data.Services.Repository.GuestOTP;
public class GuestOTPRepository : IGuestOTPRepository
{
    private readonly IDataStore dataStore;
    private readonly IMapper mapper;
    public GuestOTPRepository(IDataStore dataStore, IMapper mapper)
    {
        this.dataStore = dataStore;
        this.mapper = mapper;
    }
    public Task<AppResult<IEnumerable<GuestOTPDTO>>> GetAllAsync(int? count, int? skip)
    {
        throw new NotImplementedException();
    }

    public Task<AppResult<IEnumerable<GuestOTPDTO>>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<AppResult<IEnumerable<GuestOTPDTO>>> GetByEmailAsync(string email)
    {
        // Get the current UTC time
        var currentTime = DateTime.UtcNow;

        //Filter all not expired otp
        Expression<Func<Entities.GuestOTP, bool>> filter = a => (a.Email == email) &&
        (currentTime - a.CreatedOn) <= TimeSpan.FromMinutes(10);

        var result = await dataStore.GuestOTP.FindAsync(filter);
        if (!result.Succeeded || result.Result == null)
        {
            return AppResult<IEnumerable<GuestOTPDTO>>.CreateFailed(result.Error.Exception, result.Message);
        }
        var guestOTPDTOs = mapper.Map<IEnumerable<GuestOTPDTO>>(result.Result);
        return AppResult<IEnumerable<GuestOTPDTO>>.CreateSucceeded(guestOTPDTOs, "Successfully get all OTP of the guest");
    }
    public async Task<AppResult<GuestOTPDTO>> CreateGuestOTP(GuestOTPDTO guestOTPDTO)
    {
        var guestOTP = mapper.Map<Entities.GuestOTP>(guestOTPDTO);
        var result = await dataStore.GuestOTP.Add(guestOTP);
        if (!result.Succeeded || result.Result is null)
        {
            return AppResult<GuestOTPDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
        }
        var dtoGuestOtp = mapper.Map<GuestOTPDTO>(result.Result);
        return AppResult<GuestOTPDTO>.CreateSucceeded(dtoGuestOtp, "OTP successfully created");
    }

    public Task<AppResult<GuestOTPDTO>> UpdateGuestOTP(GuestOTPDTO guestOTPDTO)
    {
        throw new NotImplementedException();
    }
}
