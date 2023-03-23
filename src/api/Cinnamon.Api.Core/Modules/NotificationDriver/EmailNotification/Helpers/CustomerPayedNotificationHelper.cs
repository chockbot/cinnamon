using Flurl;
using static Cinnamon.Api.Core.Modules.NotificationDriver.Interactors.CustomerPayedNotificationArgs;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification.Helpers;

public class CustomerPayedNotificationHelper 
{
    public string GetTemplate(string customerName, string experienceName, string coachName, 
        DateTime purchaseDate, string payerName, decimal amount, string host, 
        IEnumerable<IncludedMembers> members, string referenceNumber, string paymentMethod)
    {
        string imgSrc = host.AppendPathSegment("images/cinnamon-logo.png");
        string enrolleesString = string.Empty;

        foreach(var item in members)
        {
            if(string.IsNullOrEmpty(enrolleesString))
            {
                enrolleesString += item.Name;
            }
            else 
            {
                enrolleesString += $", {item.Name}";
            }
        }

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
                    '
                    >
                    <div class='img-logo' style='padding-top: 1rem'>
                        <img
                        style='width: 200px; height: auto; margin: 0 auto; display: block'
                        src='{imgSrc}'
                        alt='logo'
                        />
                    </div>
                    <div
                        class='purchased-customer'
                        style='padding: 1.5rem; padding-bottom: 0.5rem'
                    >
                        <p style='margin: 0; font-size: 16px; color: #717171'>
                        Date: {purchaseDate.ToString("MMMM dd, yyyy")}
                        </p>
                        <p style='margin: 0; margin-top: 1rem; color: #ffa942; font-size: 20px'>
                        Hi <span style='text-transform: capitalize'>{customerName}</span>,
                        </p>
                        <p style='margin: 0; font-size: 20px; color: #343d4c'>
                        Thank you for your purhcase!
                        </p>
                    </div>
                    <hr style='margin: 0; border: none; height: 1px; background-color: #d9d9d9' />
                    <div class='purchased-details' style='padding: 2rem 2rem'>
                        <p style='font-size: 20px; margin: 0; color: #343d4c'>
                        <b style='text-transform: capitalize'>{experienceName}</b>
                        </p>
                        <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                        <span style='color: #717171'>Enrollees: </span>
                        <span style='color: #343d4c; text-transform: capitalize'
                            >{enrolleesString}</span
                        >
                        </p>
                        <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                        <span style='color: #717171'>Paid To: </span>
                        <span style='color: #343d4c; text-transform: capitalize'
                            >{coachName}</span
                        >
                        </p>
                        <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                        <span style='color: #717171'>Payment Reference Number: </span>
                        <span style='color: #343d4c; text-transform: uppercase'>{referenceNumber}</span>
                        </p>
                        <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                        <span style='color: #717171'>Amount: </span>
                        <span style='color: #343d4c; text-transform: uppercase'
                            ><b>PHP {amount.ToString("#,##0.00")}</b></span
                        >
                        </p>
                    </div>
                    <hr style='margin: 0; border: none; height: 1px; background-color: #d9d9d9' />
                    <div class='payer-details' style='padding: 2rem 2rem'>
                        <p style='font-size: 16px; margin: 0'>
                        <span style='color: #717171'>Payer Name: </span>
                        <span style='color: #000'
                            ><u style='text-transform: capitalize'>{payerName}</u></span
                        >
                        </p>
                        <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                        <span style='color: #717171'>Payment Method: </span>
                        <span style='color: #000'
                            ><u style='text-transform: uppercase'>{paymentMethod}</u></span
                        >
                        </p>
                    </div>
                </div>

            ";
    }
}