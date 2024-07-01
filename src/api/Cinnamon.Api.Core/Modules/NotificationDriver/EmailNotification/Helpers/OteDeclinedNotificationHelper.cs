namespace Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification.Helpers;

public class OteDeclinedNotificationHelper
{
    public string GetTemplate(string eventName, string body, string customerName)
    {
        string defaultBody = $@"
            <p style='margin: 0'>
                Thank you for your interest in the {eventName} with
                Cinnamon.ph. We regret to inform you that your registration has not been
                accepted at this time.
            </p>

            <p style='margin-bottom: 1rem'>
                Due to high demand, we have limited spots available and were unable to
                accommodate all applicants. We encourage you to stay connected with us
                for future events and opportunities.
            </p>

            <p style='margin-bottom: 1rem'>Thank you for your understanding.</p>
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
                    Registration Not Accepted for {eventName}
                </p>

                <p style='margin-bottom: 1rem; text-transform: capitalize'>
                    Dear {customerName},
                </p>

                {(string.IsNullOrEmpty(body) ? defaultBody : body)}

                <p style='margin-bottom: 1rem'>Sincerely,</p>

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