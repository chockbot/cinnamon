namespace Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification.Helpers;

public class ChatUnreadNotificationHelper 
{
    public string GetTemplate()
    {
        return $@"
            <div
                style='
                    font-size: 16px;
                    border-radius: 5px;
                    min-width: 300px;
                    max-width: 700px;
                    color: #545454;
                    margin: 0 auto;
                    margin-top: 3rem;
                    border: 1px solid #343d4c;
                    position: relative;
                    padding-top: 20px;
                    padding-bottom: 30px;
                '
                >
                <div class='img-logo' style='padding-top: 1rem'>
                    <img
                    style='width: 200px; height: auto; margin: 0 auto; display: block'
                    src='https://stcinnamondev.blob.core.windows.net/assets/cinnamon-logo.png'
                    alt='logo'
                    />
                </div>
                <div
                    style='
                    text-align: center;
                    background-color: #f3f6fc;
                    border-radius: 24px;
                    width: 80%;
                    margin-left: auto;
                    margin-right: auto;
                    padding: 20px 10px;
                    margin-top: 25px;
                    '
                >
                    <p style='color: #0f173b; margin: 0; font-size: 24px; font-weight: bold'>
                    You have a new message
                    </p>
                    <p style='margin: 0; margin-top: 15px; color: #717171; font-size: 16px'>
                    Hope this email finds you well. We wanted to notify you that you've
                    received a new message on the Cinnamon Website. Here are the details:
                    </p>
                    <a
                    href='https://cinnamon.ph/Messages'
                    style='
                        background-color: #0f173b;
                        color: #ffffff;
                        text-decoration: none;
                        padding: 10px 50px;
                        display: inline-block;
                        border-radius: 20px;
                        margin-top: 20px;
                    '
                    >Read message</a
                    >
                </div>

                <div
                    style='
                    width: 80%;
                    margin-left: auto;
                    margin-right: auto;
                    margin-top: 30px;
                    color: #717171;
                    '
                >
                    <p style='margin: 0'>
                    To view and respond to the message, simply log in to your Cinnamon account
                    <a href='https://cinnamon.ph/Messages' style='color: #279be1'
                        >https://cinnamon.ph/Messages</a
                    >. Don't miss out on any important updates or conversations!
                    </p>
                    <p style='margin: 0; margin-top: 30px'>
                    Thank you for being a part of the Cinnamon community.
                    </p>
                </div>
            </div>
        ";
    }
}