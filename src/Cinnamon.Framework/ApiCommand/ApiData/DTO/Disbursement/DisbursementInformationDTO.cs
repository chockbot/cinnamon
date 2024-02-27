namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.Disbursement;

public class DisbursementInformationDTO
{
    public int Id {get; set;}
    public int ProviderId { get; set; }
    public string ProviderEmail {get; set;}
    public string ProviderFirstName {get; set;}
    public string ProviderLastName {get; set;}
    public string CustomerName { get; set; }
    public string Label {get; set;}
    public decimal Amount {get; set;}
    public string Status {get; set;}
    public string Remarks {get; set;}
    public bool InclusivePayment {get; set;}
    public DateTime CreatedDate { get; set;}
}