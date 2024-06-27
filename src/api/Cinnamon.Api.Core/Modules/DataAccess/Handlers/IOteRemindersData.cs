using Cinnamon.Framework.ApiCommand.ApiData.OteReminderFlags.Request;
using Cinnamon.Framework.ApiCommand.ApiData.OteReminderFlags.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface IOteRemindersData 
{
    Task<AppResult<GetReminderFlagsResult>> GetReminderFlags(GetReminderFlagsArgs args);

    Task<AppResult<CreateReminderFlagResult>> CreateReminderFlag(CreateReminderFlagArgs args);

    Task<AppResult<GetEventsForReminderResult>> GetEventsForReminder();
}