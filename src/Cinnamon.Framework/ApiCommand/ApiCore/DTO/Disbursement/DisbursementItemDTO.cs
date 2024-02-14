namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.Disbursement;

public class DisbursementItemDTO
{
    public int Id {get; set;}
    public int DisbursementId {get; set;}
    public string Label {get; set;}
    public decimal Amount {get; set;}
}
