using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.Interfaces;

public interface IPayoutLog : IGenericEntity<PayoutLog> 
{
    Task<AppResult<IEnumerable<PayoutLog>>> GetPayoutByProvider(int? Id, DateTime? dateFrom, int Status);
}