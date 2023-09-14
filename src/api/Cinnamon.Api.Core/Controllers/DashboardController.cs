using Cinnamon.Api.Core.Services.DashboardService.Handlers;
using Cinnamon.Framework.ApiCommand.ApiCore;
using Cinnamon.Framework.ApiCommand.ApiCore.Dashboard.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Dashboard.Response;
using Cinnamon.Framework.ApiCommand.ApiCore.DTO.StudentAttendance;
using Cinnamon.Framework.ApiCommand.ApiCore.DTO.Badges;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Cinnamon.Framework.ApiCommand.ApiCore.OnGoingActivities.Response;
using Cinnamon.Framework.ApiCommand.ApiCore.OnGoingActivities.Request;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Handlers;

namespace Cinnamon.Api.Core.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DashboardController : ControllerBase 
{
    private readonly IGetActivitySchedulesHandler getActivitySchedulesHandler;
    private readonly IGetCurrentDateAttendanceHandler getCurrentDateAttendanceHandler;
    private readonly IUpdateStudentAttendanceCurrentDateHandler updateStudentAttendanceHandler;
    private readonly IGetStudentAttendanceHandler getStudentAttendanceHandler;
    private readonly IGetAllStudentAttendanceByIdHandler getAllStudentAttendanceByIdHandler;
    private readonly ICreateStudentAttendanceHandler createStudentAttendanceHandler;
    private readonly IUpdateAttendanceHandler updateAttendanceHandler;
    private readonly IGetAllBadgesHandler getAllBadgesHandler;
    private readonly IGetAllStudentsAttendanceHandler getAllStudentsAttendanceHandler;
    private readonly IGetCompletedStudentsHandler getCompletedStudentsHandler;
    

    public DashboardController(IGetActivitySchedulesHandler getActivitySchedulesHandler, IGetCurrentDateAttendanceHandler getCurrentDateAttendanceHandler,
        IUpdateStudentAttendanceCurrentDateHandler updateStudentAttendanceHandler,IGetStudentAttendanceHandler getStudentAttendanceHandler, IGetAllStudentAttendanceByIdHandler getAllStudentAttendanceByIdHandler, 
        ICreateStudentAttendanceHandler createStudentAttendanceHandler,IUpdateAttendanceHandler updateAttendanceHandler, IGetAllBadgesHandler getAllBadgesHandler, IGetAllStudentsAttendanceHandler getAllStudentsAttendanceHandler,
        IGetCompletedStudentsHandler getCompletedStudentsHandler)
    {
        this.getActivitySchedulesHandler = getActivitySchedulesHandler;
        this.getCurrentDateAttendanceHandler = getCurrentDateAttendanceHandler;
        this.updateStudentAttendanceHandler = updateStudentAttendanceHandler;
        this.getStudentAttendanceHandler = getStudentAttendanceHandler;
        this.getAllStudentAttendanceByIdHandler = getAllStudentAttendanceByIdHandler;
        this.createStudentAttendanceHandler = createStudentAttendanceHandler;
        this.updateAttendanceHandler = updateAttendanceHandler;
        this.getAllBadgesHandler = getAllBadgesHandler;
        this.getAllStudentsAttendanceHandler = getAllStudentsAttendanceHandler;
        this.getCompletedStudentsHandler = getCompletedStudentsHandler;
        

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
                            IsActiveSchedule = s.IsActiveSchedule,
                            IsSetSession = s.IsSetSession,
                            SessionName = s.SessionName,
                            HasExpiration = s.HasExpiration,
                            StartDate = s.StartDate
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

    [Route("GetCurrentAttendance")]
    [HttpGet]
    [ProducesResponseType(typeof(GetCurrentAttendanceResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCurrentAttendance([FromQuery] GetCurrentAttendanceArgs args)
    {
        try
        {
            var result = await getCurrentDateAttendanceHandler.ExecuteAsync(new Services.DashboardService.Interactors.GetCurrentDateAttendanceArgs {
                ActivityId = args.ActivityId,
                ScheduleId = args.ScheduleId
            });
            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetCurrentAttendanceResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }

            return new JsonResult(new GetCurrentAttendanceResult 
                {
                    IsSuccess = true,
                    Result = result.Result.StudentAttendaces.Select(s => {
                        return new Framework.ApiCommand.ApiCore.DTO.Student.StudentAttendanceDTO {
                            ActivityId = s.ActivityId,
                            Date = s.AttendanceDate,
                            Id = s.Id,
                            IsPresent = s.IsPresent,
                            Name = s.StudentName,
                            NumberOfSessions = s.NumberOfSessions,
                            Remarks = s.Remarks,
                            ScheduleId = s.ScheduleId,
                            SessionsAttended = s.SessionsAttended,
                            Status = s.Status,
                            StudentNo = s.StudentNo,
                            StudentId = s.StudentId,
                            ExpirationEndDate = s.ExpirationDateEnd,
                            ExpirationStartDate = s.ExpirationDateStart
                        };
                    })
                }
            );
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetCurrentAttendanceResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("UpdateStudentAttendances")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateStudentAttendanceResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateStudentAttendances([FromBody] UpdateStudentAttendnaceArgs args)
    {
        try
        {
            var result = await updateStudentAttendanceHandler.ExecuteAsync(new Services.DashboardService.Interactors.UpdateStudentAttedanceCurrentDateArgs {
                Students = args.Students.Select(s => {
                    return new Services.DashboardService.Interactors.UpdateStudentAttedanceCurrentDateArgs.StudentDetails {
                        ActivityId = s.ActivityId,
                        IsPresent = s.IsPresent,
                        ScheduleId = s.ScheduleId,
                        StudentId = s.StudentId
                    };
                })
            });
            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateStudentAttendanceResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }

            return new JsonResult(new UpdateStudentAttendanceResult 
                {
                    IsSuccess = true,
                    Result = result.Result.StudentAttendaces.Select(s => {
                        return new Framework.ApiCommand.ApiCore.DTO.Student.StudentAttendanceUpdateDTO {
                            ActivityId = s.ActivityId,
                            IsPresent = s.IsPresent,
                            ScheduleId = s.ScheduleId,
                            StudentId = s.StudentId
                        };
                    })
                }
            );
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateStudentAttendanceResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("GetStudentAttendance")]
    [HttpGet]
    [ProducesResponseType(typeof(GetStudentAttendanceResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudentAttendance([FromQuery] GetStudentAttendanceArgs args)
    {
        try
        {
            var result = await getStudentAttendanceHandler.ExecuteAsync(new Services.DashboardService.Interactors.GetStudentAttendanceArgs
            {
                ActivityId = args.ActivityId,
                ScheduleId = args.ScheduleId
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetStudentAttendanceResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetStudentAttendanceResult
            {
                IsSuccess = true,
                Result = result.Result.StudentAttendaces.Select(s => {
                    return new Framework.ApiCommand.ApiCore.DTO.Student.StudentAttendanceDTO
                    {
                        ActivityId = s.ActivityId,
                        Date = s.AttendanceDate,
                        Id = s.Id,
                        IsPresent = s.IsPresent,
                        Name = s.StudentName,
                        NumberOfSessions = s.NumberOfSessions,
                        Remarks = s.Remarks,
                        ScheduleId = s.ScheduleId,
                        SessionsAttended = s.SessionsAttended,
                        Status = s.Status,
                        StudentNo = s.StudentNo,
                        StudentId =s.StudentId,
                        ExpirationStartDate = s.ExpirationDateStart,
                        ExpirationEndDate = s.ExpirationDateEnd,
                    };
                })
            }
            );
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetStudentAttendanceResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetAllStudentAttendanceById")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllStudentAttendanceByIdResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllStudentAttendanceById([FromQuery] GetAllStudentAttendanceByIdArgs args)
    {
        try
        {
            var result = await getAllStudentAttendanceByIdHandler.ExecuteAsync(new Services.DashboardService.Interactors.GetAllStudentAttendanceByIdArgs
            {
                StudentId = args.StudentId,
                ActivityId = args.ActivityId,
                ScheduleId = args.ScheduleId
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllStudentAttendanceByIdResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetAllStudentAttendanceByIdResult
            {
                IsSuccess = true,
                Result = result.Result.StudentAttendaces.Select(s => {
                    return new Framework.ApiCommand.ApiCore.DTO.Student.StudentAttendanceDTO
                    {
                        ActivityId = s.ActivityId,
                        Date = s.AttendanceDate,
                        Id = s.Id,
                        IsPresent = s.IsPresent,
                        Name = s.StudentName,
                        NumberOfSessions = s.NumberOfSessions,
                        Remarks = s.Remarks,
                        ScheduleId = s.ScheduleId,
                        SessionsAttended = s.SessionsAttended,
                        Status = s.Status,
                        StudentNo = s.StudentNo,
                        StudentId = s.StudentId,
                        NumberOfBacktracking = s.NumberOfBackTracking
                    };
                })
            }
            );
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetAllStudentAttendanceByIdResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CreateStudentAttendance")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateStudentAttendanceResult), StatusCodes.Status201Created)]

    public async Task<IActionResult> CreateStudentAttendance([FromBody] CreateStudentAttendanceArgs args)
    {
        try
        {
            var result = await createStudentAttendanceHandler.ExecuteAsync(new Services.DashboardService.Interactors.CreateStudentAttendanceArgs
            {
                AttendanceDate= args.AttendanceDate,
                IsPresent= args.IsPresent,
                StudentId= args.StudentId,
            });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreateStudentAttendanceResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            return new JsonResult(new CreateStudentAttendanceResult
            {
                Result = new StudentAttendanceDTOs
                {
                  Date = result.Result.AttendanceDate,
                  IsPresent= result.Result.IsPresent,   
                  StudentId= result.Result.StudentId,
                },
                IsSuccess = true,
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateStudentAttendanceResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UpdateAttendance")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateAttendanceResult), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> UpdateAttendance([FromBody] UpdateAttendanceArgs args)
    {
        try
        {
            var result = await updateAttendanceHandler.ExecuteAsync(new Services.DashboardService.Interactors.UpdateAttendanceArgs
            {
               Id        = args.Id,
               IsPresent = args.IsPresent,
               Date      = args.Date,
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateAttendanceResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            return new JsonResult(new UpdateAttendanceResult
            {
                Result = new StudentAttendanceDTOs
                {
                    Id        = result.Result.Id,
                    Date      = result.Result.Date,
                    IsPresent = result.Result.IsPresent, 
                },
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateAttendanceResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetAllBadges")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllBadgesResult), StatusCodes.Status200OK)]
    [AllowAnonymous]

    public async Task<IActionResult> GetAllBadges()
    {
        try
        {
            var result = await getAllBadgesHandler.ExecuteAsync(new Services.DashboardService.Interactors.GetAllBadgeArgs { });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllBadgesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            return new JsonResult(new GetAllBadgesResult
            {
                IsSuccess = true,
                Result = result.Result.Badges.Select(e => {
                    return new BadgeDTO
                    {
                        Id      = e.Id,
                        Name    = e.Name,
                        Description = e.Description,
                        NumberOfStudent = e.NumberOfStudent,
                        NumberOfCompleted = e.NumberOfCompleted,
                        NumberOfReviews = e.NumberOfReviews,
                        ImgScr = e.ImgScr,
                    };
                })
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetAllBadgesResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetAllStudentsAttendance")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllStudentAttendanceResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllStudentAttendance([FromQuery] GetAllStudentAttendanceArgs args)
    {
        try
        {
            var result = await getAllStudentsAttendanceHandler.ExecuteAsync(new Services.DashboardService.Interactors.GetAllStudentsAttendanceArgs {
                ActivityId = args.ActivityId
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllStudentAttendanceResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            return new JsonResult(new GetAllStudentAttendanceResult
            {
                IsSuccess = true,
                Result = result.Result.StudentAttendaces.Select(s => {
                    return new Framework.ApiCommand.ApiCore.DTO.Student.StudentAttendanceDTO
                    {
                        Id = s.Id,
                        Date = s.AttendanceDate,
                        IsPresent = s.IsPresent,
                        NumberOfSessions = s.NumberOfSessions,
                        SessionsAttended = s.SessionsAttended,
                        StudentId = s.StudentId
                    };
                })
            }
            );
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetAllStudentAttendanceResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetCompletedStudents")]
    [HttpGet]
    [ProducesResponseType(typeof(GetCompletedStudentsResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCompletedStudents([FromQuery] GetCompletedStudetnsArgs args)
    {
        try
        {
            var result = await getCompletedStudentsHandler.ExecuteAsync(new Services.DashboardService.Interactors.GetCompletedStudentsArgs
            {
                ActivityId = args.ActivityIds
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetCompletedStudentsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            return new JsonResult(new GetCompletedStudentsResult
            {
                IsSuccess = true,
                Result = result.Result.StudentAttendaces.Select(s => {
                    return new Framework.ApiCommand.ApiCore.DTO.Student.StudentAttendanceDTO
                    {

                        ActivityId = s.ActivityId,
                        Date = s.AttendanceDate,
                        Id = s.Id,
                        IsPresent = s.IsPresent,
                        Name = s.StudentName,
                        NumberOfSessions = s.NumberOfSessions,
                        Remarks = s.Remarks,
                        ScheduleId = s.ScheduleId,
                        SessionsAttended = s.SessionsAttended,
                        Status = s.Status,
                        StudentNo = s.StudentNo,
                        StudentId = s.StudentId
                    };
                })
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetCompletedStudentsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}