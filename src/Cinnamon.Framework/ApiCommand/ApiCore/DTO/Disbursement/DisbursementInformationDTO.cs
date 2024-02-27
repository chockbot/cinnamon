namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.Disbursement;

public class DisbursementsInformationDTO
{
    public int Id {get; set;}
    public string ProviderEmail {get; set;}
    public string ProviderFirstName {get; set;}
    public string ProviderLastName {get; set;}
    public string Label {get; set;}
    public decimal Amount {get; set;}
    public string Status {get; set;}
    public string Remarks {get; set;}
    public bool InclusivePayment {get; set;}
}
