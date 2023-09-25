using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService
{
    public class CreateOngoingActivityScheduleHandler : ICreateOngoingActivityScheduleHandler
    {
        private readonly IScheduleData scheduleData;

        public CreateOngoingActivityScheduleHandler(IScheduleData scheduleData)
        {
            this.scheduleData = scheduleData;
        }
        public AppResult<CreateOngoingActivityScheduleResult> Execute(CreateOngoingActivityScheduleArgs args)
        {
            try
            {
                return ExecuteAsync(args).Result;
            }
            catch (Exception ex)
            {
                return AppResult<CreateOngoingActivityScheduleResult>.CreateFailed(ex, "An error occured in CreateOngoingActivityScheduleHandler");
            }
        }

        public async Task<AppResult<CreateOngoingActivityScheduleResult>> ExecuteAsync(CreateOngoingActivityScheduleArgs args)
        {
            try
            {
                var result = await scheduleData.CreateOngoingActivitySchedule(new Framework.ApiCommand.ApiData.Schedule.Request.CreateOngoingActivityScheduleArgs
                {
                    ActivityScheduleTimeId = args.ActivityScheduleTimeId,
                    IsCompleted = args.IsCompleted,
                    PurchaseOrderId = args.PurchaseOrderId,
                    ScheduleDate = args.ScheduleDate,
                    CreatedBy = args.CreatedBy
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<CreateOngoingActivityScheduleResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
                }

                if (result.Succeeded && !result.Result.IsSuccess)
                {
                    return AppResult<CreateOngoingActivityScheduleResult>.CreateFailed(
                        new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in CreateOngoingActivityScheduleHandler");
                }

                return AppResult<CreateOngoingActivityScheduleResult>.CreateSucceeded(new CreateOngoingActivityScheduleResult
                {
                    IsSuccess = result.Succeeded
                }, "Successfully created ongoing activity schedule");
            }
            catch (Exception ex)
            {
                return AppResult<CreateOngoingActivityScheduleResult>.CreateFailed(ex, "An error occured in CreateOngoingActivityScheduleHandler");
            }
        }
    }
}
