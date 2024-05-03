namespace Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification.Helpers;

public class ExpiringStudentNotificationHelper 
{
    public string GetTemplate(DateTime dateSend, string firstname, string lastname, string activityName,
        DateTime expirationDate, decimal amount, string activityLink, string activityAddress, string imageLocation,
        string aPrice, decimal rating, int ratingCount)
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
                    Date: {dateSend.ToString("MMMM dd, yyyy")}
                    </p>
                    <p style='margin: 0; margin-top: 10px; color: #ffa942; font-size: 20px'>
                    Hi <span style='text-transform: capitalize'>{firstname}</span>,
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
                    <div
                        style='
                            width: 215px;
                            margin-left: auto;
                            margin-right: auto;
                            color: #343d4c;
                            margin-top: 30px;
                        '
                        >
                        <a
                            href='{activityLink}'
                            style='color: #343d4c; text-decoration: none'
                        >
                            <img
                            style='width: 100%; height: auto; border-radius: 10px'
                            src='{imageLocation}'
                            alt='image-icon'
                            />
                            <div>
                            <div style='display: flex; align-items: center; margin-top: 5px'>
                                <img
                                src='https://cinnamon.ph/images/expolore-rating-star.png'
                                alt='star'
                                />
                                <p style='margin: 0; margin-left: 5px'>{rating}({ratingCount})</p>
                            </div>
                            <p style='margin: 0; font-size: 16px; margin-top: 5px'>
                                <b>{activityName}</b>
                            </p>
                            <p style='margin: 0; font-size: 14px'>{activityAddress}</p>
                            <p style='margin: 0; font-size: 14px'>{aPrice}</p>
                            </div>
                        </a>
                    </div>
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