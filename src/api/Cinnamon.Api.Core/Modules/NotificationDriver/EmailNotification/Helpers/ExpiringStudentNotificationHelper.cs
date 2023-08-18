namespace Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification.Helpers;

public class ExpiringStudentNotificationHelper 
{
    public string GetTemplate(DateTime dateSend, string firstname, string lastname, string activityName,
        DateTime expirationDate, decimal amount, string activityLink)
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
                    background-color: #fffcf9;
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
                    class='purchased-customer'
                    style='padding: 1.5rem; padding-bottom: 0.5rem'
                >
                    <p style='margin: 0; font-size: 16px; color: #717171'>
                    Date: {dateSend.ToString("MMMM, dd yyyy")}
                    </p>
                    <p style='margin: 0; margin-top: 10px; color: #ffa942; font-size: 20px'>
                    Hi <span style='text-transform: capitalize'>{firstname} {lastname}</span>,
                    </p>
                </div>
                <div class='message-section' style='padding: 1.5rem; padding-bottom: 0.5rem'>
                    <p style='font-size: 16px; color: #343d4c; margin-bottom: 0'>
                    <b>Activity Expiring Soon!</b>
                    </p>
                    <p
                    style='
                        font-size: 16px;
                        text-align: justify;
                        color: #717171;
                        word-wrap: break-word;
                    '
                    >
                    We would like to inform you that the expiration date of your
                    {activityName} is approaching, and it is set for {expirationDate.ToString("MMMM, dd yyyy")}. 
                    If you find value in the services we provide and would like to
                    continue enjoying them, the opportunity to renew is available for your
                    consideration
                    </p>
                </div>
                <div
                    class='message-section'
                    style='padding: 1.5rem; padding-bottom: 0.5rem; padding-top: 0'
                >
                    <p style='font-size: 16px; color: #343d4c; margin-bottom: 0'>
                    <b>Detals:</b>
                    </p>
                    <ul style='color: #717171; word-wrap: break-word'>
                    <li>Activity Name: {activityName}</li>
                    <li>Expiration Date: {expirationDate.ToString("MMMM, dd yyyy")}</li>
                    <li>Renewal Amount: {amount.ToString("#,##0.00")}</li>
                    </ul>
                </div>
                <div
                    class='message-section'
                    style='padding: 1.5rem; padding-bottom: 0.5rem; padding-top: 0'
                >
                    <p style='font-size: 16px; color: #343d4c; margin-bottom: 0'>
                    <b>Renew your activity here:</b>
                    </p>
                    <ul style='color: #717171; word-wrap: break-word'>
                    <li>Visit {activityLink}</li>
                    </ul>
                </div>
                <div
                    class='message-section'
                    style='padding: 1.5rem; padding-bottom: 0.5rem; padding-top: 0'
                >
                    <p style='color: #717171; word-wrap: break-word'>
                    If you need help, where here: support@cinnamon.ph or send us a message on
                    facebook at https://www.facebook.com/cinnamonexperience
                    </p>
                </div>
            </div>
        ";
    }
}