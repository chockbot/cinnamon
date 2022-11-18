using Cinnamon.Core.Common;
using Cinnamon.Core.Config;
using Cinnamon.Core.Module.EmailService.Handler;
using Cinnamon.Core.Module.NotificationService.Interactors;
using Cinnamon.Core.Module.NotificationService.Interactors.Results;

namespace Cinnamon.Core.Module.NotificationService.Handler.VerifyEmail;

public class EmailVerificationHandler: IEmailVerification 
{
    private readonly ISendMailHandler sendMailHandler;
    private readonly VerifyEmailHelper helper;
    private readonly CoreConfig coreConfig;

    public EmailVerificationHandler(ISendMailHandler sendMailHandler, CoreConfig coreConfig)
    {
        this.sendMailHandler = sendMailHandler;
        this.helper = new VerifyEmailHelper();
        this.coreConfig = coreConfig;
    }

    public AppResult<EmailVerificationResult> Execute (EmailVerification args)
    {
        throw new NotImplementedException();
    }

    public async Task<AppResult<EmailVerificationResult>> ExecuteAsync(EmailVerification args)
    {
        try
        {
            var emailBody = helper.GetTemplate(args.VerificationLink, coreConfig.BaseUrl);
            var sendMailResponse = await sendMailHandler
                .ExecuteAsync(new EmailService.Interactors.SendMail()
                {
                    Body = emailBody,
                    From = "dexter.echalico@cinnamon.ph",
                    Recipients = new List<string> { args.Email },
                    Subject = "Email Verification",
                    ContentType = "html"
                });
            
            if(!sendMailResponse.Succeeded)
            {
                return AppResult<EmailVerificationResult>
                    .CreateFailed(new ApplicationException(sendMailResponse.Error.Description), sendMailResponse.Message);
            }
            
            return AppResult<EmailVerificationResult>.CreateSucceeded(new EmailVerificationResult(), "Verification link successfully send");
        }
        catch (Exception ex)
        {
            return AppResult<EmailVerificationResult>.CreateFailed(ex, "An error occured during EmailVerificationHandler");
        }
    }
}