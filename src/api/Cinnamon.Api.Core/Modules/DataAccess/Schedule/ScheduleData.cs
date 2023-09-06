using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.Address.Response;
using Cinnamon.Framework.ApiCommand.ApiData.Schedule.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Schedule.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.Schedule
{
    public class ScheduleData : IScheduleData
    {
        private readonly IFlurlClient _flurlClient;
        
        public ScheduleData(IFlurlClientFactory flurlFac, ApplicationConfig config)
        {
            _flurlClient = flurlFac.Get(config.ApiDataUrl);
        }

        public async Task<AppResult<GetScheduleResult>> GetScheduleById(int id)
        {
            try
            {
                var result = await _flurlClient
                                .Request($"Schedule/GetScheduleById/{id}")
                                .GetJsonAsync<GetScheduleResult>();

                return AppResult<GetScheduleResult>.CreateSucceeded(result, "Successfully getting schedule by id api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<GetScheduleResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<GetScheduleResult>.CreateFailed(ex, "An error occured when getting schedule by id api");
            }
        }

        public async Task<AppResult<GetAllScheduleResult>> GetAllSchedules()
        {
            try
            {
                var result = await _flurlClient
                                .Request("Schedule/GetAllSchedules")
                                .GetJsonAsync<GetAllScheduleResult>();

                return AppResult<GetAllScheduleResult>.CreateSucceeded(result, "Successfully getting get all schedule api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<GetAllScheduleResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<GetAllScheduleResult>.CreateFailed(ex, "An error occured when getting all schedule api");
            }
        }

        public async Task<AppResult<CreateScheduleResult>> CreateSchedule(CreateScheduleArgs args)
        {
            try
            {
                var result = await _flurlClient
                                .Request("Schedule/CreateSchedule")
                                .PostJsonAsync(args)
                                .ReceiveJson<CreateScheduleResult>();

                return AppResult<CreateScheduleResult>.CreateSucceeded(result, "Successfully posting create schedule api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<CreateScheduleResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<CreateScheduleResult>.CreateFailed(ex, "An error occured when posting create customer api");
            }
        }

        public async Task<AppResult<UpdateScheduleResult>> UpdateSchedule(UpdateScheduleArgs args)
        {
            try
            {
                var result = await _flurlClient
                                .Request("Schedule/UpdateSchedule")
                                .PostJsonAsync(args)
                                .ReceiveJson<UpdateScheduleResult>();

                return AppResult<UpdateScheduleResult>.CreateSucceeded(result, "Successfully posting update schedule api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<UpdateScheduleResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<UpdateScheduleResult>.CreateFailed(ex, "An error occured when posting update schedule api");
            }
        }

        public async Task<AppResult<CreateManySchedulesResult>> CreateManySchedules(CreateManySchedulesArgs args)
        {
            try
            {
                var result = await _flurlClient
                                .Request("Schedule/CreateManySchedules")
                                .PostJsonAsync(args)
                                .ReceiveJson<CreateManySchedulesResult>();

                return AppResult<CreateManySchedulesResult>.CreateSucceeded(result, "Successfully posting create schedule api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<CreateManySchedulesResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<CreateManySchedulesResult>.CreateFailed(ex, "An error occured when posting create customer api");
            }
        }

        public async Task<AppResult<UpdateManySchedulesResult>> UpdateManySchedules(UpdateManySchedulesArgs args)
        {
            try
            {
                var result = await _flurlClient
                                .Request("Schedule/UpdateManySchedules")
                                .PostJsonAsync(args)
                                .ReceiveJson<UpdateManySchedulesResult>();

                return AppResult<UpdateManySchedulesResult>.CreateSucceeded(result, "Successfully posting update many schedule api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<UpdateManySchedulesResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<UpdateManySchedulesResult>.CreateFailed(ex, "An error occured when posting update many schedule api");
            }
        }

        public async Task<AppResult<DeleteManySchedulesResult>> DeleteManySchedules(DeleteManySchedulesArgs args)
        {
            try
            {
                var result = await _flurlClient
                                .Request("Schedule/DeleteManySchedules")
                                .PostJsonAsync(args)
                                .ReceiveJson<DeleteManySchedulesResult>();

                return AppResult<DeleteManySchedulesResult>.CreateSucceeded(result, "Successfully posting delete many schedule api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<DeleteManySchedulesResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<DeleteManySchedulesResult>.CreateFailed(ex, "An error occured when posting delete many schedule api");
            }
        }

        public async Task<AppResult<GetActivityScheduleTimesResult>> GetActivityScheduleTimes(GetActivityScheduleTimesArgs args)
        {
            try
            {
                var result = await _flurlClient
                            .Request("Schedule/GetActivityScheduleTimes")
                            .SetQueryParams(args)
                            .GetJsonAsync<GetActivityScheduleTimesResult>();

                return AppResult<GetActivityScheduleTimesResult>.CreateSucceeded(result, "Successfully posted get activity schedule times api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<GetActivityScheduleTimesResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<GetActivityScheduleTimesResult>.CreateFailed(ex, "An error occured when posting get activity schedule times api");
            }
        }

        public async Task<AppResult<CreateOngoingActivityScheduleResult>> CreateOngoingActivitySchedule(CreateOngoingActivityScheduleArgs args)
        {
            try
            {
                var result = await _flurlClient
                                .Request("Schedule/CreateOngoingActivitySchedule")
                                .PostJsonAsync(args)
                                .ReceiveJson<CreateOngoingActivityScheduleResult>();

                return AppResult<CreateOngoingActivityScheduleResult>.CreateSucceeded(result, "Successfully called create ongoing activity schedule api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<CreateOngoingActivityScheduleResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<CreateOngoingActivityScheduleResult>.CreateFailed(ex, "An error occured when calling create ongoing activity schedule api");
            }
        }
    }
}
