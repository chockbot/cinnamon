namespace Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification.Helpers;

public class OtePurchaseVerification 
{
    public string GetTemplate(TemplateArgs args)
    {
        string ticketsTemplate = string.Empty;

        string providerLogo = string.Empty;

        if(args.EventName.ToLower().Contains("jpark"))
        {
            providerLogo = $@"
                <img
                    src='https://stcinnamondev.blob.core.windows.net/upload-container/jpark-logo.png'
                    alt='logo'
                />
            ";
        }

        foreach(var item in args.Tickets)
        {
            ticketsTemplate += $@"
                <div style='display: flex; width: 100%; justify-content: space-between'>
                    <p
                        style='
                        color: #717171;
                        font-size: 16px;
                        margin-top: 0;
                        margin-bottom: 10px;
                        margin-right: 15px;
                        '
                    >
                        <span style='text-transform: capitalize;'>{item.TicketName}</span> x {item.TicketCount}
                    </p>
                    <p
                        style='
                        color: #717171;
                        font-size: 16px;
                        margin-top: 0;
                        margin-bottom: 10px;
                        '
                    >
                        P {item.TicketPrice.ToString("#,##0.00")}
                    </p>
                </div>
            ";
        }
        string ticketLinkText = args.EventLocation.ToLower() == "online" ? "View Link Here!" : "View Ticket Here!";

        string htmlDiscount = string.Empty;
        if(args.Discount.HasValue)
        {
            htmlDiscount += $@"
                <p
                    style='
                        color: #717171;
                        font-size: 16px;
                        margin-top: 0;
                        margin-bottom: 10px;
                        margin-bottom: 10px;
                    '
                    >
                    Discount: PHP {args.Discount.Value.ToString("#,##0.00")}
                </p>
            ";
        }

        return $@"
            <div
                style='
                    font-size: 16px;
                    border-radius: 15px;
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
                    class='cinnamon-ticket-view'
                    style='
                    background-color: #ffb84c;
                    padding-top: 35px;
                    padding-left: 25px;
                    padding-right: 25px;
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
                        font-size: 24px;
                        font-weight: 900;
                    '
                    >
                    Thank you for your purchase!
                    </p>
                    <p
                    style='
                        color: #343d4c;
                        font-size: 16px;
                        text-align: justify;
                        margin-bottom: 0;
                    '
                    >
                    Hi <span style='text-transform: capitalize;'>{args.CustomerName}</span>, we are excited to confirm your recent ticket purchase
                    for <span style='text-transform: capitalize;'>{args.EventName}</span> on {args.EventDate.ToString("MMMM dd, yyyy")}. Thank you for choosing to attend our
                    event! This email serves as your official confirmation.
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
                        href='{args.TicketDetailsLink}'
                        ><b>{ticketLinkText}</b></a
                    >
                    </div>
                </div>
                <div style='padding: 1.5rem; padding-bottom: 0.5rem' class='event-details'>
                    <p
                    style='
                        color: #0f173b;
                        font-size: 20px;
                        font-weight: 700;
                        margin-bottom: 10px;
                        margin-top: 0;
                        text-transform: capitalize;
                    '
                    >
                    {args.EventName}
                    </p>
                    <p
                    style='
                        color: #717171;
                        font-size: 16px;
                        margin-top: 0;
                        margin-bottom: 10px;
                    '
                    >
                    {args.EventLocation}
                    </p>
                    <p
                    style='
                        color: #717171;
                        font-size: 16px;
                        margin-top: 0;
                        margin-bottom: 10px;
                    '
                    >
                    {args.EventDate.ToString("MMMM dd, yyyy")} | {args.EventDate.ToString("hh:mm tt")}
                    </p>
                </div>
                <div style='padding: 1.5rem; padding-bottom: 0.5rem' class='ticket-details'>
                    <p
                    style='
                        color: #343d4c;
                        font-size: 16px;
                        font-weight: 700;
                        margin-top: 0;
                        margin-bottom: 15px;
                    '
                    >
                    Purchase Details
                    </p>
                    <p
                    style='
                        color: #717171;
                        font-size: 16px;
                        margin-top: 0;
                        margin-bottom: 10px;
                    '
                    >
                    Tickets:
                    </p>
                    {ticketsTemplate}
                </div>
                <div style='padding: 1.5rem; padding-bottom: 0.5rem' class='provider-details'>
                    <p
                    style='
                        color: #717171;
                        font-size: 16px;
                        margin-top: 0;
                        margin-bottom: 10px;
                        text-transform: capitalize;
                    '
                    >
                    Paid To : {args.ProviderName}
                    </p>
                    <p
                    style='
                        color: #717171;
                        font-size: 16px;
                        margin-top: 0;
                        margin-bottom: 10px;
                    '
                    >
                    Payment Reference Number : {args.ReferenceNumber}
                    </p>
                    <p
                    style='
                        color: #717171;
                        font-size: 16px;
                        margin-top: 0;
                        margin-bottom: 10px;
                    '
                    >
                    Payment Method: {args.PaymentMethod}
                    </p>
                    <p
                    style='
                        color: #717171;
                        font-size: 16px;
                        margin-top: 0;
                        margin-bottom: 10px;
                    '
                    >
                    Amount: PHP {args.SubTotal.ToString("#,##0.00")}
                    </p>
                    {htmlDiscount}
                    <p
                    style='
                        color: #717171;
                        font-size: 16px;
                        margin-top: 0;
                        margin-bottom: 10px;
                    '
                    >
                    Service Fee: PHP {args.ServiceFee.ToString("#,##0.00")}
                    </p>
                    <p
                    style='
                        color: #717171;
                        font-size: 16px;
                        margin-top: 0;
                        margin-bottom: 10px;
                    '
                    >
                    Handling Fee: PHP {args.HandlingFee.ToString("#,##0.00")}
                    </p>
                    <p
                    style='
                        font-weight: 700;
                        color: #0f173b;
                        font-size: 16px;
                        margin-top: 0;
                        margin-bottom: 10px;
                    '
                    >
                    Total Purchase: PHP {args.TotalAmount.ToString("#,##0.00")}
                    </p>
                </div>
                <hr style='margin: 0; border: none; height: 1px; background-color: #d9d9d9' />
                <div class='maker-details' 
                    style='
                        padding: 2rem 2rem;
                        display: flex;
                        align-items: center;
                        justify-content: space-between;'
                >
                    <div>
                        <p style='font-size: 16px; margin: 0'>
                        <span style='color: #717171'>Experience By: </span>
                        <span style='color: #000; text-transform: capitalize;'>
                            <u style='text-transform: capitalize'>{args.ProviderName}</u></span
                        >
                        </p>
                        <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                        <span style='color: #717171'>Email: </span>
                        <span style='color: #000'> <u>{args.ProviderEmail}</u></span>
                        </p>
                        <p style='font-size: 16px; margin: 0; margin-top: 1rem'>
                        <span style='color: #717171'>Contact No: </span>
                        <span style='color: #000'> <u>{args.ProviderNumber}</u></span>
                        </p>
                    </div>
                    {providerLogo}
                </div>
            </div>
        ";
    }

    public class TemplateArgs 
    {
        public string ProviderName {get; set;}
        public string ProviderEmail {get; set;}
        public string ProviderNumber {get; set;}

        public string CustomerName {get; set;}

        public string EventName {get; set;}
        public string EventLocation {get; set;}
        public DateTime EventDate {get; set;}
        public IEnumerable<TicketDetails> Tickets {get; set;}

        public string ReferenceNumber {get; set;}
        public string PaymentMethod {get; set;}
        public decimal SubTotal {get; set;}
        public decimal ServiceFee {get; set;}
        public decimal HandlingFee {get; set;}
        public decimal TotalAmount {get; set;}
        public decimal? Discount {get; set;}

        public string TicketDetailsLink {get; set;}
    }

    public class TicketDetails 
    {
        public string TicketName {get; set;}
        public int TicketCount {get; set;}
        public decimal TicketPrice {get; set;}
    }
}