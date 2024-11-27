using Cinnamon.Framework.ApiCommand.ApiData.DTO.GuestOTP;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IGuestOTPRepository
{
    Task<AppResult<IEnumerable<GuestOTPDTO>>> GetByEmailAsync(string email);
    Task<AppResult<IEnumerable<GuestOTPDTO>>> GetAllAsync(int? count, int? skip);
    Task<AppResult<IEnumerable<GuestOTPDTO>>> GetAllAsync();
    Task<AppResult<GuestOTPDTO>> UpdateGuestOTP(GuestOTPDTO guestOTPDTO);
    Task<AppResult<GuestOTPDTO>> CreateGuestOTP(GuestOTPDTO guestOTPDTO);
}
