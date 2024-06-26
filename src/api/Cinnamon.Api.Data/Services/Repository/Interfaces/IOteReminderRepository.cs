using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteReminderFlag;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IOteReminderRepository 
{
    Task<AppResult<OteReminderFlagDTO>> CreateReminderFlag(OteReminderFlagDTO reminderFlag);

    Task<AppResult<OteReminderFlagDTO>> GetReminderFlag(int activityId, int dateId);
}