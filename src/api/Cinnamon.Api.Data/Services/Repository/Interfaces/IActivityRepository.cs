using Cinnamon.Framework.Common;
using Cinnamon.Api.Data.Services.Repository.Activity.DTO;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IActivityRepository
{
    Task<AppResult<ActivityDTO>> GetByIdAsync(int id);
    Task<AppResult<IEnumerable<ActivityDTO>>> GetAllAsync(bool? isActive, int? count, int? skip);
}