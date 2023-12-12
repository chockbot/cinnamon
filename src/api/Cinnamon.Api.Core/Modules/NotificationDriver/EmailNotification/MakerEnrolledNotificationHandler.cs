using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.EmailDriver.Handlers;
using Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification.Helpers;
using Cinnamon.Api.Core.Modules.NotificationDriver.Handler;
using Cinnamon.Api.Core.Modules.NotificationDriver.Interactors;
using Cinnamon.Api.Core.Modules.NotificationDriver.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification;

public class MakerEnrolledNotificationHandler : IMakerEnrolledNotificationHandler
{
    private readonly ISendMailHandler sendMailHandler;
    private readonly ApplicationConfig config;
    private readonly MakerEnrolledNotificationHelper helper;

    public MakerEnrolledNotificationHandler(ISendMailHandler sendMailHandler, ApplicationConfig config)
    {
        this.sendMailHandler = sendMailHandler;
        this.config = config;
        this.helper = new MakerEnrolledNotificationHelper();
    }

    public AppResult<MakerEnrolledNotificationResult> Execute(MakerEnrolledNotificationArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<MakerEnrolledNotificationResult>.CreateFailed(ex, "An error occured in MakerEnrolledNotificationHandler");
        }
    }

    public async Task<AppResult<MakerEnrolledNotificationResult>> ExecuteAsync(MakerEnrolledNotificationArgs args)
    {
        try
        {
            var emailBody = helper.GetTemplate(args.MakerName, args.ExperienceName, args.PurchaseDate, 
                args.PayerName, args.Amount, args.ServiceFee, config.FrontendUrl, args.Students, args.ReferenceNumber, args.PaymentMethod,
                args.PayerEmail, args.PaymentProviderFee, args.AppliedCredits, args.IsInclusivePayment, args.DiscountAmount, args.AddOnsAmount);
                
            var sendMailResponse = await sendMailHandler
                .ExecuteAsync(new EmailDriver.Interactors.SendMailArgs {
                    Body = emailBody,
                    Recipients = new List<string> { args.Email },
                    Subject = "New Enrolled Students",
                    ContentType = "html"
                });
            
            if(!sendMailResponse.Succeeded)
            {
                return AppResult<MakerEnrolledNotificationResult>
                    .CreateFailed(new ApplicationException(sendMailResponse.Error.Description), sendMailResponse.Message);
            }

            return AppResult<MakerEnrolledNotificationResult>.CreateSucceeded(
                    new MakerEnrolledNotificationResult{}, "New enrolled notification sent");
        }
        catch (Exception ex)
        {
            return AppResult<MakerEnrolledNotificationResult>.CreateFailed(ex, "An error occured in MakerEnrolledNotificationHandler");
        }
    }
}