using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.Address.Response;
using Cinnamon.Framework.ApiCommand.ApiData.Schedule.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Schedule.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;

namespace Cinnamon.Api.Core.Modules.DataAccess.Schedule
{
    public class ScheduleData : IScheduleData
    {
        private readonly IFlurlClient _flurlClient;
        public ScheduleData(IFlurlClient flurlClient)
        {
            _flurlClient = flurlClient;
        }

        public async Task<AppResult<GetScheduleResult>> GetScheduleById(int id)
        {
            try
            {
                var result = await _flurlClient
                                .Request($"Schedule/GetScheduleById/{id}")
                                .GetJsonAsync<GetScheduleResult>();

                return AppResult<GetScheduleResult>.CreateSucceeded(result, "Successfully getting customer by id api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<GetScheduleResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<GetScheduleResult>.CreateFailed(ex, "An error occured when getting customer by id api");
            }
        }

        public async Task<AppResult<GetAllScheduleResult>> GetAllSchedules()
        {
            try
            {
                var result = await _flurlClient
                                .Request("Schedule/GetAllSchedules")
                                .GetJsonAsync<GetAllScheduleResult>();

                return AppResult<GetAllScheduleResult>.CreateSucceeded(result, "Successfully getting get all customers api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<GetAllScheduleResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<GetAllScheduleResult>.CreateFailed(ex, "An error occured when getting all customers api");
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

                return AppResult<CreateScheduleResult>.CreateSucceeded(result, "Successfully posting create customer api");
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

                return AppResult<UpdateScheduleResult>.CreateSucceeded(result, "Successfully posting update customer api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<UpdateScheduleResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<UpdateScheduleResult>.CreateFailed(ex, "An error occured when posting update customer api");
            }
        }
    }
}
