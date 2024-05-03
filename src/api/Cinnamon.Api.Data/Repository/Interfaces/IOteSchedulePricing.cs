using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.Common;


namespace Cinnamon.Api.Data.Repository.Interfaces;

public interface IOteSchedulePricing : IGenericEntity<OteSchedulePricing> 
{
    Task<AppResult<IEnumerable<OteSchedulePricing>>> GetPricings(int Id);
}