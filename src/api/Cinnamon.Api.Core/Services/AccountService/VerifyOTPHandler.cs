using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AccountService;
public class VerifyOTPHandler : IVerifyOTPHandler
{
    private readonly IGuestOTPData guestOTPData;
    public VerifyOTPHandler(IGuestOTPData guestOTPData)
    {
        this.guestOTPData = guestOTPData;
    }

    public AppResult<VerifyOTPResult> Execute(VerifyOTPArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<VerifyOTPResult>.CreateFailed(ex, "An error occured in SubmitVerifyEmailHandler");
        }
    }

    public Task<AppResult<VerifyOTPResult>> ExecuteAsync(VerifyOTPArgs args)
    {
        throw new NotImplementedException();
    }
}
