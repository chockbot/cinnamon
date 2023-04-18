using Flurl;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification.Helpers;

public class VerifyResetPasswordNotificationHelper 
{
    public string GetTemplate(string email, DateTime dateChanged, string host)
    {
        string imgSrc = host.AppendPathSegment("images/cinnamon-logo.png");

        return $@"
            <div
            style='
                font-size: 16px;
                border-radius: 5px;
                max-width: 80%;
                color: #545454;
                margin: 0 auto;
                margin-top: 3rem;
            '
            >
                <div style='margin-bottom: 2rem'>
                    <img
                    src='{imgSrc}'
                    alt='logo'
                    style='width: 200px; height: auto'
                    />
                </div>
                <p><b>Password Reset Notification</b></p>
                <p style='margin-bottom: 1.5rem; margin-top: 1.5rem'>
                    The Password on your Cinnamon account has been reset successfully. If you
                    performed the password reset, then this is only for your information.
                </p>
                <p style='margin-bottom: 1.5rem; margin-top: 1.5rem'>
                    Cinnamon Account: <u>{email}</u>
                    <br />
                    Date of Password Change: {dateChanged.ToString("MMMM dd, yyyy hh:mm:ss tt")}
                </p>
                <p style='margin-bottom: 1.5rem; margin-top: 1.5rem'>
                    If you did not change or are unsure if you changed your password, then
                    please contact us at <u>support@cinnamon.ph</u>.
                </p>
                <div style='margin-top: 2rem'>
                    <p style='margin-top: 0; margin-bottom: 0.3rem'>Thank you!</p>
                    <br />
                    <p style='margin-top: 0; margin-bottom: 0.3rem'>
                    This is a system generated message. Please do not reply to this email.
                    </p>
                </div>
            </div>
        ";
    }
}