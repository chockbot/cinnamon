using System.Globalization;
using AutoMapper;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.DirectStudent.Request;
using Cinnamon.Framework.ApiCommand.ApiData.DirectStudent.Response;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.DirectStudent;
using Microsoft.AspNetCore.Mvc;

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
            if(!result.Succeeded || result.Result is null)
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
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new GetDirectStudentsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            // get all without pagination
            var all = await directStudentRepository.GetDirectStudents(null, null, args.ActivityId, args.ScheduleId, args.Status);
            if(!all.Succeeded || all.Result is null)
            {
                return new JsonResult(new GetDirectStudentsResult { ErrorInfo = new ErrorInfo { Message = all.Message } });
            }
            var totalRecords = all.Result.Count();

            return new JsonResult(new GetDirectStudentsResult { 
                IsSuccess = true, 
                Result = result.Result,
                Pagination = new Pagination {
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
    [Route("Attendance")]
    [ProducesResponseType(typeof(StudentAttendanceResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> StudentAttendance([FromQuery] StudentAttendanceArgs args)
    {
        try
        {
            DateTime? date = null;
            if(!string.IsNullOrEmpty(args.Date))
            {
                date = DateTime.ParseExact(args.Date, "yyyyMMdd", CultureInfo.InvariantCulture);
            }
            
            var result = await directStudentAttendanceRepository.GetAllAsync(
                args.CountPerPage, (args.PageIndex -1) * args.CountPerPage, date, args.IncludeStudent, 
                args.ActivityIds, args.ScheduleIds); 
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new StudentAttendanceResult {ErrorInfo = new ErrorInfo {Message = result.Message}});    
            }
            
            var totalCountRes = await directStudentAttendanceRepository
                .GetAllAsync(null, null, date, args.IncludeStudent, args.ActivityIds, args.ScheduleIds);
            if(!totalCountRes.Succeeded || totalCountRes.Result is null)
            {
                return new JsonResult(new StudentAttendanceResult {ErrorInfo = new ErrorInfo {Message = totalCountRes.Message}});    
            }

            var totalRecords = totalCountRes.Result.Count();
            return new JsonResult(new StudentAttendanceResult {
                Result = result.Result,
                IsSuccess = true,
                Pagination = new Pagination {
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
            return new JsonResult(new StudentAttendanceResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [HttpPost]
    [Route("Attendance/Bulk")]
    [ProducesResponseType(typeof(CreateStudentAttendanceResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateStudentAttendance([FromBody] CreateStudentAttendanceArgs args)
    {
        try
        {
            var dtos = mapper.Map<IEnumerable<DirectStudentAttendanceDTO>>(args.CreateStudentAttendances);

            var result = await directStudentAttendanceRepository.CreateDirectStudentAttendances(dtos);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new CreateStudentAttendanceResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateStudentAttendanceResult {
                IsSuccess = true,
                Result = result.Result
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateStudentAttendanceResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}