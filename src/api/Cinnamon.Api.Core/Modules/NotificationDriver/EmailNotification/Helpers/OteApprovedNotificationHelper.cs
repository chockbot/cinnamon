namespace Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification.Helpers;

public class OteApprovedNotificationHelper
{
    public string GetTemplate(string eventName, string body, string customerName, 
        DateTime eventDate, string eventLocation, string approvedUrl, bool paidTicket)
    {
        string defaultBody = $@"
            <p style='margin: 0'>
                We are thrilled to inform you that your registration {eventName} with Cinnamon.ph has been confirmed!
            </p>

            <p style='margin-bottom: 1rem'>
                Please bring a copy of your your ticket for entry. We look
                forward to seeing you at the festival!
            </p>
        ";

        string urlLinkSection = string.Empty;
        if(paidTicket)
        {
            urlLinkSection = $@"
                <p style='margin-bottom: 1rem'>
                    Please follow the <a href='{approvedUrl}'>link</> to purchase the approved ticket.
                </p>
            ";
        }
        else 
        {
            urlLinkSection = $@"
                <p style='margin-bottom: 1rem'>
                    Use this <a href='{approvedUrl}'>Ticket Link</a> to view your ticket.
                </p>
            ";
        }

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
                    Registration Confirmed for {eventName}
                </p>

                <p style='margin-bottom: 1rem; text-transform: capitalize'>
                    Dear {customerName},
                </p>

                {(string.IsNullOrEmpty(body) ? defaultBody : body)}

                {urlLinkSection}

                <p style='margin-bottom: 1rem'>Warm regards,</p>

                <p style='margin-bottom: 1.5rem'>Cinnamon.ph Team</p>
                </div>

                <hr />

                <div style='width: 80%; margin-left: auto; margin-right: auto'>
                <p style='margin-bottom: 1rem; margin-top: 1rem'>Event Details:</p>
                <p style='margin-bottom: 5px; margin-top: 0'>Date: {eventDate.ToString("MMMM dd, yyyy")}</p>
                <p style='margin-bottom: 5px; margin-top: 0'>Time: {eventDate.ToString("hh:mm tt")}</p>
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