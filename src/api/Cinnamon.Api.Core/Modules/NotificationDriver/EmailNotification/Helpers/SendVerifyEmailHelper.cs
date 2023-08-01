using Flurl;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification.Helpers;

public class SendVerifyEmailHelper 
{
    public string GetTemplate(string link, string host)
    {
        string imgSrc = host.AppendPathSegment("images/cinnamon-logo.png");

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
                        Ready to sprinkle Cinnamon in your life! Verify your email address by clicking the button below.
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
                        <p>{link}</p>
                        <p>Didn't make this request? You can safety ignore and delete this email</p>
                    </div>
                </blockquote>
            </div>
        ";
    }
}