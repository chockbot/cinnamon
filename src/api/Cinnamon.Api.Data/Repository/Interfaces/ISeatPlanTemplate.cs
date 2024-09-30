using System.Linq.Expressions;
using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.Interfaces;

public interface ISeatPlanTemplate : IGenericEntity<SeatPlanTemplate> 
{
    Task<AppResult<IEnumerable<SeatPlanTemplate>>> SeatPlanWithoutPayload(Expression<Func<SeatPlanTemplate, bool>> filter, int page, int limit);
}