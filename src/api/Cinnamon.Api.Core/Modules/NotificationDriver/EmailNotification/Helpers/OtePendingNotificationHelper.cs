namespace Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification.Helpers;

public class OtePendingNotificationHelper
{
    public string GetTemplate(string eventName, string body, string customerName)
    {
        string defaultBody = $@"
            <p style='margin-bottom: 1rem'>
                Thank you for registering for the {eventName} with Cinnamon.ph! 
                We have received your application and it is currently under review
            </p>

            <p style='margin-bottom: 1rem'>You will receive a confirmation email once your registration has been approved.</p>

            <p style='margin-bottom: 1rem'>
                Thank you for your interest in the {eventName}. We look forward to your participation.
            </p>
        ";

        return $@"
        <div
            style='
                font-size: 16px;
                border-radius: 12px;
                min-width: 300px;
                max-width: 700px;
                color: #545454;
                margin: 0 auto;
                margin-top: 3rem;
                border: 1px solid #d9d9d9;
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
                    Registration Pending Approval for {eventName}
                </p>

                <p style='margin-bottom: 1rem; text-transform: capitalize'>
                    Dear {customerName},
                </p>

                {(string.IsNullOrEmpty(body) ? defaultBody : body)}

                <p style='margin-bottom: 1rem'>Best regards,</p>

                <p style='margin-bottom: 1.5rem'>Cinnamon.ph Team</p>

                <p style='margin-bottom: 2rem'>
                    For questions & concerns, email us at
                    <a style='color: #279be1' href='mailto:support@cinnamon.ph'
                    >support@cinnamon.ph</a
                    >
                </p>
                </div>
            </div>
        </div>
        ";
    }
}