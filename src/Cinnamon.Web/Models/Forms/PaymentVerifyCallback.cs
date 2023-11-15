using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Web.Models.Forms;

public class PaymentVerifyCallback
{
    public DateTime? created { get; set; }
    public string? business_id { get; set; }
    public string? @event { get; set; }
    [Required]
    public Data data { get; set; }
    public object? api_version { get; set; }

    public class Account
    {
        public object? name { get; set; }
        public object? balance { get; set; }
        public object? point_balance { get; set; }
        public object? account_details { get; set; }
    }


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Card
    {
        public string? currency { get; set; }
        public CardInformation? card_information { get; set; }
        public ChannelProperties? channel_properties { get; set; }
        public object? card_verification_results { get; set; }
    }

    public class Ewallet
    {
        public Account? account { get; set; }
        public string? channel_code { get; set; }
        public ChannelProperties? channel_properties { get; set; }
    }

    public class CardInformation
    {
        public string? type { get; set; }
        public string? issuer { get; set; }
        public string? country { get; set; }
        public string? network { get; set; }
        public string? token_id { get; set; }
        public string? expiry_year { get; set; }
        public string? fingerprint { get; set; }
        public string? expiry_month { get; set; }
        public string? cardholder_name { get; set; }
        public string? masked_card_number { get; set; }
    }

    public class ChannelProperties
    {
        public object? cardonfile_type { get; set; }
        public string? failure_return_url { get; set; }
        public string? success_return_url { get; set; }
        public object? skip_three_d_secure { get; set; }
        public string? cancel_return_url { get; set; }
    }

    public class Data
    {
        [Required]
        public string id { get; set; }
        public object? items { get; set; }
        public int? amount { get; set; }
        [Required]
        public string status { get; set; }
        public string? country { get; set; }
        public string? created { get; set; }
        public string? updated { get; set; }
        public string? currency { get; set; }
        public object? metadata { get; set; }
        public object? customer_id { get; set; }
        public object? description { get; set; }
        public object? failure_code { get; set; }
        [Required]
        public string reference_id { get; set; }
        public object? payment_detail { get; set; }
        public PaymentMethod? payment_method { get; set; }
        public string? payment_request_id { get; set; }
    }

    public class PaymentMethod
    {
        public string? id { get; set; }
        public Card? card { get; set; }
        public string? type { get; set; }
        public string? status { get; set; }
        public DateTime? created { get; set; }
        public Ewallet? ewallet { get; set; }
        public object? qr_code { get; set; }
        public DateTime? updated { get; set; }
        public object? metadata { get; set; }
        public object? description { get; set; }
        public string? reusability { get; set; }
        public object? direct_debit { get; set; }
        public string? reference_id { get; set; }
        public object? virtual_account { get; set; }
        public object? over_the_counter { get; set; }
        public object? direct_bank_transfer { get; set; }
    }
}