using AutoMapper;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.DirectStudent.Request;
using Cinnamon.Framework.ApiCommand.ApiData.DirectStudent.Response;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.DirectStudent;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Cinnamon.Api.Data.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DirectStudentController : ControllerBase
{
    private readonly IDirectStudentRepository directStudentRepository;
    private readonly IDirectStudentAttendanceRepository directStudentAttendanceRepository;
    private readonly IMapper mapper;

    public DirectStudentController(IDirectStudentRepository directStudentRepository,
        IDirectStudentAttendanceRepository directStudentAttendanceRepository, IMapper mapper)
    {
        this.directStudentRepository = directStudentRepository;
        this.directStudentAttendanceRepository = directStudentAttendanceRepository;
        this.mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateDirectStudentsResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDirectStudents([FromBody] CreateDirectStudentsArgs args)
    {
        try
        {
            var dtos = mapper.Map<IEnumerable<DirectStudentDTO>>(args.CreateDirectStudents);

            var result = await directStudentRepository.CreateDirectStudents(dtos);
            if (!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new CreateDirectStudentsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateDirectStudentsResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateDirectStudentsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetDirectStudentsResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetDirectStudents([FromQuery] GetDirectStudentsArgs args)
    {
        try
        {
            var result = await directStudentRepository.GetDirectStudents(args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage,
                args.ActivityId, args.ScheduleId, args.Status);
            if (!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new GetDirectStudentsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            // get all without pagination
            var all = await directStudentRepository.GetDirectStudents(null, null, args.ActivityId, args.ScheduleId, args.Status);
            if (!all.Succeeded || all.Result is null)
            {
                return new JsonResult(new GetDirectStudentsResult { ErrorInfo = new ErrorInfo { Message = all.Message } });
            }
            var totalRecords = all.Result.Count();

            return new JsonResult(new GetDirectStudentsResult
            {
                IsSuccess = true,
                Result = result.Result,
                Pagination = new Pagination
                {
                    PageIndex = args.PageIndex,
                    PerPage = args.CountPerPage,
                    TotalRecords = totalRecords,
                    TotalPages = args.CountPerPage.HasValue && args.PageIndex.HasValue ?
                        (int)Math.Ceiling((double)totalRecords / args.CountPerPage.Value) : null
                }
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetDirectStudentsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [HttpGet]
    [Route("Infos")]
    [ProducesResponseType(typeof(StudentInfosResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> StudentInfos([FromQuery] StudentInfosArgs args)
    {
        try
        {
            var result = await directStudentRepository.GetDirectStudentsInfo(args.ProviderId, args.SearchValue ?? string.Empty, args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new StudentInfosResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            var totalCountRes = await directStudentRepository
                .GetDirectStudentsInfo(args.ProviderId,args.SearchValue ?? string.Empty, null, null);
                
            if(!totalCountRes.Succeeded || totalCountRes.Result is null)
            {
                return new JsonResult(new StudentInfosResult { ErrorInfo = new ErrorInfo { Message = totalCountRes.Message } });
            }

            var totalRecords = totalCountRes.Result.Count();
            return new JsonResult(new StudentInfosResult
            {
                Result = result.Result,
                IsSuccess = true,
                Pagination = new Pagination
                {
                    PageIndex = args.PageIndex,
                    PerPage = args.CountPerPage,
                    TotalRecords = totalRecords,
                    TotalPages = args.CountPerPage.HasValue && args.PageIndex.HasValue ?
                                    (int)Math.Ceiling((double)totalRecords / args.CountPerPage.Value) : null
                }
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new StudentInfosResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [HttpGet]
    [Route("Infos/{id}")]
    [ProducesResponseType(typeof(DirectStudentInfoResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DirectStudentInfo(int id)
    {
        try
        {
            var result = await directStudentRepository.DirectStudentInfo(id);
            if (!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new DirectStudentInfoResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new DirectStudentInfoResult
            {
                Result = result.Result,
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new DirectStudentInfoResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UpdateDirectStudent")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateDirectStudentResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateDirectStudent([FromBody] UpdateDirectStudentArgs args)
    {
        try
        {
            var info = mapper.Map<DirectStudentInfoDTO>(args.UpdateStudentInfo);
            var sessions = mapper.Map<IEnumerable<DirectStudentSessionDTO>>(args.UpdateStudentSessions);

            var result = await directStudentRepository.UpdateDirectStudent(info, sessions);
            if (!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new UpdateDirectStudentResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateDirectStudentResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateDirectStudentResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [HttpGet]
    [Route("GetStudentAttendanceById")]
    [ProducesResponseType(typeof(GetDirectStudentByIdResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetStudentAttendanceById([FromQuery] GetDirectStudentByIdArgs args)
    {
        try
        {
            var result = await directStudentAttendanceRepository.GetStudentById(args.StudentId,
                args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage, args.IncludeStudent,
                args.ActivityIds, args.ScheduleIds);
            if (!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new GetDirectStudentByIdResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            var totalCountRes = await directStudentAttendanceRepository.GetStudentById(args.StudentId, null, null, args.IncludeStudent, args.ActivityIds, args.ScheduleIds);
            if (!totalCountRes.Succeeded || totalCountRes.Result is null)
            {
                return new JsonResult(new GetDirectStudentByIdResult { ErrorInfo = new ErrorInfo { Message = totalCountRes.Message } });
            }

            var totalRecords = totalCountRes.Result.Count();
            return new JsonResult(new GetDirectStudentByIdResult
            {
                Result = result.Result, 
                IsSuccess = true,
                Pagination = new Pagination
                {
                    PageIndex = args.PageIndex,
                    PerPage = args.CountPerPage,
                    TotalRecords = totalRecords,
                    TotalPages = args.CountPerPage.HasValue && args.PageIndex.HasValue ?
                                    (int)Math.Ceiling((double)totalRecords / args.CountPerPage.Value) : null
                }
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new StudentAttendanceResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [HttpGet]
    [Route("Attendance")]
    [ProducesResponseType(typeof(StudentAttendanceResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> StudentAttendance([FromQuery] StudentAttendanceArgs args)
    {
        try
        {
            DateTime? date = null;
            if (!string.IsNullOrEmpty(args.Date))
            {
                date = DateTime.ParseExact(args.Date, "yyyyMMdd", CultureInfo.InvariantCulture);
            }

            var result = await directStudentAttendanceRepository.GetAllAsync(
                args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage, date, args.IncludeStudent,
                args.ActivityIds, args.ScheduleIds);
            if (!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new StudentAttendanceResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            var totalCountRes = await directStudentAttendanceRepository
                .GetAllAsync(null, null, date, args.IncludeStudent, args.ActivityIds, args.ScheduleIds);
            if (!totalCountRes.Succeeded || totalCountRes.Result is null)
            {
                return new JsonResult(new StudentAttendanceResult { ErrorInfo = new ErrorInfo { Message = totalCountRes.Message } });
            }

            var totalRecords = totalCountRes.Result.Count();
            return new JsonResult(new StudentAttendanceResult
            {
                Result = result.Result,
                IsSuccess = true,
                Pagination = new Pagination
                {
                    PageIndex = args.PageIndex,
                    PerPage = args.CountPerPage,
                    TotalRecords = totalRecords,
                    TotalPages = args.CountPerPage.HasValue && args.PageIndex.HasValue ?
                                    (int)Math.Ceiling((double)totalRecords / args.CountPerPage.Value) : null
                }
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new StudentAttendanceResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [HttpPost]
    [Route("Attendance/Bulk")]
    [ProducesResponseType(typeof(CreateDirectStudentAttendanceResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateStudentAttendance([FromBody] CreateStudentAttendanceArgs args)
    {
        try
        {
            var dtos = mapper.Map<IEnumerable<DirectStudentAttendanceDTO>>(args.CreateStudentAttendances);

            var result = await directStudentAttendanceRepository.CreateDirectStudentAttendances(dtos);
            if (!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new CreateDirectStudentAttendanceResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateDirectStudentAttendanceResult
            {
                IsSuccess = true,
                Result = result.Result
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateDirectStudentAttendanceResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [HttpPost]
    [Route("Attendance/Update/Bulk")]
    [ProducesResponseType(typeof(UpdateStudentAttendanceBulkResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateStudentAttendance([FromBody] UpdateStudentAttendanceBulkArgs args)
    {
        try
        {
            var dtos = mapper.Map<IEnumerable<DirectStudentAttendanceDTO>>(args.StudentAttendances);

            var result = await directStudentAttendanceRepository.UpdateStudentAttendances(dtos, args.Date);
            if (!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new UpdateStudentAttendanceBulkResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateStudentAttendanceBulkResult
            {
                IsSuccess = true,
                Result = result.Result
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateStudentAttendanceBulkResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [HttpGet]
    [Route("GetDirectStudentsPayment")]
    [ProducesResponseType(typeof(GetDirectStudentsPaymentResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetDirectStudentsPayment([FromQuery] GetDirectStudentsPaymentArgs args)
    {
        try
        {
            DateTime From = DateTime.ParseExact(args.DateFrom, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

            var result = await directStudentRepository.GetStudentPaymentByProvider(args.ProviderId, From);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetDirectStudentsPaymentResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetDirectStudentsPaymentResult
            {
                Result = result.Result,
                IsSuccess = true,
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetDirectStudentsPaymentResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [HttpGet]
    [Route("Sessions")]
    [ProducesResponseType(typeof(StudentSessionsResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> StudentSessions([FromQuery] StudentSessionsArgs args)
    {
        try
        {
            bool ongoing = args.SessionStatus?.ToLower() == "ongoing";
            bool completed = args.SessionStatus?.ToLower() == "completed";

            var result = await directStudentRepository.StudentSessions(args.StudentId, ongoing, completed);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new StudentSessionsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new StudentSessionsResult
            {
                Result = result.Result,
                IsSuccess = true,
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new StudentSessionsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [HttpGet]
    [Route("Sessions/{id}")]
    [ProducesResponseType(typeof(StudentSessionResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> StudentSession(int id)
    {
        try
        {
            var result = await directStudentRepository.StudentSession(id);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new StudentSessionResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new StudentSessionResult
            {
                Result = result.Result,
                IsSuccess = true,
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new StudentSessionResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [HttpGet]
    [Route("ExpiringSessions")]
    [ProducesResponseType(typeof(ExpiringSessionsResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ExpiringSessions()
    {
        try
        {
            var result = await directStudentRepository.ExpiringStudents();
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new ExpiringSessionsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new ExpiringSessionsResult
            {
                Result = result.Result,
                IsSuccess = true,
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new ExpiringSessionsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}