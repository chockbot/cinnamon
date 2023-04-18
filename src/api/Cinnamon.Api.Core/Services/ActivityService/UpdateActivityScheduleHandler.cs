using System.Security.Claims;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;
using Ganss.XSS;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class UpdateActivityScheduleHandler : IUpdateActivityScheduleHandler
{
    private readonly IScheduleData scheduleData;
    private readonly IHttpContextAccessor httpContext;

    public UpdateActivityScheduleHandler(IScheduleData scheduleData, IHttpContextAccessor httpContext)
    {
        this.scheduleData = scheduleData;
        this.httpContext = httpContext;
    }

    public AppResult<UpdateScheduleResult> Execute(UpdateScheduleArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<UpdateScheduleResult>.CreateFailed(ex, "An error occured in UpdateActivityScheduleHandler");
        }
    }

    public async Task<AppResult<UpdateScheduleResult>> ExecuteAsync(UpdateScheduleArgs args)
    {
        try
        {
            var updateScheduleResult = await scheduleData.UpdateSchedule(new Framework.ApiCommand.ApiData.Schedule.Request.UpdateScheduleArgs
            {
                Id = args.Id,
                DateTime = args.DateTime,
                IsActiveSchedule = args.IsActiveSchedule,
                IsSetSession = args.IsSetSession,
                Name = args.Name,
                Order = args.Order,
                PerUnit1 = args.PerUnit1,
                PerUnit2 = args.PerUnit2,
                Price = args.Price,
                PriceUnit1 = args.PriceUnit1,
                PriceUnit2 = args.PriceUnit2,
                SessionName = args.SessionName,
                UnitPrice = args.UnitPrice
            });

            if (!updateScheduleResult.Succeeded || updateScheduleResult.Result == null)
            {
                return AppResult<UpdateScheduleResult>.CreateFailed(new ApplicationException(updateScheduleResult.Message), updateScheduleResult.Message);
            }
            if (updateScheduleResult.Succeeded && !updateScheduleResult.Result.IsSuccess)
            {
                return AppResult<UpdateScheduleResult>.CreateFailed(
                    new ApplicationException(updateScheduleResult.Result.ErrorInfo?.Message), "An error occured in UpdateActivityScheduleHandler");
            }

            var updated = updateScheduleResult.Result.Result;

            return AppResult<UpdateScheduleResult>.CreateSucceeded(new UpdateScheduleResult
            {
                Id               = updated.Id,
                DateTime         = updated.DateTime,
                IsActiveSchedule = updated.IsActiveSchedule,
                Name             = updated.Name,
                Order            = updated.Order,
                PerUnit1         = updated.PerUnit1,
                PerUnit2         = updated.PerUnit2,
                Price            = updated.Price,
                PriceUnit1       = updated.PriceUnit1,
                PriceUnit2       = updated.PriceUnit2,
                UnitPrice        = updated.UnitPrice
            }, "Successfully update activity schedule details");

        }
        catch (Exception ex)
        {
            return AppResult<UpdateScheduleResult>.CreateFailed(ex, "An error occured in UpdateActivityScheduleHandler");
        }
    }
    
}