using Flurl;
using static Cinnamon.Api.Core.Modules.NotificationDriver.Interactors.MakerEnrolledNotificationArgs;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification.Helpers;

public class MakerEnrolledNotificationHelper 
{
    public string GetTemplate(string makerName, string experienceName,
        DateTime purchaseDate, string payerName, decimal amount, decimal serviceFee, string host, 
        IEnumerable<IncludedStudents> students, string referenceNumber, string paymentMethod,
        string payerEmail, decimal providerFee)
    {
        string imgSrc = host.AppendPathSegment("https://stcinnamondev.blob.core.windows.net/assets/cinnamon-logo.png");
        string enrolleesString = string.Empty;

        foreach(var item in students)
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
                        background-color: #fffcf9;
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
                        Hi <span style='text-transform: capitalize'>{makerName}</span>,
                        </p>
                        <p style='margin: 0; font-size: 20px; color: #343d4c'>
                        There are new student/s joining your experience
                        </p>
                    </div>
                    <hr style='margin: 0; border: none; height: 1px; background-color: #d9d9d9' />
                    <div class='purchased-details' style='padding: 2rem 2rem'>
                        <p style='font-size: 20px; margin: 0; color: #343d4c'>
                        <b style='text-transform: capitalize'>{experienceName}</b>
                        </p>
                        <table style='width: 100%'>
                        <tbody>
                            <tr>
                            <td style='width: 50%'>
                                <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                                <span style='color: #717171'>Enrollees: </span>
                                </p>
                            </td>
                            <td style='text-align: right; width: 50%'>
                                <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                                <span style='color: #343d4c; text-transform: capitalize'
                                    >{enrolleesString} ({students.Count()})</span
                                >
                                </p>
                            </td>
                            </tr>
                            <tr>
                            <td style='width: 50%'>
                                <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                                <span style='color: #717171'>Payment Reference Number: </span>
                                </p>
                            </td>
                            <td style='text-align: right; width: 50%'>
                                <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                                <span style='color: #343d4c; text-transform: uppercase'
                                    >{referenceNumber}</span
                                >
                                </p>
                            </td>
                            </tr>
                            <tr>
                            <td style='width: 50%'>
                                <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                                <span style='color: #717171'>Amount: </span>
                                </p>
                            </td>
                            <td style='text-align: right; width: 50%'>
                                <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                                <span style='color: #343d4c; text-transform: uppercase'
                                    >PHP {amount.ToString("#,##0.00")}</span
                                >
                                </p>
                            </td>
                            </tr>
                            <tr>
                            <td style='width: 50%'>
                                <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                                <span style='color: #717171'>Payment Provider Fee: </span>
                                </p>
                            </td>
                            <td style='text-align: right; width: 50%'>
                                <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                                <span style='color: #343d4c; text-transform: uppercase'
                                    >PHP {providerFee.ToString("#,##0.00")}</span
                                >
                                </p>
                            </td>
                            </tr>
                            <tr>
                            <td style='width: 50%'>
                                <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                                <span style='color: #717171'>Service Fee: </span>
                                </p>
                            </td>
                            <td style='text-align: right; width: 50%'>
                                <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                                <span style='color: #343d4c; text-transform: uppercase'
                                    >PHP {serviceFee.ToString("#,##0.00")}</span
                                >
                                </p>
                            </td>
                            </tr>
                            <tr>
                            <td style='width: 50%'>
                                <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                                <b style='color: #343d4c'>Total Purchase: </b>
                                </p>
                            </td>
                            <td style='text-align: right; width: 50%'>
                                <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                                <span style='color: #343d4c; text-transform: uppercase'
                                    ><b
                                    >PHP {(amount + serviceFee +
                                    providerFee).ToString("#,##0.00")}</b
                                    ></span
                                >
                                </p>
                            </td>
                            </tr>
                        </tbody>
                        </table>
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
                        <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                        <span style='color: #717171'>Contact Info: </span>
                        <span style='color: #000'><u style=''>{payerEmail}</u></span>
                        </p>
                        <br />
                        <p style='margin: 0; font-size: 16px; color: #717171'>
                        Note: Please check your attendance section
                        </p>
                    </div>
                </div>
            ";
    }
}