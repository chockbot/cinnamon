using Flurl;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification.Helpers;

public class OteThankYouNotificationHelper
{
    public string GetTemplate(string eventName, string subject, string body, string customerName, string link)
    {
        string defaultBody = $@"
            <p style='margin: 0'>
                Thank you for joining us at the Autumn Harvest Festival! We hope you had
                a great time.
            </p>

            <p style='margin-bottom: 1rem'>We look forward to seeing you!</p>

            <p style='margin-bottom: 1rem'>Best regards,</p>

            <p style='margin-bottom: 0'>Cinnamon.ph Team</p>
        ";

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

            <div style='margin-top: 30px; color: #717171'>
                <div style='width: 80%; margin-left: auto; margin-right: auto'>
                <p
                    style='
                    text-align: center;
                    font-weight: bolder;
                    color: #0f173b;
                    font-size: 18px;
                    margin-bottom: 1.5rem;
                    '
                >
                    {(string.IsNullOrEmpty(subject.Trim()) ? eventName : subject )}
                </p>

                <p style='margin-bottom: 1rem; text-transform: capitalize'>Dear {customerName},</p>

                {(string.IsNullOrEmpty(body.Trim()) ? defaultBody : body)}

                <p style='margin-bottom: 1rem; margin-top: 3rem;'>What did you think of sample event?</p>
                <div style='display: flex'>
                    <a
                    href='{link}?rating=1'
                    style='
                        font-size: 24px;
                        display: block;
                        padding-left: 10px;
                        padding-right: 10px;
                        padding-top: 5px;
                        padding-bottom: 5px;
                        box-shadow: 0px 0px 48px 0px rgba(217, 217, 217, 0.6);
                        margin-right: 10px;
                        cursor: pointer;
                    '
                    >&#128544;</a
                    >
                    <a
                    href='{link}?rating=2'
                    style='
                        font-size: 24px;
                        display: block;
                        padding-left: 10px;
                        padding-right: 10px;
                        padding-top: 5px;
                        padding-bottom: 5px;
                        box-shadow: 0px 0px 48px 0px rgba(217, 217, 217, 0.6);
                        margin-right: 10px;
                        cursor: pointer;
                    '
                    >&#128542;</a
                    >
                    <a
                    href='{link}?rating=3'
                    style='
                        font-size: 24px;
                        display: block;
                        padding-left: 10px;
                        padding-right: 10px;
                        padding-top: 5px;
                        padding-bottom: 5px;
                        box-shadow: 0px 0px 48px 0px rgba(217, 217, 217, 0.6);
                        margin-right: 10px;
                        cursor: pointer;
                    '
                    >&#128528;</a
                    >
                    <a
                    href='{link}?rating=4'
                    style='
                        font-size: 24px;
                        display: block;
                        padding-left: 10px;
                        padding-right: 10px;
                        padding-top: 5px;
                        padding-bottom: 5px;
                        box-shadow: 0px 0px 48px 0px rgba(217, 217, 217, 0.6);
                        margin-right: 10px;
                        cursor: pointer;
                    '
                    >&#128512;</a
                    >
                    <a
                    href='{link}?rating=5'
                    style='
                        font-size: 24px;
                        display: block;
                        padding-left: 10px;
                        padding-right: 10px;
                        padding-top: 5px;
                        padding-bottom: 5px;
                        box-shadow: 0px 0px 48px 0px rgba(217, 217, 217, 0.6);
                        margin-right: 10px;
                        cursor: pointer;
                    '
                    >&#128525;</a
                    >
                </div>
                
                </div>
            </div>
        </div>
        ";
    }
}