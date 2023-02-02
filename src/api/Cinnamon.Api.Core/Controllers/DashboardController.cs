using Cinnamon.Api.Core.Services.DashboardService.Handlers;
using Cinnamon.Framework.ApiCommand.ApiCore;
using Cinnamon.Framework.ApiCommand.ApiCore.Dashboard.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Core.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DashboardController : ControllerBase 
{
    private readonly IGetActivitySchedulesHandler getActivitySchedulesHandler;

    public DashboardController(IGetActivitySchedulesHandler getActivitySchedulesHandler)
    {
        this.getActivitySchedulesHandler = getActivitySchedulesHandler;
    }

    [Route("GetActivitySchedules")]
    [HttpGet]
    [ProducesResponseType(typeof(GetActivitySchedulesResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActivitySchedules()
    {
        try
        {
            var result = await getActivitySchedulesHandler.ExecuteAsync(new Services.DashboardService.Interactors.GetActivityScheduleArgs());
            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetActivitySchedulesResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }

            return new JsonResult(new GetActivitySchedulesResult 
                {
                    IsSuccess = true,
                    Result = result.Result.Schedules.Select(s => {
                        return new Framework.ApiCommand.ApiCore.DTO.Schedule.ActivityScheduleDTO {
                            ActivityId = s.ActivityId,
                            Description = s.Description,
                            ScheduleDescription = s.ScheduleDescription,
                            ScheduleId = s.ScheduleId,
                            ScheduleTitle = s.ScheduleTitle,
                            Title = s.Title,
                        };
                    }) 
                }
            );
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetActivitySchedulesResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }
}