using Flurl;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification.Helpers;
public class SendEmailOTPHelper
{
    public string GetTemplate(int[] OTP, string host)
    {
        string imgSrc = host.AppendPathSegment("images/cinnamon-logo.png");
        string otpString = string.Join(" ", OTP.Select(x => x.ToString()));

        return $@"
            <div
                style='
                    font-size: 16px;
                    font-weight: bold;
                    width: 550px;
                    border: 2px solid;
                    border-radius: 5px;
                    margin: 0 auto;
                    color: #545454;
                '
                >
                <blockquote style='margin: 3rem'>
                    <div style='margin-bottom: 2rem'>
                        <img
                        src='{imgSrc}'
                        alt='logo'
                        style='width: 200px; height: auto'
                        />
                    </div>
                    <p>Hi,</p>
                    <p>
                        Ready to sprinkle Cinnamon in your life! Verify your email address using OTP below.
                    </p>
                    <div style='display:flex;gap: 5px;font-size: 24px; font-weight: bold;'>
                         {otpString}
                    </div>
                    
                    <p>We're so glad to welcome you!</p>
                    <p>Cinnamon Team</p>
                    <div style='font-size: 12px; font-weight: normal'>
                        <p>Didn't make this request? You can safety ignore and delete this email</p>
                    </div>
                </blockquote>
            </div>
        ";
    }
}
