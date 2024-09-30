using Flurl;
using Microsoft.AspNetCore.Http;
using static Cinnamon.Api.Core.Modules.NotificationDriver.Interactors.CustomerPayedNotificationArgs;
using static Cinnamon.Framework.ApiCommand.ApiCore.Transaction.Request.SubmitPurchaseOrderArgs;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification.Helpers;

public class CustomerPayedNotificationHelper
{
    public string GetTemplate(string customerName, string experienceName, string coachName,
        DateTime purchaseDate, string payerName, decimal amount, decimal serviceFee, string host,
        IEnumerable<IncludedMembers> members, string referenceNumber, string paymentMethod,
        string makerEmail, string coachNumber, decimal providerFee, decimal appliedCredits, bool isInclusivePayment,
        decimal discountAmount, decimal addOnsAmount, IEnumerable<AddOnDetails> addOnsDetails)
    {
        string imgSrc = "https://stcinnamondev.blob.core.windows.net/assets/cinnamon-logo.png";
        string chatUrl = host.AppendPathSegment("Messages");
        string enrolleesString = string.Empty;
        string addOnsString = string.Empty;
        string addOnTitle = string.Empty;

        //Members
        foreach (var item in members)
        {
            if (string.IsNullOrEmpty(enrolleesString))
            {
                enrolleesString += item.Name;
            }
            else
            {
                enrolleesString += $", {item.Name}";
            }
        }
        //Add-ons
        if (addOnsDetails.Count() != 0)
        {
            addOnTitle = "Add-ons: ";
            foreach (var item in addOnsDetails)
            {
                if (string.IsNullOrEmpty(addOnsString))
                {
                    addOnsString += $"{item.AddOnName}({item.AddOnCount})";
                }
                else
                {
                    addOnsString += $", {item.AddOnName}({item.AddOnCount})";
                }
            }
        }
        else
        {
            addOnTitle   = string.Empty;
            addOnsString = string.Empty;
        }
        // Check if amount is greater than 0 before including the related information
        string amountHtmlString = amount > 0 ? $@"
            <tr>
                <td style='width: 50%'>
                    <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                    <span style='color: #717171'>Amount: </span>
                    </p>
                </td>
                <td style='text-align: right; width: 50%'>
                    <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                    <span style='color: #343d4c; text-transform: uppercase'>{amount.ToString("#,##0.00")}</span>
                    </p>
                </td>
            </tr>
        " : "";
        // Check if add-ons is not null
        string addonHtmlString = addOnsDetails.Count() != 0 ? $@"<td style='text-align: right; width: 50%'>
                                                                    <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                                                                    <span style='color: #343d4c; text-transform: capitalize'>{addOnsString}</span>
                                                                    </p>
                                                                 </td>" : "";
        string addOnTotalHtmlString = addOnsDetails.Count() != 0 ? $@"<tr>
                                                                        <td style='width: 50%'>
                                                                            <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                                                                            <span style='color: #717171'>Add-Ons: </span>
                                                                            </p>
                                                                        </td>
                                                                        <td style='text-align: right; width: 50%'>
                                                                            <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                                                                            <span style='color: #343d4c; text-transform: uppercase'
                                                                                >{addOnsAmount.ToString("#,##0.00")}</span
                                                                            >
                                                                            </p>
                                                                        </td>
                                                                       </tr>" : "";
        string paymentProviderHtmlString = string.Empty;
        string serviceFeeHtmlString = string.Empty;

        if (!isInclusivePayment)
        {
            paymentProviderHtmlString = $@"
                <tr>
                    <td style='width: 50%'>
                        <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                        <span style='color: #717171'>Service Fee: </span>
                        </p>
                    </td>
                    <td style='text-align: right; width: 50%'>
                        <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                        <span style='color: #343d4c; text-transform: uppercase'
                            >{providerFee.ToString("#,##0.00")}</span
                        >
                        </p>
                    </td>
                </tr>
            ";

            serviceFeeHtmlString = $@"
                <tr>
                    <td style='width: 50%'>
                        <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                        <span style='color: #717171'>Handling Fee: </span>
                        </p>
                    </td>
                    <td style='text-align: right; width: 50%'>
                        <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                        <span style='color: #343d4c; text-transform: uppercase'
                            >{serviceFee.ToString("#,##0.00")}</span
                        >
                        </p>
                    </td>
                </tr>
            ";
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
                        class='cinnamon-chat'
                        style='
                        background-color: #ffb84c;
                        padding-top: 35px;
                        padding-left: 10px;
                        padding-right: 10px;
                        padding-bottom: 30px;
                        margin-top: 3rem;
                        border-radius: 40px;
                        width: 88%;
                        margin-left: auto;
                        margin-right: auto;
                        margin-bottom: 2rem;
                        '
                    >
                        <p
                        style='
                            color: #0f173b;
                            text-align: center;
                            font-size: 20px;
                            font-weight: 700;
                        '
                        >
                        Thank you for your purchase!
                        </p>
                        <p
                        style='
                            color: #0f173b;
                            font-size: 16px;
                            text-align: justify;
                            margin-bottom: 0;
                        '
                        >
                        Hi {customerName}, we are excited for your experience! If you have
                        questions, clarifications, or just want to confirm anything, feel free to
                        message your activity provider by going to CHAT now.
                        </p>
                        <div style='text-align: center; margin-top: 3rem'>
                        <a
                            style='
                            background-color: #0f173b;
                            color: #ffffff;
                            padding: 15px 40px;
                            border-radius: 20px;
                            text-decoration: none;
                            '
                            href='{chatUrl}'
                            ><b>Chat Now!</b></a
                        >
                        </div>
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
                        Thank you for your purchase!
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
                                    <span style='color: #717171'>{addOnTitle} </span>
                                    </p>
                                </td>
                                {addonHtmlString}
                            </tr>
                            <tr>
                                <td style='width: 50%'>
                                    <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                                    <span style='color: #717171'>Enrollees: </span>
                                    </p>
                                </td>
                                <td style='text-align: right; width: 50%'>
                                    <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                                    <span style='color: #343d4c; text-transform: capitalize'
                                        >{enrolleesString} ({members.Count()})</span
                                    >
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td style='width: 50%'>
                                    <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                                    <span style='color: #717171'>Paid To: </span>
                                    </p>
                                </td>
                                <td style='text-align: right; width: 50%'>
                                    <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                                    <span style='color: #343d4c; text-transform: capitalize'
                                        >{coachName}</span
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
                                    <span style='color: #717171'>Payment Method: </span>
                                    </p>
                                </td>
                                <td style='text-align: right; width: 50%'>
                                    <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                                    <span style='color: #343d4c; text-transform: uppercase'
                                        >{paymentMethod}</span
                                    >
                                    </p>
                                </td>
                            </tr>
                            {amountHtmlString}
                            {addOnTotalHtmlString}
                            {paymentProviderHtmlString}
                            {serviceFeeHtmlString}
                            <tr>
                                <td style='width: 50%'>
                                    <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                                    <span style='color: #717171'>Applied Discount: </span>
                                    </p>
                                </td>
                                <td style='text-align: right; width: 50%'>
                                    <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                                    <span style='color: #343d4c; text-transform: uppercase'
                                        >{(discountAmount > 0 ? "-" : "")}{discountAmount.ToString("#,##0.00")}</span
                                    >
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td style='width: 50%'>
                                    <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                                    <span style='color: #717171'>Applied Credits: </span>
                                    </p>
                                </td>
                                <td style='text-align: right; width: 50%'>
                                    <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                                    <span style='color: #343d4c; text-transform: uppercase'
                                        >{GetCreditString(appliedCredits)}</span
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
                                        >PHP {GetTotalPurchase(amount, addOnsAmount, serviceFee, providerFee, discountAmount, appliedCredits).ToString("#,##0.00")}</b
                                        ></span
                                    >
                                    </p>
                                </td>
                            </tr>
                        </tbody>
                        </table>
                    </div>
                    <hr style='margin: 0; border: none; height: 1px; background-color: #d9d9d9' />
                    <div class='maker-details' style='padding: 2rem 2rem'>
                        <p style='font-size: 16px; margin: 0'>
                        <span style='color: #717171'>Experience By: </span>
                        <span style='color: #000'>
                            <u style='text-transform: capitalize'>{coachName}</u></span
                        >
                        </p>
                        <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                        <span style='color: #717171'>Email: </span>
                        <span style='color: #000'> <u>{makerEmail}</u></span>
                        </p>
                        <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                        <span style='color: #717171'>Contact No: </span>
                        <span style='color: #000'> <u>{coachNumber}</u></span>
                        </p>
                    </div>
                </div>
            ";
    }

    private string GetCreditString(decimal appliedCredits)
    {
        return appliedCredits > 0 ? "- " + appliedCredits.ToString("#,##0.00") : "0.00";
    }

    private decimal GetTotalPurchase(decimal amount, decimal addOnsAmount, decimal serviceFee, decimal providerFee, decimal discountAmount, decimal appliedCredits)
    {
        var result = amount + addOnsAmount + serviceFee + providerFee - discountAmount - appliedCredits;
        result = result < 0 ? 0 : result;
        return result;
    }
}