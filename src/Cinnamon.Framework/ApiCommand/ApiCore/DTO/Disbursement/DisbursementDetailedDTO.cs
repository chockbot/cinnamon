namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.Disbursement;

public class DisbursementDetailedDTO 
{
    public DisbursementsInformationDTO Disbursement {get; set;}
    public IEnumerable<DisbursementItemDTO> DisbursementItems {get; set;}
}