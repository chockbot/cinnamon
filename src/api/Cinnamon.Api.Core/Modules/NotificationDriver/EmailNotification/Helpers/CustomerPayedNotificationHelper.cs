using Flurl;
using static Cinnamon.Api.Core.Modules.NotificationDriver.Interactors.CustomerPayedNotificationArgs;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification.Helpers;

public class CustomerPayedNotificationHelper 
{
    public string GetTemplate(string customerName, string experienceName, string coachName, 
        DateTime purchaseDate, string payerName, decimal amount, string host, 
        IEnumerable<IncludedMembers> members)
    {
        string imgSrc = host.AppendPathSegment("images/cinnamon-logo.png");
        string liHTML = string.Empty;

        if(members.Count() > 0)
        {
            liHTML = "<ul style='margin-top: 0.5rem'>";
            foreach(var m in members)
            {
                liHTML += $"<li>{m.Name}</li>";
            }
            liHTML += "</ul>";
        }

        return $@"
                <div
                    style='
                        font-size: 16px;
                        border-radius: 5px;
                        max-width: 80%;
                        color: #545454;
                        margin: 0 auto;
                        margin-top: 3rem;
                    '
                    >
                    <div style='margin-bottom: 2rem'>
                        <img
                        src='{imgSrc}'
                        alt='logo'
                        style='width: 200px; height: auto'
                        />
                    </div>
                    <p><b>Successful Payment for the Cinnamon Experience</b></p>
                    <p style='margin-bottom: 1.5rem'>Hi <u>{customerName}!</u></p>
                    <p style='margin-bottom: 1.5rem'>
                        You have successfully paid PHP {amount.ToString("#,###.00")} for your enrollment in the Cinnamon
                        Experience <u>{experienceName}</u> with <u>{coachName}</u>.
                    </p>
                    <p style='margin-bottom: 5px'>Inluded Members:</p>
                    {liHTML}
                    <div
                        id='details'
                        style='
                        padding: 1.5rem;
                        background-color: #ffc000;
                        border: 2px solid #fba358;
                        color: #fff;
                        '
                    >
                        <p style='margin-top: 0; margin-bottom: 0.3rem'>
                        <b>Payment Reference Number: CIN12345</b>
                        </p>
                        <p style='margin-top: 0; margin-bottom: 0'>
                        <b>Date: {purchaseDate.ToString("MMMM dd, yyyy hh:mm:ss tt")}</b>
                        </p>
                    </div>
                    <div
                        id='account-details'
                        style='border: 2px solid #e3eaec; padding: 1.5rem; border-top: 0'
                    >
                        <p style='margin-top: 0; margin-bottom: 0.3rem'>
                        <b>Payer Name: <u>{payerName}</u></b>
                        </p>
                        <p style='margin-top: 0; margin-bottom: 0.3rem'>
                        <b>Account Number: <u>XXXX-XXXX-XXXX-9999</u></b>
                        </p>
                        <p style='margin-top: 0; margin-bottom: 0.3rem'>
                        <b>Bank Name: <u>Security Bank</u></b>
                        </p>
                    </div>
                    <div
                        id='enrolled-details'
                        style='border: 2px solid #e3eaec; padding: 1.5rem; border-top: 0'
                    >
                        <p style='margin-top: 0; margin-bottom: 0.3rem'>To: {coachName}</p>
                        <p style='margin-top: 0; margin-bottom: 0.3rem'>Experience: {experienceName}</p>
                        <p style='margin-top: 0; margin-bottom: 0.3rem'>Date: {purchaseDate.ToString("MMMM dd, yyyy hh:mm:ss tt")}</p>
                    </div>
                    <div
                        id='amount'
                        style='border: 2px solid #e3eaec; padding: 1.5rem; border-top: 0'
                    >
                        <p style='margin-top: 0; margin-bottom: 0.3rem'>Amount</p>
                        <p style='margin-top: 0; margin-bottom: 0.3rem'>PHP {amount.ToString("#,###.00")}</p>
                    </div>
                    <div style='margin-top: 2rem'>
                        <p style='margin-top: 0; margin-bottom: 0.3rem'>
                        This is a system generated message. Please do not reply to this email.
                        </p>
                        <p style='margin-top: 0; margin-bottom: 0.3rem'>
                        Thank you for trusting Cinnamon. See you in class!
                        </p>
                    </div>
                </div>
            ";
    }
}