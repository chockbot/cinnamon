using Cinnamon.Api.Core.Services.AccountService;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Handlers;
using Cinnamon.Framework.ApiCommand.ApiCore;
using Cinnamon.Framework.ApiCommand.ApiCore.Account.Response;
using Cinnamon.Framework.ApiCommand.ApiCore.DTO.Student;
using Cinnamon.Framework.ApiCommand.ApiCore.OnGoingActivities.Request;
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
    private readonly IUpdateOngoingActivityHadler updateOngoingActivityHadler;
    private readonly IAddActivityExpirationHandler addActivityExpirationHandler;
    private readonly IGetEnrolledStudentsHandler getEnrolledStudentsHandler;
    public OnGoingActivitiesController(IGetAllOngoingActivitiesHandler getAllOngoingActivitiesHandler, IGetOngoingActivityByIdHandler getOngoingActivityByIdHandler,
        IUpdateOngoingActivityHadler updateOngoingActivityHadler, IAddActivityExpirationHandler addActivityExpirationHandler, IGetEnrolledStudentsHandler getEnrolledStudentsHandler)
    {
        this.getAllOngoingActivitiesHandler = getAllOngoingActivitiesHandler;   
        this.getOngoingActivityByIdHandler  = getOngoingActivityByIdHandler;
        this.updateOngoingActivityHadler    = updateOngoingActivityHadler;
        this.addActivityExpirationHandler   = addActivityExpirationHandler;
        this.getEnrolledStudentsHandler     = getEnrolledStudentsHandler;
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
                        StudentNo = e.StudentNo,
                        ExpirationStartDate= e.ExpirationStartDate,
                        ExpirationEndDate= e.ExpirationEndDate,
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
                    StudentNo = objResult.StudentNo,
                    ExpirationStartDate = objResult.ExpirationStartDate,
                    ExpirationEndDate = objResult.ExpirationEndDate,
                },
                IsSuccess = true,
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetOngoingActivityByIdResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetEnrolledStudents/{activityId}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetEnrolledStudentsResult), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> GetEnrolledStudents(int activityId)
    {
        try
        {
            var result = await getEnrolledStudentsHandler.ExecuteAsync(new Services.OnGoingActivityService.Interactors.GetEnrolledStudentsArgs
            {
                ActivityId = activityId
            });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetEnrolledStudentsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            return new JsonResult(new GetEnrolledStudentsResult
            {
                IsSuccess = true,
                Result = result.Result.Students.Select(e => {
                    return new StudentDTO
                    {
                        Id = e.Id,
                        Name = e.Name,
                        ActivityId = e.ActivityId,
                        CustomerId = e.CustomerId,
                        NumberOfSessions = e.NumberOfSessions,
                        Remarks = e.Remarks,
                        ScheduleId = e.ScheduleId,
                        SessionsAttended = e.SessionsAttended,
                        Status = e.Status,
                        StudentNo = e.StudentNo,
                        ExpirationStartDate = e.ExpirationStartDate,
                        ExpirationEndDate = e.ExpirationEndDate,
                    };
                })
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetEnrolledStudentsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }


    [Route("UpdateOngoingActivity")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateOngoingActivityResult), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> UpdateFamilyMembers([FromBody] UpdateOngoingActivityArgs args)
    {
        try
        {
            var result = await updateOngoingActivityHadler.ExecuteAsync(new Services.OnGoingActivityService.Interactors.UpdateOngoingActivityArgs
            {
                Id= args.Id,
                Name= args.Name,
                NumberOfSessions= args.NumberOfSessions,
                Remarks= args.Remarks,
                SessionsAttended= args.SessionsAttended,
                Status= args.Status,
                StudentNo= args.StudentNo,
                ExpirationStartDate = args.ExpirationStartDate,
                ExpirationEndDate = args.ExpirationEndDate
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateOngoingActivityResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            return new JsonResult(new UpdateOngoingActivityResult
            {
                Result = new StudentDTO
                {
                   StudentNo= result.Result.StudentNo,
                   Status= result.Result.Status,
                   SessionsAttended = result.Result.SessionsAttended,
                   Remarks = result.Result.Remarks,
                   NumberOfSessions= result.Result.NumberOfSessions,
                   Name= result.Result.Name,
                   Id= result.Result.Id,
                   ExpirationStartDate= result.Result.ExpirationStartDate,
                   ExpirationEndDate= result.Result.ExpirationEndDate
                },
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateOngoingActivityResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("AddActivityExpiration")]
    [HttpPost]
    [ProducesResponseType(typeof(AddActivityExpirationResult), StatusCodes.Status202Accepted)]
    [AllowAnonymous]
    public async Task<IActionResult> AddActivityExpiration([FromBody] AddActivityExpirationArgs args)
    {
        try
        {
            var result = await addActivityExpirationHandler.ExecuteAsync(new Services.OnGoingActivityService.Interactors.AddActivityExpirationArgs
            {
                Id = args.Id,
                ScheduleId = args.ScheduleId,
                SessionName = args.SessionName,
                ExpirationStartDate = args.ExpirationStartDate,
                ExpirationEndDate = args.ExpirationEndDate
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new AddActivityExpirationResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            return new JsonResult(new AddActivityExpirationResult
            {
                Result = new StudentDTO
                {
                    Id = result.Result.Id,
                    ExpirationStartDate = result.Result.ExpirationStartDate,
                    ExpirationEndDate = result.Result.ExpirationEndDate
                },
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new AddActivityExpirationResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}
