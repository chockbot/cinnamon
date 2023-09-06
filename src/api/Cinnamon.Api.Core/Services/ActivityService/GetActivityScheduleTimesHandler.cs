using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService
{
    public class GetActivityScheduleTimesHandler : IGetActivityScheduleTimesHandler
    {
        private readonly IScheduleData scheduleData;
        public GetActivityScheduleTimesHandler(IScheduleData scheduleData)
        {
            this.scheduleData = scheduleData;
        }
        public AppResult<GetActivityScheduleTimesResult> Execute(GetActivityScheduleTimesArgs args)
        {
            try
            {
                return ExecuteAsync(args).Result;
            }
            catch (Exception ex)
            {
                return AppResult<GetActivityScheduleTimesResult>.CreateFailed(ex, "An error occured in GetActivityScheduleTimesHandler");
            }
        }

        public async Task<AppResult<GetActivityScheduleTimesResult>> ExecuteAsync(GetActivityScheduleTimesArgs args)
        {
            try
            {
                var result = await scheduleData.GetActivityScheduleTimes(new Framework.ApiCommand.ApiData.Schedule.Request.GetActivityScheduleTimesArgs
                {
                    ActivityScheduleId = args.ActivityScheduleId,
                    DayOfWeek = args.DayOfWeek,
                    ScheduleDate = args.ScheduleDate
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<GetActivityScheduleTimesResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
                }

                if (result.Succeeded && !result.Result.IsSuccess)
                {
                    return AppResult<GetActivityScheduleTimesResult>.CreateFailed(
                        new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetActivityScheduleTimesResult");
                }

                return AppResult<GetActivityScheduleTimesResult>.CreateSucceeded(new GetActivityScheduleTimesResult
                {
                    ActivityScheduleTimes = result.Result.Result.ActivityScheduleTimes.Select(a => {
                        return new ActivityScheduleTime
                        {
                            DayOfWeek = a.DayOfWeek,
                            ActivityScheduleId = a.ActivityScheduleId,
                            ActivityScheduleTimeId = a.ActivityScheduleTimeId,
                            EndTime = a.EndTime,
                            IsAvailable = a.IsAvailable,
                            StartTime = a.StartTime
                        };
                    })
                }, "Successfully get activity schedule times");
            }
            catch (Exception ex)
            {
                return AppResult<GetActivityScheduleTimesResult>.CreateFailed(ex, "An error occured in GetActivityScheduleTimesHandler");
            }
        }
    }
}
