using Cinnamon.Framework.ApiCommand.ApiData.Customer.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Customer.Response;
using Cinnamon.Framework.ApiCommand.ApiData.Schedule.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Schedule.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers
{
    public interface IScheduleData
    {
        Task<AppResult<GetScheduleResult>> GetScheduleById(int id);
        Task<AppResult<GetAllScheduleResult>> GetAllSchedules();
        Task<AppResult<CreateScheduleResult>> CreateSchedule(CreateScheduleArgs args);
        Task<AppResult<UpdateScheduleResult>> UpdateSchedule(UpdateScheduleArgs args);
        Task<AppResult<CreateManySchedulesResult>> CreateManySchedules(CreateManySchedulesArgs args);
        Task<AppResult<UpdateManySchedulesResult>> UpdateManySchedules(UpdateManySchedulesArgs args);
        Task<AppResult<DeleteManySchedulesResult>> DeleteManySchedules(DeleteManySchedulesArgs args);
        Task<AppResult<GetActivityScheduleTimesResult>> GetActivityScheduleTimes(GetActivityScheduleTimesArgs args);
        Task<AppResult<CreateOngoingActivityScheduleResult>> CreateOngoingActivitySchedule(CreateOngoingActivityScheduleArgs args);
    }
}
