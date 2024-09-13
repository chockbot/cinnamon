using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteWaitlist;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;
public interface IOteWaitlistRepository
{
    Task<AppResult<IEnumerable<OteWaitlistDTO>>> GetWaitlistByProvider(int? providerId, int? activityId, 
        IEnumerable<int>? status = null, int? oteDateId = null);
    Task<AppResult<IEnumerable<OteWaitlistDTO>>> GetAllAsync(int? count, int? skip);
    Task<AppResult<IEnumerable<OteWaitlistDTO>>> GetAllAsync();
    Task<AppResult<OteWaitlistDTO>> CreateOteWaitlist(OteWaitlistDTO oteWaitlistDTO);
    Task<AppResult<OteWaitlistDTO>> UpdateOteWaitlist(OteWaitlistDTO oteWaitlistDTO);
    Task<AppResult<bool>> DeleteOteWaitlist(int Id);
    Task<AppResult<OteWaitlistDTO>> GetWaitList(int id);
}
