using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class OteVerificationHandler : IOteVerificationHandler
{
    private readonly IGetProfileHandler getProfileHandler;
    private readonly IOteFindByHandler oteFindByHandler;
    private readonly IOteTicketData oteTicketData;

    public OteVerificationHandler(IGetProfileHandler getProfileHandler, IOteFindByHandler oteFindByHandler,
        IOteTicketData oteTicketData)
    {
        this.getProfileHandler = getProfileHandler;
        this.oteFindByHandler = oteFindByHandler;
        this.oteTicketData = oteTicketData;
    }

    public AppResult<OteVerificationResult> Execute(OteVerificationArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<OteVerificationResult>.CreateFailed(ex, "An error occured. Please contact support.");
        }
    }

    public async Task<AppResult<OteVerificationResult>> ExecuteAsync(OteVerificationArgs args)
    {
        try
        {
            var oteFindRes = await oteFindByHandler.ExecuteAsync(new OteFindByHandlerArgs {
                Handler = args.Handler,
            });
            if(!oteFindRes.Succeeded || oteFindRes.Result is null)
            {
                return AppResult<OteVerificationResult>.CreateFailed(new ApplicationException("Invalid QR Code."), "Invalid QR Code.", "invalid");
            }
            var oteDetails = oteFindRes.Result;

            var profileRes = await getProfileHandler.ExecuteAsync(new ());
            if(!profileRes.Succeeded || profileRes.Result is null)
            {
                return AppResult<OteVerificationResult>.CreateFailed(new ApplicationException("Invalid QR Code."), "Invalid QR Code.", "invalid");
            }
            var profile = profileRes.Result;

            if(profile.Id != oteDetails.ProviderId)
            {
                return AppResult<OteVerificationResult>.CreateFailed(new ApplicationException("Invalid QR Code."), "Invalid QR Code.", "invalid");
            }

            var qrcodeRes = await oteTicketData.GetByCode(args.QrCode);
            if(!qrcodeRes.Succeeded || qrcodeRes.Result is null || !qrcodeRes.Result.IsSuccess)
            {
                return AppResult<OteVerificationResult>.CreateFailed(new ApplicationException("Invalid QR Code."), "Invalid QR Code.", "invalid");
            }
            var qrcode = qrcodeRes.Result.Result;

            if(qrcode.ActivityId != oteDetails.Id)
            {
                return AppResult<OteVerificationResult>.CreateFailed(new ApplicationException("Invalid QR Code."), "Invalid QR Code.", "invalid");
            }

            if(qrcode.Status != "UNVERIFIED")
            {
                return AppResult<OteVerificationResult>.CreateFailed(new ApplicationException("QR Code already used."), "QR Code already used.", "duplicate");
            }

            if(args.DateId != qrcode.OteDateId)
            {
                return AppResult<OteVerificationResult>.CreateFailed(new ApplicationException("Invalid QR Code."), "Invalid QR Code.", "invalid");
            }

            var updateQrRes = await oteTicketData.UpdateTicket(new Framework.ApiCommand.ApiData.OteTicket.Request.UpdateTicketArgs {
                Id = qrcode.Id,
                Status = "VERIFIED"
            });
            if(!updateQrRes.Succeeded || updateQrRes.Result is null || !updateQrRes.Result.IsSuccess)
            {
                return AppResult<OteVerificationResult>.CreateFailed(new ApplicationException("An error occured. Please contact support."), "An error occured. Please contact support.", "error");
            }

            return AppResult<OteVerificationResult>.CreateSucceeded(new OteVerificationResult 
                {Verified = true, TicketSeat = qrcode.Title, Id = qrcode.Id}, "QR Code Successfully validated.");

        }
        catch (Exception ex)
        {
            return AppResult<OteVerificationResult>.CreateFailed(ex, "An error occured. Please contact support.");
        }
    }
}