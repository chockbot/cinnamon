using System.Globalization;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.StudentAttendance.Request;
using Cinnamon.Framework.ApiCommand.ApiData.StudentAttendance.Response;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Data.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentAttendanceController : ControllerBase 
{
    private readonly IStudentAttendanceRepository studentAttendanceRepository;

    public StudentAttendanceController(IStudentAttendanceRepository studentAttendanceRepository)
    {
        this.studentAttendanceRepository = studentAttendanceRepository;
    }

    [Route("GetStudentAttendanceById/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetStudentAttendanceResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudentById(int id)
    {
        try
        {
            var result = await studentAttendanceRepository.GetByIdAsync(id);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetStudentAttendanceResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetStudentAttendanceResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetStudentAttendanceResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetAllStudentAttendance")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllStudentAttendanceResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllStudentAttendance([FromQuery] GetAllStudentAttendanceArgs args)
    {
        try
        {
            DateOnly? date = null;
            if(!string.IsNullOrEmpty(args.Date))
            {
                date = DateOnly.ParseExact(args.Date, "yyyyMMdd", CultureInfo.InvariantCulture);
            }

            var result =
                args.PageIndex.HasValue && args.CountPerPage.HasValue || !string.IsNullOrEmpty(args.Date) || 
                    args.IsIncludeStudent.HasValue ?
                await studentAttendanceRepository.GetAllAsync(args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage, 
                    date, args.IsIncludeStudent) :
                await studentAttendanceRepository.GetAllAsync();

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllStudentAttendanceResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            // get all without pagination to get all rows
            var all = args.PageIndex.HasValue && args.CountPerPage.HasValue || !string.IsNullOrEmpty(args.Date) || 
                    args.IsIncludeStudent.HasValue ?
                await studentAttendanceRepository.GetAllAsync(null,null) :
                await studentAttendanceRepository.GetAllAsync();

            if (!all.Succeeded || all.Result == null)
            {
                return new JsonResult(new GetAllStudentAttendanceResult { ErrorInfo = new ErrorInfo { Message = all.Message } });
            }

            var totalRecords = all.Result.Count();
            return new JsonResult(new GetAllStudentAttendanceResult
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
            return new JsonResult(new GetAllStudentAttendanceResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
    
    [Route("CreateStudentAttendance")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateStudentAttendanceResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateStudentAttendance([FromBody] CreateStudentAttendaceArgs args)
    {
        try
        {
            var result = await studentAttendanceRepository.Create(args.StudentId, args.IsPresent, args.Date);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreateStudentAttendanceResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateStudentAttendanceResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateStudentAttendanceResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CreateManyStudent")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateManyStudentAttendanceResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateManyStudent([FromBody] CreateManyStudentAttendanceArgs args)
    {
        try
        {
            var result = await studentAttendanceRepository.Create(args.StudentAttendaces.Select(s => {
                return new Framework.ApiCommand.ApiData.DTO.StudentAttendance.CreateManyAttendanceDTO {
                    Date = s.Date,
                    IsPresent = s.IsPresent,
                    StudentId = s.StudentId
                };
            }));

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreateManyStudentAttendanceResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateManyStudentAttendanceResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateManyStudentAttendanceResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UpdateStudentAttendance")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateStudentAttendanceResult), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> UpdateStudentAttendance([FromBody] UpdateStudentAttendanceArgs args)
    {
        try
        {
            var result = await studentAttendanceRepository.Update(args.AttendanceId, args.IsPresent, args.Date);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateStudentAttendanceResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateStudentAttendanceResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateStudentAttendanceResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UpdateManyStudent")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateManyStudentAttendanceResult), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> UpdateManyStudent([FromBody] UpdateManyStudentAttendanceArgs args)
    {
        try
        {
            var result = await studentAttendanceRepository.Update(args.StudentAttendaces.Select(s => {
                return new Framework.ApiCommand.ApiData.DTO.StudentAttendance.UpdateManyStudentDTO {
                    Date = s.Date,
                    Id = s.AttendanceId,
                    IsPresent = s.IsPresent
                };
            }));

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateManyStudentAttendanceResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateManyStudentAttendanceResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateManyStudentAttendanceResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}