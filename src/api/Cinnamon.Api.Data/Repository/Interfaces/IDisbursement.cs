using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Disbursement;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.Interfaces;

public interface IDisbursement : IGenericEntity<Disbursement> {
    Task<AppResult<IEnumerable<Disbursement>>> CreateDisbursements(IEnumerable<Disbursement> disbursements, 
        IEnumerable<int> studentIds, IEnumerable<int> purchaseOrderIds);
    
    Task<AppResult<IEnumerable<DisbursementInformationDTO>>> GetDisbursementsInfo(string filterBy, string filterValue);
}