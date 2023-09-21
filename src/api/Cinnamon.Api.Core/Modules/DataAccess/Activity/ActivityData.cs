using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.Activity.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Activity.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.Activity;

public class ActivityData: IActivityData
{
	private readonly IFlurlClient flurlClient;
	public ActivityData(ApplicationConfig config, IFlurlClientFactory flurlFac)
	{
		flurlClient = flurlFac.Get(config.ApiDataUrl);
	}

	public async Task<AppResult<CreatedActivityResult>> CreateActivity(CreateActivityArgs args)
	{
		try
		{
			var result = await flurlClient
				.Request("Activity/CreateActivity")
				.PostJsonAsync(args)
				.ReceiveJson<CreatedActivityResult>();

			return AppResult<CreatedActivityResult>.CreateSucceeded(result, "Successfully posting create activity api");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<CreatedActivityResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<CreatedActivityResult>.CreateFailed(ex, "An error occured when posting create activity api");
		}
	}

	public async Task<AppResult<GetActivitiesByCategoriesResult>> GetActivitiesByCategories(int id, GetActivityArgs? args = null)
	{
		try
		{
			var result = await flurlClient
							.Request($"Activity/GetActivitiesByCategories/{id}")
							.SetQueryParams(args)
							.GetJsonAsync<GetActivitiesByCategoriesResult>();

			return AppResult<GetActivitiesByCategoriesResult>.CreateSucceeded(result, "Successfully getting activity by id api");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<GetActivitiesByCategoriesResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<GetActivitiesByCategoriesResult>.CreateFailed(ex, "An error occured when getting activity by id api");
		}
	}

	public async Task<AppResult<GetActivitiesBySubCategoriesResult>> GetActivitiesBySubCategories(int id, GetActivityArgs? args = null)
	{
		try
		{
			var result = await flurlClient
							.Request($"Activity/GetActivitiesBySubCategories/{id}")
							.SetQueryParams(args)
							.GetJsonAsync<GetActivitiesBySubCategoriesResult>();

			return AppResult<GetActivitiesBySubCategoriesResult>.CreateSucceeded(result, "Successfully getting activity by id api");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<GetActivitiesBySubCategoriesResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<GetActivitiesBySubCategoriesResult>.CreateFailed(ex, "An error occured when getting activity by id api");
		}
	}

	public async Task<AppResult<GetActivityResult>> GetActivityById(int id, GetActivityArgs? args = null)
	{
		try
		{
			var result = await flurlClient
							.Request($"Activity/GetActivityById/{id}")
							.SetQueryParams(args)
							.GetJsonAsync<GetActivityResult>();

			return AppResult<GetActivityResult>.CreateSucceeded(result, "Successfully getting activity by id api");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<GetActivityResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<GetActivityResult>.CreateFailed(ex, "An error occured when getting activity by id api");
		}
	}

	public async Task<AppResult<GetActivityResult>> GetActivityByHandler(string handler, GetActivityArgs? args = null)
	{
		try
		{
			var result = await flurlClient
							.Request($"Activity/GetActivityByHandler/{handler}")
							.SetQueryParams(args)
							.GetJsonAsync<GetActivityResult>();

			return AppResult<GetActivityResult>.CreateSucceeded(result, "Successfully getting activity by id api");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<GetActivityResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<GetActivityResult>.CreateFailed(ex, "An error occured when getting activity by id api");
		}
	}

	public async Task<AppResult<GetAllActivitiesResult>> GetAllActivities(GetAllActivities args)
	{
		try
		{
			var result = await flurlClient
							.Request("Activity/GetAllActivities")
							.SetQueryParams(args)
							.GetJsonAsync<GetAllActivitiesResult>();

			return AppResult<GetAllActivitiesResult>.CreateSucceeded(result, "Successfully getting get all activities api");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<GetAllActivitiesResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<GetAllActivitiesResult>.CreateFailed(ex, "An error occured when getting all activities api");
		}
	}

	public async Task<AppResult<UpdatedActivityResult>> UpdateActivity(UpdateActivity args)
	{
		try
		{
			var result = await flurlClient
							.Request("Activity/UpdateActivity")
							.PostJsonAsync(args)
							.ReceiveJson<UpdatedActivityResult>();

			return AppResult<UpdatedActivityResult>.CreateSucceeded(result, "Successfully posting update activity api");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<UpdatedActivityResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<UpdatedActivityResult>.CreateFailed(ex, "An error occured when posting update activity api");
		}
	}

	public async Task<AppResult<GetAllActivitiesResult>> GetPopularActivities(GetAllActivities args)
	{
		try
		{
			var result = await flurlClient
							.Request("Activity/Popular")
							.SetQueryParams(args)
							.GetJsonAsync<GetAllActivitiesResult>();

			return AppResult<GetAllActivitiesResult>.CreateSucceeded(result, "Successfully getting get all activities api");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<GetAllActivitiesResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<GetAllActivitiesResult>.CreateFailed(ex, "An error occured when getting all activities api");
		}
	}

	public async Task<AppResult<UpdatedActivityResult>> UpdateActivityGuid(UpdateActivity args)
	{
		try
		{
			var result = await flurlClient
							.Request("Activity/Guid/Update")
							.PostJsonAsync(args)
							.ReceiveJson<UpdatedActivityResult>();

			return AppResult<UpdatedActivityResult>.CreateSucceeded(result, "Successfully updated activity guids");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<UpdatedActivityResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<UpdatedActivityResult>.CreateFailed(ex, "An error occured when updating activity guids");
		}
	}

	public async Task<AppResult<DeleteActivityResult>> DeleteActivityById(DeleteActivityArgs args)
	{
		try
		{
			var result = await flurlClient
							.Request("Activity/Remove")
							.PostJsonAsync(args)
							.ReceiveJson<DeleteActivityResult>();

			return AppResult<DeleteActivityResult>.CreateSucceeded(result, "Successfully deleted activity");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<DeleteActivityResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<DeleteActivityResult>.CreateFailed(ex, "An error occured when deleting activity");
		}
	}

	public async Task<AppResult<RecommendedActivitiesResult>> RecommendedActivities(int primaryId, int count)
	{
		try
		{
			var result = await flurlClient
							.Request($"Activity/RecommendedActivities/{primaryId}/{count}")
							.GetJsonAsync<RecommendedActivitiesResult>();

			return AppResult<RecommendedActivitiesResult>.CreateSucceeded(result, "Successfully getting recommended activities api");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<RecommendedActivitiesResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<RecommendedActivitiesResult>.CreateFailed(ex, "An error occured when getting recommended activities api");
		}
	}

	public async Task<AppResult<PopularActivitiesResult>> PopularActivities(PopularActivitiesArgs args)
	{
		try
		{
			var result = await flurlClient
							.Request($"Activity/PopularActivities")
							.SetQueryParams(args)
							.GetJsonAsync<PopularActivitiesResult>();

			return AppResult<PopularActivitiesResult>.CreateSucceeded(result, "Successfully getting popular activities");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<PopularActivitiesResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<PopularActivitiesResult>.CreateFailed(ex, "An error occured when getting popular activities");
		}
	}

	public async Task<AppResult<CreateOteActivityResult>> CreateOteActivity(CreateOteActivityArgs args)
	{
		try
		{
			var result = await flurlClient
							.Request("Activity/CreateOteActivity")
							.PostJsonAsync(args)
							.ReceiveJson<CreateOteActivityResult>();

			return AppResult<CreateOteActivityResult>.CreateSucceeded(result, "One time event activity successfully created.");
		}
		catch (FlurlHttpException ex)
		{
			var error = ex.GetResponseJsonAsync();
			return AppResult<CreateOteActivityResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<CreateOteActivityResult>.CreateFailed(ex, "An error occured when creating One time event activity");
		}
	}

	public async Task<AppResult<UpdateOteActivityResult>> UpdateOteActivity(UpdateOteActivityArgs args)
	{
		try
		{
			var result = await flurlClient
							.Request("Activity/UpdateOteActivity")
							.PostJsonAsync(args)
							.ReceiveJson<UpdateOteActivityResult>();

			return AppResult<UpdateOteActivityResult>.CreateSucceeded(result, "One time event activity successfully created.");
		}
		catch (FlurlHttpException ex)
		{
			var error = ex.GetResponseJsonAsync();
			return AppResult<UpdateOteActivityResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<UpdateOteActivityResult>.CreateFailed(ex, "An error occured when creating One time event activity");
		}
	}

	public async Task<AppResult<GetOteActivityByHandlerResult>> GetOteActivityByHandler(GetOteActivityArgs args, string handler)
	{
		try
		{
			var result = await flurlClient
							.Request($"Activity/OteActivity/{handler}")
							.PostJsonAsync(args)
							.ReceiveJson<GetOteActivityByHandlerResult>();

			return AppResult<GetOteActivityByHandlerResult>.CreateSucceeded(result, "One time event activity successfully get.");
		}
		catch (FlurlHttpException ex)
		{
			var error = ex.GetResponseJsonAsync();
			return AppResult<GetOteActivityByHandlerResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<GetOteActivityByHandlerResult>.CreateFailed(ex, "An error occured when getting One time event activity");
		}
	}
}
