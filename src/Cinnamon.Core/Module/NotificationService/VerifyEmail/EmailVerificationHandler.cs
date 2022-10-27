using Cinnamon.Core.Common;
using Cinnamon.Core.Module.EmailService.Handler;
using Cinnamon.Core.Module.NotificationService.Interactors;
using Cinnamon.Core.Module.NotificationService.Interactors.Results;

namespace Cinnamon.Core.Module.NotificationService.Handler.VerifyEmail;

public class EmailVerificationHandler: IEmailVerification 
{
    private readonly ISendMailHandler sendMailHandler;

    public EmailVerificationHandler(ISendMailHandler sendMailHandler)
    {
        this.sendMailHandler = sendMailHandler;
    }

    public AppResult<EmailVerificationResult> Execute (EmailVerification args)
    {
        throw new NotImplementedException();
    }

    public async Task<AppResult<EmailVerificationResult>> ExecuteAsync(EmailVerification args)
    {
        try
        {
            var emailBody = GetTemplate(args.VerificationLink);
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

    private string GetTemplate(string link)
    {
        return $@"
            <div
                style='
                    padding: 3rem;
                    font-size: 16px;
                    font-weight: bold;
                    width: 550px;
                    border: 2px solid;
                    border-radius: 5px;
                    margin: 0 auto;
                    color: #545454;
                '
                >
                <h1 style='color: #ffa942; margin-top: 0'>Cinnamon</h1>
                <p>Hi,</p>
                <p>
                    Great to have you in Cinnamon! For us to verify your email address, please
                    click the button below
                </p>
                <a
                    href='{link}'
                    style='
                    display: block;
                    width: 100px;
                    background-color: #ffa942;
                    padding: 10px;
                    text-align: center;
                    text-decoration: none;
                    color: #545454;
                    '
                    >Log In</a
                >
                <p>We're so glad to welcome you!</p>
                <p>Cinnamon Team</p>
                <div style='font-size: 12px; font-weight: normal'>
                    <p>Trouble logging In? Paste this URL in to your browser:</p>
                    <p>Didn't make this request? You can safety ignore and delete this email</p>
                </div>
            </div>
        ";
    }
}