using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.OteReminderFlags.Request;
using Cinnamon.Framework.ApiCommand.ApiData.OteReminderFlags.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.OteReminders;

public class OteRemindersData : IOteRemindersData
{
    private readonly IFlurlClient flurlClient;

	public OteRemindersData(ApplicationConfig config, IFlurlClientFactory flurlFac)
	{
		flurlClient = flurlFac.Get(config.ApiDataUrl);
	}

    public async Task<AppResult<GetReminderFlagsResult>> GetReminderFlags(GetReminderFlagsArgs args)
    {
        try
		{
			var result = await flurlClient
							.Request($"OteReminders/")
                            .SetQueryParams(args)
							.GetJsonAsync<GetReminderFlagsResult>();

			return AppResult<GetReminderFlagsResult>.CreateSucceeded(result, "Successfully getting events reminder flags.");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<GetReminderFlagsResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<GetReminderFlagsResult>.CreateFailed(ex, "An error occured when getting events reminder flags.");
		}
    }

    public async Task<AppResult<CreateReminderFlagResult>> CreateReminderFlag(CreateReminderFlagArgs args)
	{
		try
		{
			var result = await flurlClient
				.Request("OteReminders")
				.PostJsonAsync(args)
				.ReceiveJson<CreateReminderFlagResult>();

			return AppResult<CreateReminderFlagResult>.CreateSucceeded(result, "Successfully posting create ote reminder flag.");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<CreateReminderFlagResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<CreateReminderFlagResult>.CreateFailed(ex, "An error occured when posting create ote reminder flag.");
		}
	}

    public async Task<AppResult<GetEventsForReminderResult>> GetEventsForReminder()
    {
        try
		{
			var result = await flurlClient
							.Request($"OteReminders/EventsForReminders")
							.GetJsonAsync<GetEventsForReminderResult>();

			return AppResult<GetEventsForReminderResult>.CreateSucceeded(result, "Successfully getting events for reminder.");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<GetEventsForReminderResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<GetEventsForReminderResult>.CreateFailed(ex, "An error occured when getting events for reminder.");
		}
    }
}