using Flurl;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification.Helpers;

public class ResetPasswordNotificationHelper 
{
    public string GetTemplate(string link, string host, string email)
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
                <p><b>Request to Reset Password</b></p>
                <p style='margin-bottom: 1.5rem; margin-top: 1.5rem'>
                    We have received a password change request for your Cinnamon Account
                    <u>{email}</u>
                </p>
                <p style='margin-bottom: 1.5rem; margin-top: 1.5rem'>
                    If you did not ask to change your password, then you can ignore this email
                    and your password will be retained. This request will remain active for 24
                    hrs
                </p>
                <p style='margin-bottom: 1.5rem; margin-top: 1.5rem'>
                    Click below if you would like to reset your password.
                </p>
                <a
                    href='{link}'
                    style='
                    display: block;
                    width: 200px;
                    background-color: #ffa942;
                    padding: 10px;
                    text-align: center;
                    text-decoration: none;
                    color: #545454;
                    '
                    >Reset Password</a
                >
                <div style='margin-top: 2rem'>
                    <p style='margin-top: 0; margin-bottom: 0.3rem'>
                    This is a system generated message. Please do not reply to this email.
                    </p>
                    <br />
                </div>
            </div>
        ";
    }
}