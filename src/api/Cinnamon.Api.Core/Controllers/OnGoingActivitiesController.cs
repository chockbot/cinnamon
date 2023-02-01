using Cinnamon.Api.Core.Services.OnGoingActivityService.Handlers;
using Cinnamon.Framework.ApiCommand.ApiCore;
using Cinnamon.Framework.ApiCommand.ApiCore.DTO.Student;
using Cinnamon.Framework.ApiCommand.ApiCore.OnGoingActivities.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Core.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OnGoingActivitiesController : ControllerBase
{
    private readonly IGetAllOngoingActivitiesHandler getAllOngoingActivitiesHandler;
    private readonly IGetOngoingActivityByIdHandler getOngoingActivityByIdHandler;
    public OnGoingActivitiesController(IGetAllOngoingActivitiesHandler getAllOngoingActivitiesHandler, IGetOngoingActivityByIdHandler getOngoingActivityByIdHandler)
    {
        this.getAllOngoingActivitiesHandler = getAllOngoingActivitiesHandler;   
        this.getOngoingActivityByIdHandler  = getOngoingActivityByIdHandler;
    }

    [Route("GetAllOnGoingActivities")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllOngoingActivitiesResult), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllOngoingActivities()
    {
        try
        {
           var result= await getAllOngoingActivitiesHandler.ExecuteAsync(new Services.OnGoingActivityService.Interactors.GetAllOngoingActivitiesArgs{ });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllOngoingActivitiesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            return new JsonResult(new GetAllOngoingActivitiesResult
            {
                IsSuccess = true,
                Result = result.Result.Students.Select(e => {
                    return new Framework.ApiCommand.ApiCore.DTO.Student.StudentDTO
                    {
                        Id = e.Id,
                        Name = e.Name,
                        ActivityId= e.ActivityId,
                        CustomerId  = e.CustomerId,
                        NumberOfSessions= e.NumberOfSessions,   
                        Remarks = e.Remarks,    
                        ScheduleId = e.ScheduleId,
                        SessionsAttended = e.SessionsAttended,
                        Status = e.Status,
                        StudentNo = e.StudentNo
                    };
                })
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetAllOngoingActivitiesResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetOngoingActivity/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetOngoingActivityByIdResult), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> GetOngoingActivityById(int id)
    {
        try
        {
            var result = await getOngoingActivityByIdHandler.ExecuteAsync(new Services.OnGoingActivityService.Interactors.GetOngoingActivityByIdArgs
            {
                Id = id
            });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetOngoingActivityByIdResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            var objResult = result.Result;

            return new JsonResult(new GetOngoingActivityByIdResult
            {
                Result = new StudentDTO
                {
                    Id = objResult.Id,
                    ActivityId = objResult.ActivityId,
                    CustomerId = objResult.CustomerId,
                    Name = objResult.Name,
                    NumberOfSessions = objResult.NumberOfSessions,
                    Remarks = objResult.Remarks,
                    ScheduleId = objResult.ScheduleId,
                    SessionsAttended = objResult.SessionsAttended,
                    Status = objResult.Status,
                    StudentNo = objResult.StudentNo
                },
                IsSuccess = true,
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetOngoingActivityByIdResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}
