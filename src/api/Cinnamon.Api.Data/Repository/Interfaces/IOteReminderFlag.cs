using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteReminderFlag;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.Interfaces;

public interface IOteReminderFlag : IGenericEntity<OteReminderFlag>
{
    Task<AppResult<IEnumerable<OteForReminderDTO>>> GetEventsForReminder();
}
