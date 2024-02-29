using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class OteSharedLinkVerificationHandler : IOteSharedLinkVerificationHandler
{
    private readonly IOteTicketData oteTicketData;

    public OteSharedLinkVerificationHandler(IOteTicketData oteTicketData)
    {
        this.oteTicketData = oteTicketData;
    }

    public AppResult<OteSharedLinkVerificationResult> Execute(OteSharedLinkVerificationArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<OteSharedLinkVerificationResult>> ExecuteAsync(OteSharedLinkVerificationArgs args)
    {
        try
        {
            var sharedLinkRes = await oteTicketData.GetSharedLink(new Framework.ApiCommand.ApiData.OteTicket.Request.GetSharedLinkArgs {
                Guid = args.Guid,
                Token = args.Token
            });
            if(!sharedLinkRes.Succeeded || sharedLinkRes.Result is null || !sharedLinkRes.Result.IsSuccess)
            {
                return AppResult<OteSharedLinkVerificationResult>.CreateFailed(
                    new ApplicationException("Invalid request. Please contact support."), "Invalid request. Please contact support.", "error");
            }

            var sharedLink = sharedLinkRes.Result.Result.FirstOrDefault();
            if(sharedLink is null)
            {
                return AppResult<OteSharedLinkVerificationResult>.CreateFailed(
                    new ApplicationException("Invalid request. Please contact support."), "Invalid request. Please contact support.", "error");
            }

            var qrcodeRes = await oteTicketData.GetByCode(args.QrCode);
            if(!qrcodeRes.Succeeded || qrcodeRes.Result is null || !qrcodeRes.Result.IsSuccess)
            {
                return AppResult<OteSharedLinkVerificationResult>.CreateFailed(new ApplicationException("Invalid QR Code."), "Invalid QR Code.", "invalid");
            }
            var qrcode = qrcodeRes.Result.Result;

            if(qrcode.ActivityId != sharedLink.ActivityId)
            {
                return AppResult<OteSharedLinkVerificationResult>.CreateFailed(new ApplicationException("Invalid QR Code."), "Invalid QR Code.", "invalid");
            }

            if(qrcode.Status != "UNVERIFIED")
            {
                return AppResult<OteSharedLinkVerificationResult>.CreateFailed(new ApplicationException("QR Code already used."), "QR Code already used.", "duplicate");
            }

            if(sharedLink.OteDateId != qrcode.OteDateId)
            {
                return AppResult<OteSharedLinkVerificationResult>.CreateFailed(new ApplicationException("Invalid QR Code."), "Invalid QR Code.", "invalid");
            }

            var updateQrRes = await oteTicketData.UpdateTicket(new Framework.ApiCommand.ApiData.OteTicket.Request.UpdateTicketArgs {
                Id = qrcode.Id,
                Status = "VERIFIED"
            });
            if(!updateQrRes.Succeeded || updateQrRes.Result is null || !updateQrRes.Result.IsSuccess)
            {
                return AppResult<OteSharedLinkVerificationResult>.CreateFailed(new ApplicationException("An error occured. Please contact support."), "An error occured. Please contact support.", "error");
            }

            return AppResult<OteSharedLinkVerificationResult>.CreateSucceeded(new OteSharedLinkVerificationResult 
                {Verified = true, TicketSeat = qrcode.Title, Id = qrcode.Id}, "QR Code Successfully validated.");
        }
        catch (Exception ex)
        {
            return AppResult<OteSharedLinkVerificationResult>.CreateFailed(ex, "An error occured in OteSharedLinkVerificationHandler.");
        }
    }
}