using Cinnamon.Framework.ApiCommand.ApiData.GuestOTP.Request;
using Cinnamon.Framework.ApiCommand.ApiData.GuestOTP.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface IGuestOTPData
{
    Task<AppResult<CreateGuestOTPResult>> CreateOTP(CreateGuestOTPArgs args);
    Task<AppResult<GetGuestOTPByEmailResult>> GetOTPByEmail(GetGuestOTPByEmailArgs args);
}
