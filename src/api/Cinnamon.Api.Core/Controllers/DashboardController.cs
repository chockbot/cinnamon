using AutoMapper;
using Cinnamon.Api.Core.Services.DashboardService.Handlers;
using Cinnamon.Api.Core.Services.Disbursement.Handlers;
using Cinnamon.Framework.ApiCommand.ApiCore;
using Cinnamon.Framework.ApiCommand.ApiCore.Dashboard.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Dashboard.Response;
using Cinnamon.Framework.ApiCommand.ApiCore.DTO.Badges;
using Cinnamon.Framework.ApiCommand.ApiCore.DTO.StudentAttendance;
using Cinnamon.Framework.ApiCommand.ApiCore.DTO.DirectStudents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CoreDto = Cinnamon.Framework.ApiCommand.ApiCore.DTO;
using Cinnamon.Framework.ApiCommand.ApiCore.Activity.Response;

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
    private readonly IGetOTEByProviderHandler getOTEByProviderHandler;
    private readonly IGetOTEByActivityIdHandler getOTEByActivityIdHandler;
    private readonly IGetTicketDetailsHandler getTicketDetailsHandler;
    private readonly IUpdateOTETicketHandler updateOTETicketHandler;
    private readonly IGetDisbursementByProviderId getDisbursementByProviderId;
    private readonly IGetEnrolledStudentsByProviderHandler getEnrolledStudentsByProviderHandler;
    private readonly ICreateDirectStudentsHandler createDirectStudentsHandler;
    private readonly IMapper mapper;
    public DashboardController(IGetActivitySchedulesHandler getActivitySchedulesHandler, IGetCurrentDateAttendanceHandler getCurrentDateAttendanceHandler,
        IUpdateStudentAttendanceCurrentDateHandler updateStudentAttendanceHandler,IGetStudentAttendanceHandler getStudentAttendanceHandler, IGetAllStudentAttendanceByIdHandler getAllStudentAttendanceByIdHandler, 
        ICreateStudentAttendanceHandler createStudentAttendanceHandler,IUpdateAttendanceHandler updateAttendanceHandler, IGetAllBadgesHandler getAllBadgesHandler, IGetAllStudentsAttendanceHandler getAllStudentsAttendanceHandler,
        IGetCompletedStudentsHandler getCompletedStudentsHandler, IGetOTEByProviderHandler getOTEByProviderHandler, IGetOTEByActivityIdHandler getOTEByActivityIdHandler, IGetTicketDetailsHandler getTicketDetailsHandler,
        IUpdateOTETicketHandler updateOTETicketHandler, IGetDisbursementByProviderId getDisbursementByProviderId, IGetEnrolledStudentsByProviderHandler getEnrolledStudentsByProviderHandler, ICreateDirectStudentsHandler createDirectStudentsHandler, IMapper mapper)
    {
        this.getActivitySchedulesHandler          = getActivitySchedulesHandler;
        this.getCurrentDateAttendanceHandler      = getCurrentDateAttendanceHandler;
        this.updateStudentAttendanceHandler       = updateStudentAttendanceHandler;
        this.getStudentAttendanceHandler          = getStudentAttendanceHandler;
        this.getAllStudentAttendanceByIdHandler   = getAllStudentAttendanceByIdHandler;
        this.createStudentAttendanceHandler       = createStudentAttendanceHandler;
        this.updateAttendanceHandler              = updateAttendanceHandler;
        this.getAllBadgesHandler                  = getAllBadgesHandler;
        this.getAllStudentsAttendanceHandler      = getAllStudentsAttendanceHandler;
        this.getCompletedStudentsHandler          = getCompletedStudentsHandler;
        this.getOTEByProviderHandler              = getOTEByProviderHandler;
        this.getOTEByActivityIdHandler            = getOTEByActivityIdHandler;
        this.getTicketDetailsHandler              = getTicketDetailsHandler;
        this.updateOTETicketHandler               = updateOTETicketHandler;
        this.getDisbursementByProviderId          = getDisbursementByProviderId;
        this.getEnrolledStudentsByProviderHandler = getEnrolledStudentsByProviderHandler;
        this.createDirectStudentsHandler          = createDirectStudentsHandler;
        this.mapper = mapper;
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
                            ExpirationStartDate = s.ExpirationDateStart,
                            StudentType = s.StudentType
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
                        StudentId = s.StudentId,
                        StudentType = s.StudentType
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
    [Route("GetOTEByProvider")]
    [HttpGet]
    [ProducesResponseType(typeof(GetOTEByProviderResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOTEByProvider([FromQuery] GetOTEByProviderArgs args)
    {
        try
        {
            var result = await getOTEByProviderHandler.ExecuteAsync(new Services.DashboardService.Interactors.GetOTEByProviderArgs
            {
                Id = args.Id
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetOTEByProviderResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            return new JsonResult(new GetOTEByProviderResult
            {
                IsSuccess = true,
                Result = result.Result.OTEActivities.Select(s => {
                    return new Framework.ApiCommand.ApiCore.DTO.Activity.OteActivityDTO
                    {
                        Id               = s.Id,  
                        ExperienceTypeId = s.ExperienceTypeId,
                        EventName        = s.EventName,
                        Description      = s.Description,
                        Handler          = s.Handler,
                        CityName         = s.CityName,
                        RegionName       = s.RegionName,
                        EventImage       = s.EventImage,
                        PinnedLocation   = s.PinnedLocation,
                        ScheduleFrom     = s.ScheduleFrom,
                        ScheduleTo       = s.ScheduleTo,
                        Slots            = s.Slots,
                        Sold             = s.Sold,
                        Available        = s.Available
                    };
                })
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetOTEByProviderResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
    [Route("GetOTEByActivityId")]
    [HttpGet]
    [ProducesResponseType(typeof(GetOTEByActivityIdResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOTEByActivityId([FromQuery] GetOTEByActivityIdArgs args)
    {
        try
        {
            var result = await getOTEByActivityIdHandler.ExecuteAsync(new Services.DashboardService.Interactors.GetOTEByActivityIdArgs
            {
                SearchValue  = args.SearchValue ?? string.Empty,
                SearchBy     = args.SearchBy ?? 0,
                ActivityId   = args.ActivityId,
                CountPerPage = args.CountPerPage,
                PageIndex = args.PageIndex,
                DateId = args.DateId
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetOTEByActivityIdResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            return new JsonResult(new GetOTEByActivityIdResult
            {
                IsSuccess = true,
                ErrorInfo = result.Result.ErrorInfo,
                Pagination = result.Result.Pagination,
                Result = result.Result.OTEDetails.Select(s =>
                {
                    return new Framework.ApiCommand.ApiCore.DTO.Activity.OteTicketDTO
                    {
                        Id         = s.Id,
                        ActivityId = s.ActivityId,
                        Title      = s.Title,
                        Amount     = s.Amount,
                        QRCode     = s.QRCode,
                        Status     = s.Status,
                        Customer = new Framework.ApiCommand.ApiCore.DTO.Customer.CustomerDTO
                        {
                            FirstName = s.Customer.FirstName,
                            LastName = s.Customer.LastName,
                            Email = s.Customer.Email
                        }
                    };
                })
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetOTEByActivityIdResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
    [Route("GetTicketDetails")]
    [HttpGet]
    [ProducesResponseType(typeof(GetTicketDetailsResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTicketDetails([FromQuery] GetTicketDetailsArgs args)
    {
        try
        {
            var result = await getTicketDetailsHandler.ExecuteAsync(new Services.DashboardService.Interactors.GetTicketDetailsArgs
            {
                ActivityId = args.ActivityId,
                DateId = args.DateId
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetTicketDetailsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            return new JsonResult(new GetTicketDetailsResult
            {
                IsSuccess = true,
                Result = result.Result.OTETickets.Select(s => {
                    return new Framework.ApiCommand.ApiCore.DTO.Activity.OteScheduleDTO
                    {
                        ActivityId= s.ActivityId,
                        From = s.From,  
                        To = s.To,
                        Recurrences = s.Recurrences,
                        OtePricingDTO = new Framework.ApiCommand.ApiCore.DTO.Activity.OtePricingDTO
                        {
                            Id                    = s.OtePricingDTO.Id,
                            Name                  = s.OtePricingDTO.Name,
                            Description           = s.OtePricingDTO.Description,
                            MaxSlots              = s.OtePricingDTO.MaxSlots,
                            Sold                  = s.OtePricingDTO.Sold,
                            Available             = s.OtePricingDTO.Available,
                            Price                 = s.OtePricingDTO.Price,
                            OteSchedulePricingsId = s.OtePricingDTO.OteSchedulePricingsId
                        }
                    };
                })
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetOTEByProviderResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
    [Route("UpdateOTETicket")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateOTETicketResult), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> UpdateOTETicket([FromBody] UpdateOTETicketArgs args)
    {
        try
        {
            var result = await updateOTETicketHandler.ExecuteAsync(new Services.DashboardService.Interactors.UpdateOTETicketArgs
            {
                Id = args.Id,
                Status = args.Status
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateOTETicketResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            return new JsonResult(new UpdateOTETicketResult
            {
                Result = new Framework.ApiCommand.ApiCore.DTO.Activity.OteTicketDTO
                {
                    Status     = result.Result.Status,
                    Amount     = result.Result.Amount,
                    Title      = result.Result.Title,
                    QRCode     = result.Result.QRCode,
                    ActivityId = result.Result.ActivityId,
                },
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateOTETicketResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetDisbursementByProvider")]
    [HttpGet]
    [ProducesResponseType(typeof(GetDisbursementByProviderResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDisbursementByProvider([FromQuery] GetDisbursementByProviderArgs args)
    {
        try
        {
            var result = await getDisbursementByProviderId.ExecuteAsync(new Services.Disbursement.Interactors.GetDisbursementByProviderArgs
            {
                ProviderId   = args.ProviderId,
                FilterBy     = args.FilterBy ?? string.Empty,
                FilterValue  = args.FilterValue ?? string.Empty,
                CountPerPage = args.CountPerPage,
                PageIndex    = args.PageIndex,
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetDisbursementByProviderResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetDisbursementByProviderResult
            {
                IsSuccess  = true,
                ErrorInfo  = result.Result.ErrorInfo,
                Pagination = result.Result.Pagination,
                Result = result.Result.DisbursementInformation.Select(s => {
                    return new Framework.ApiCommand.ApiCore.DTO.Disbursement.DisbursementsInformationDTO
                    {
                        Id                = s.Id,
                        ProviderId        = s.ProviderId,
                        ProviderEmail     = s.ProviderEmail,
                        ProviderFirstName = s.ProviderFirstName,
                        ProviderLastName  = s.ProviderLastName,
                        CustomerName      = s.CustomerName,
                        Amount            = s.Amount,
                        Label             = s.Label,
                        Payload           = s.Payload,
                        PayoutDate        = s.PayoutDate,
                        Status            = s.Status,
                        Remarks           = s.Remarks,
                        InclusivePayment  = s.InclusivePayment,
                    };
                })
            }
            );
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetDisbursementByProviderResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetEnrolledStudentsByProvider")]
    [HttpGet]
    [ProducesResponseType(typeof(GetEnrolledStudentsByProviderResult), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> GetEnrolledStudentsByProvider([FromQuery] GetEnrolledStudentsByProviderArgs args)
    {
        try
        {
            var result = await getEnrolledStudentsByProviderHandler.ExecuteAsync(new Services.DashboardService.Interactors.GetEnrolledStudentsByProviderArgs
            {
                CountPerPage = args.CountPerPage,
                PageIndex    = args.PageIndex,
                ProviderId   = args.ProviderId,
                SearchBy     = args.SearchBy ?? 0,
                SearchValue  = args.SearchValue ?? string.Empty,
                ActivityId   = args.ActivityId ?? 0
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetEnrolledStudentsByProviderResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetEnrolledStudentsByProviderResult
            {
                IsSuccess = true,
                ErrorInfo = result.Result.ErrorInfo,
                Pagination = result.Result.Pagination,
                Result = result.Result.EnrolledStudentsList.Select(s =>
                {
                    return new Framework.ApiCommand.ApiCore.DTO.Student.StudentDTO
                    {
                        Id                  = s.Id,
                        ActivityId          = s.ActivityId,
                        Name                = s.Name,
                        Remarks             = s.Remarks,
                        ScheduleId          = s.ScheduleId,
                        SessionsAttended    = s.SessionsAttended,
                        NumberOfSessions    = s.NumberOfSessions,
                        StudentNo           = s.StudentNo,
                        ActivityName        = s.ActivityTitle,
                        ExpirationEndDate   = s.ExpirationEndDate,
                        ExpirationStartDate = s.ExpirationStartDate,
                        HasExpiration       = s.HasExpiration,
                        LastAttendance      = s.LastAttendance,
                        StudentType         = s.StudentType
                    };
                })
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetEnrolledStudentsByProviderResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CreateDirectStudents")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateDirectStudentsResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateDirectStudents([FromBody] CreateDirectStudentsArgs args)
    {
        try
        {
            var result = await createDirectStudentsHandler.ExecuteAsync(new Services.DashboardService.Interactors.CreateDirectStudentsArgs
            {
                CreateDirectStudents = args.CreateDirectStudents.Select(s =>
                {
                    return new Services.DashboardService.Interactors.CreateDirectStudentsArgs.CreateDirectStudent
                    {
                        CreateDirectStudentInfo = new Services.DashboardService.Interactors.CreateDirectStudentsArgs.CreateDirectStudentInfo
                        {
                            BirthMonth = s.CreateDirectStudentInfo.BirthMonth,
                            BirthYear  = s.CreateDirectStudentInfo.BirthYear,
                            Gender     = s.CreateDirectStudentInfo.Gender,
                            Name       = s.CreateDirectStudentInfo.Name,
                            Id         = s.CreateDirectStudentInfo.Id,
                            Email      = s.CreateDirectStudentInfo.Email
                        },
                        CreateDirectStudentSession = new Services.DashboardService.Interactors.CreateDirectStudentsArgs.CreateDirectStudentSession
                        {
                            ActivityId       = s.CreateDirectStudentSession.ActivityId,
                            Name             = s.CreateDirectStudentSession.Name,
                            Remarks          = s.CreateDirectStudentSession.Remarks ?? string.Empty,
                            ScheduleId       = s.CreateDirectStudentSession.ScheduleId,
                            Status           = s.CreateDirectStudentSession.Status,
                            Period           = s.CreateDirectStudentSession.Period
                        },
                        CreateDirectStudentPayment = new Services.DashboardService.Interactors.CreateDirectStudentsArgs.CreateDirectStudentPayment
                        {
                            Amount = s.CreateDirectStudentPayment.Amount,
                            PaymentDate = s.CreateDirectStudentPayment.PaymentDate
                        }
                    };
                }).ToList()
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreateDirectStudentsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            var mapResult = mapper.Map<IEnumerable<CoreDto.DirectStudents.DirectStudentsDTO>>(result.Result.CreateDirectStudents);

            return new JsonResult(new CreateDirectStudentsResult
            {
                IsSuccess = true,
                Result = mapResult
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateDirectStudentsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}