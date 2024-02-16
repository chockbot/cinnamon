using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.Interfaces;

public interface IDisbursement : IGenericEntity<Disbursement> {
    Task<AppResult<IEnumerable<Disbursement>>> CreateDisbursements(IEnumerable<Disbursement> disbursements, 
        IEnumerable<int> studentIds);
}