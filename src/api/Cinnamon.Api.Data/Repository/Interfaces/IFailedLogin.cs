using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.Interfaces;

public interface IFailedLogin : IGenericEntity<FailedLogin>
{
    Task<AppResult<IEnumerable<FailedLogin>>> GetFailedLogins(string email, DateTime from, DateTime to);
}