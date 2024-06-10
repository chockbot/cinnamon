using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteDate;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IOteDateRepository 
{
    Task<AppResult<OteDateDTO>> GetOteDateById(int id);
    Task<AppResult<IEnumerable<OteDateDTO>>> GetOteDate (int activityId, DateTime? from, DateTime? to);

    Task<AppResult<OteDateDTO>> GetFirstOteDate (int activityId);
}