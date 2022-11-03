namespace Cinnamon.Core.Module.NotificationService.Handler.VerifyEmail;

public class VerifyEmailHelper 
{
    public string GetTemplate(string link)
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
                <div style='margin-bottom: 2rem'>
                    <img
                    src='http://dev.cinnamon.ph/images/cinnamon-logo.png'
                    alt='logo'
                    style='width: 200px; height: auto'
                    />
                </div>
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