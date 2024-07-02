using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteReminderFlag;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IOteReminderRepository 
{
    Task<AppResult<OteReminderFlagDTO>> CreateReminderFlag(OteReminderFlagDTO reminderFlag);

    Task<AppResult<IEnumerable<OteReminderFlagDTO>>> GetReminderFlags(int? activityId, int? dateId);

    Task<AppResult<IEnumerable<OteForReminderDTO>>> GetEventsForReminder();

    Task<AppResult<IEnumerable<CustomersNeedToRemindDTO>>> CustomersToRemind(int activityId, int oteDateId);

    Task<AppResult<IEnumerable<OteForReminderDTO>>> GetEventsForThankYou();
}