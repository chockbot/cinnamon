using Flurl;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification.Helpers;

public class OteEventReminderNotificationHelper
{
    public string GetTemplate(string eventName, string subject, string body, string customerName,
        DateTime eventStart, string eventLocation, int daysStart)
    {
        string defaultBody = $@"
            <p style='margin: 0'>
                We are excited to remind you that {eventName} is just {daysStart} {(daysStart > 1 ? "days" : "day")}
                away! Here are the details to ensure you are fully prepared for a
                fantastic experience: Please make sure to bring a copy of your
                registration confirmation and arrive early to enjoy all the activities
                planned
            </p>

            <p style='margin-bottom: 1rem'>We look forward to seeing you!</p>

            <p style='margin-bottom: 1rem'>Best regards,</p>

            <p style='margin-bottom: 1rem'>Cinnamon.ph Team</p>
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
                    {(string.IsNullOrEmpty(subject.Trim()) ? eventName : subject )} is starting in {daysStart} {(daysStart > 1 ? "days" : "day")}
                </p>

                <p style='margin-bottom: 1rem; text-transform: capitalize'>Dear {customerName},</p>

                {(string.IsNullOrEmpty(body.Trim()) ? defaultBody : body)}
                
                </div>

                <hr />

                <div style='width: 80%; margin-left: auto; margin-right: auto'>
                <p style='margin-bottom: 1rem; margin-top: 1rem'>Event Details:</p>
                <p style='margin-bottom: 5px; margin-top: 0'>Date: {eventStart.ToString("MMMM dd, yyyy")}</p>
                <p style='margin-bottom: 5px; margin-top: 0'>Time: {eventStart.ToString("hh:mm tt")}</p>
                <p style='margin-bottom: 5px; margin-top: 0'>Location: {eventLocation}</p>

                <p style='margin-top: 1rem'>
                    For questions and concerns, email us at
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