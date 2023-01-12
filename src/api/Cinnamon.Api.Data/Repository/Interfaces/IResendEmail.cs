using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.Interfaces;

public interface IResendEmail : IGenericEntity<ResendEmail>
{
    Task<AppResult<IEnumerable<ResendEmail>>> GetByEmailDateRange(string email, DateTime from, DateTime to);
}