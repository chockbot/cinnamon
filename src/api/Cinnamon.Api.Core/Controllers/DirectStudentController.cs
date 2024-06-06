using AutoMapper;
using Cinnamon.Api.Core.Services.DirectStudentService.Handlers;
using Cinnamon.Framework.ApiCommand.ApiCore;
using Cinnamon.Framework.ApiCommand.ApiCore.DirectStudent.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.DirectStudent.Response;
using Cinnamon.Framework.ApiCommand.ApiCore.DTO.DirectStudents;
using CoreDto = Cinnamon.Framework.ApiCommand.ApiCore.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Core.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DirectStudentsController : ControllerBase 
{
    private readonly IDirectStudentsInfoHandler directStudentsInfoHandler;
    private readonly IUpdateDirectStudentHandler updateDirectStudentHandler;
    private readonly IDirectStudentHandler directStudentHandler;
    private readonly IGetDirectStudentByIdHandler getDirectStudentByIdHandler;
    private readonly ICreateDirectStudentAttendanceHandler createDirectStudentAttendanceHandler;
    private readonly IMapper mapper;

    public DirectStudentsController(IDirectStudentsInfoHandler directStudentsInfoHandler, IMapper mapper,
        IUpdateDirectStudentHandler updateDirectStudentHandler,IDirectStudentHandler directStudentHandler,
        IGetDirectStudentByIdHandler getDirectStudentByIdHandler,ICreateDirectStudentAttendanceHandler createDirectStudentAttendanceHandler)
    {
        this.directStudentsInfoHandler            = directStudentsInfoHandler;
        this.mapper                               = mapper;
        this.updateDirectStudentHandler           = updateDirectStudentHandler;
        this.directStudentHandler                 = directStudentHandler;
        this.getDirectStudentByIdHandler          = getDirectStudentByIdHandler;
        this.createDirectStudentAttendanceHandler = createDirectStudentAttendanceHandler;
    }

    [HttpGet]
    [ProducesResponseType(typeof(DirectStudentInfoReult), StatusCodes.Status200OK)]   
    public async Task<IActionResult> Index([FromQuery] DirectStudentInfoArgs args)
    {
        try
        {
            var result = await directStudentsInfoHandler.ExecuteAsync(new Services.DirectStudentService.Interactors.DirectStudentsInfosArgs {
                PageCount = args.CountPerPage,
                PageIndex = args.PageIndex,
                SearchName = args.SearchName
            });
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new DirectStudentInfoReult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }

            var mapped = mapper.Map<IEnumerable<DirectStudentInfoDTO>>(result.Result.DirectStudentInfos);

            return new JsonResult(new DirectStudentInfoReult {
                IsSuccess = true,
                Result = mapped
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new DirectStudentInfoReult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(DirectStudentResult), StatusCodes.Status200OK)]   
    public async Task<IActionResult> Index(int id)
    {
        try
        {
            var result = await directStudentHandler.ExecuteAsync(new Services.DirectStudentService.Interactors.DirectStudentArgs {
                StudentId = id
            });
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new DirectStudentResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }

            return new JsonResult(new DirectStudentResult {
                IsSuccess = true,
                Result = new DirectStudentDTO {
                    DirectStudentInfo = mapper.Map<DirectStudentInfoDTO>(result.Result.DirectStudentInfoResult),
                    DirectStudentPayment = mapper.Map<DirectStudentPaymentDTO>(result.Result.DirectStudentPaymentResult),
                    DirectStudentSession = mapper.Map<DirectStudentSessionDTO>(result.Result.DirectStudentSessionResult)
                }
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new DirectStudentResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("UpdateStudent")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateStudentResult), StatusCodes.Status200OK)]   
    public async Task<IActionResult> UpdateStudent([FromBody] UpdateStudentArgs args)
    {
        try
        {
            var result = await updateDirectStudentHandler.ExecuteAsync(new Services.DirectStudentService.Interactors.UpdateDirectStudentArgs {
                ActivityId       = args.ActivityId,
                Amount           = args.Amount,
                BirthMonth       = args.BirthMonth,
                BirthYear        = args.BirthYear,
                Gender           = args.Gender,
                Name             = args.Name,
                ScheduleId       = args.ScheduleId,
                StudentId        = args.StudentId,
                NumberOfSessions = args.NumberOfSessions,
                SessionsAttended = args.SessionsAttended,
                Remarks          = args.Remarks,
                StudentNo        = args.StudentNo,

            });
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new UpdateStudentResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }

            var mapped = mapper.Map<DirectStudentInfoDTO>(result.Result);

            return new JsonResult(new UpdateStudentResult {
                IsSuccess = true,
                Result = mapped
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateStudentResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [AllowAnonymous]
    [Route("GetStudentAttendanceById")]
    [HttpGet]
    [ProducesResponseType(typeof(GetDirectStudentByIdResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDirectStudentById([FromQuery] GetDirectStudentByIdArgs args)
    {
        try
        {
            var result = await getDirectStudentByIdHandler.ExecuteAsync(new Services.DirectStudentService.Interactors.GetDirectStudentByIdArgs
            {
                StudentId = args.StudentId,
                ActivityId = args.ActivityId,
                ScheduleId = args.ScheduleId
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetDirectStudentByIdResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetDirectStudentByIdResult
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
                    };
                })
            }
            );
        }
        catch (Exception)
        {

            throw;
        }
    }

    [AllowAnonymous]
    [Route("CreateDirectAttendance")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateDirectStudentAttendanceResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateAttendance([FromBody] CreateDirectStudentAttendanceArgs args)
    {
        try
        {
            var result = await createDirectStudentAttendanceHandler.ExecuteAsync(new Services.DirectStudentService.Interactors.CreateDirectStudentAttendanceArgs
            {
                CreateDirectStudentsAttendance = args.CreateStudentAttendances.Select(s =>
                {
                    return new Services.DirectStudentService.Interactors.CreateDirectStudentAttendanceArgs.CreateStudentAttendance
                    {
                        AttendanceDate = s.Date,
                        IsPresent      = s.IsPresent,
                        StudentId      = s.DirectStudentSessionId
                    };
                }).ToList()
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreateDirectStudentAttendanceResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            var mapResult = mapper.Map<IEnumerable<CoreDto.DirectStudents.DirectStudentAttendanceDTO>>(result.Result.CreateDirectStudentsAttendance);
            return new JsonResult(new CreateDirectStudentAttendanceResult
            {
                IsSuccess = true,
                Result = mapResult
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateDirectStudentAttendanceResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}