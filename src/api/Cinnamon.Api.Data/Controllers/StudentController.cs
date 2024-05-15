using Microsoft.AspNetCore.Mvc;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.Student.Response;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.Student.Request;

namespace Cinnamon.Api.Data.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentController : ControllerBase 
{
    private readonly IStudentRepository studentRepository;

    public StudentController(IStudentRepository studentRepository)
    {
        this.studentRepository = studentRepository;
    }

    [Route("GetStudentById/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetStudentResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudentById(int id)
    {
        try
        {
            var result = await studentRepository.GetByIdAsync(id);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetStudentResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetStudentResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetStudentResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetAllStudents")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllStudentResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllStudents([FromQuery] GetAllStudentArgs args)
    {
        try
        {
            var result =
                args.PageIndex.HasValue && args.CountPerPage.HasValue || args.ActivityId.HasValue || 
                    args.ScheduleId.HasValue || args.Status != null ?
                await studentRepository.GetAllAsync(args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage, 
                    args.ActivityId, args.ScheduleId, args.Status) :
                await studentRepository.GetAllAsync();

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllStudentResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            // get all without pagination to get all rows
            var all = args.PageIndex.HasValue && args.CountPerPage.HasValue || args.ActivityId.HasValue || 
                    args.ScheduleId.HasValue || args.Status != null ?
                await studentRepository.GetAllAsync(null, null, null, null, null) :
                await studentRepository.GetAllAsync();

            if (!all.Succeeded || all.Result == null)
            {
                return new JsonResult(new GetAllStudentResult { ErrorInfo = new ErrorInfo { Message = all.Message } });
            }

            var totalRecords = all.Result.Count();
            return new JsonResult(new GetAllStudentResult
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
            return new JsonResult(new GetAllStudentResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetEnrolledStudents/{ActivityId}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetEnrolledStudentsResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEnrolledStudents(int ActivityId)
    {
        try
        {
            var result = await studentRepository.GetEnrolledStudent(ActivityId);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetEnrolledStudentsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetEnrolledStudentsResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetEnrolledStudentsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CreateStudent")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateStudentResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateStudent([FromBody] CreateStudentArgs args)
    {
        try
        {
            var result = await studentRepository.Create(args.CustomerId, args.FamilyMemberId, 
                args.ActivityId, args.ScheduleId, args.Name, args.StudentNo, 
                args.NumberOfSessions, args.SessionsAttended, args.NumberOfBacktracking, args.ExpirationEndDate, args.ExpirationStartDate, 
                args.OngoingActivityId);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreateStudentResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateStudentResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateStudentResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CreateManyStudent")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateManyStudentResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateManyStudent([FromBody] CreateManyStudentArgs args)
    {
        try
        {
            var result = await studentRepository.Create(args.CustomerId, args.ActivityId, args.ScheduleId, args.NumberOfSessions, args.SessionsAttended, args.NumberOfBacktracking, args.ExpirationStartDate, args.ExpirationEndDate, args.Students.Select(s => {
                return new Framework.ApiCommand.ApiData.DTO.Student.CreateManyStudentDTO {
                    FamilyMemberId = s.FamilyMemberId,
                    Name = s.Name,
                    StudentNo = s.StudentNo
                };
            }), args.OngoingActivityId);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreateManyStudentResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateManyStudentResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateManyStudentResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UpdateStudent")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateStudentResult), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> UpdateStudent([FromBody] UpdateStudentArgs args)
    {
        try
        {
            var result = await studentRepository.Update(args.StudentId, args.Name, args.StudentNo, 
                args.NumberOfSessions, args.SessionsAttended, args.NumberOfBacktracking, args.Remarks, args.Status, args.ExpirationStartDate, args.ExpirationEndDate, args.HasReview);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateStudentResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateStudentResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateStudentResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetStudentsToDisburse")]
    [HttpGet]
    [ProducesResponseType(typeof(GetStudentsToDisburseResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudentsToDisburse([FromQuery] GetStudentsToDisburseArgs args)
    {
        try
        {
            var result = await studentRepository.GetStudentsToDisburse(args.IsInclusive ?? false, args.IsExpired ?? false);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetStudentsToDisburseResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetStudentsToDisburseResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetStudentsToDisburseResult
            {
                Result = result.Result,
                IsSuccess = true,
                Pagination = new()
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetStudentsToDisburseResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UpdateStudentsDisbursementStatus")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateStudentDisbursementStatusResult), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> UpdateStudentsDisbursementStatus([FromBody] UpdateStudentDisbursementArgs args)
    {
        try
        {
            var result = await studentRepository.UpdateStudentsDisbursementStatus(args.Ids, args.IsDisbursement);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateStudentDisbursementStatusResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateStudentDisbursementStatusResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateStudentDisbursementStatusResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetCompletedStudentsById")]
    [HttpGet]
    [ProducesResponseType(typeof(GetCompletedStudentsByIdResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCompletedStudentsById([FromQuery] GetCompletedStudentsByIdArgs args)
    {
        try
        {
            var result =
                args.PageIndex.HasValue && args.CountPerPage.HasValue || args.CustomerId != 0 ?
                await studentRepository.GetCompletedStudentsById(args.CustomerId ,args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage) :
                await studentRepository.GetAllAsync();

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetCompletedStudentsByIdResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            // get all without pagination to get all rows
            var all = args.PageIndex.HasValue && args.CountPerPage.HasValue || args.CustomerId != 0 ? 
                await studentRepository.GetCompletedStudentsById(0, null, null) :
                await studentRepository.GetAllAsync();

            if (!all.Succeeded || all.Result == null)
            {
                return new JsonResult(new GetCompletedStudentsByIdResult { ErrorInfo = new ErrorInfo { Message = all.Message } });
            }

            var totalRecords = all.Result.Count();
            return new JsonResult(new GetCompletedStudentsByIdResult
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
            return new JsonResult(new GetCompletedStudentsByIdResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetAllStudentsById")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllStudentsByIdResult), StatusCodes.Status200OK)]

    public async Task<IActionResult> GetAllStudentsById([FromQuery] GetAllStudentsByIdArgs args)
    {
        try
        {
            var result =
                args.PageIndex.HasValue && args.CountPerPage.HasValue || args.CustomerId != 0 ?
                await studentRepository.GetAllStudentsById(args.CustomerId, args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage) :
                await studentRepository.GetAllAsync();

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllStudentsByIdResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            // get all without pagination to get all rows
            var all = args.PageIndex.HasValue && args.CountPerPage.HasValue || args.CustomerId != 0 ?
                await studentRepository.GetAllStudentsById(0, null, null) :
                await studentRepository.GetAllAsync();

            if (!all.Succeeded || all.Result == null)
            {
                return new JsonResult(new GetAllStudentsByIdResult { ErrorInfo = new ErrorInfo { Message = all.Message } });
            }

            var totalRecords = all.Result.Count();
            return new JsonResult(new GetAllStudentsByIdResult
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
            return new JsonResult(new GetAllStudentsByIdResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetExpiringStudents")]
    [HttpGet]
    [ProducesResponseType(typeof(GetExpiringStudentsResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetExpiringStudents()
    {
        try
        {
            var result = await studentRepository.ExpiringStudents();

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetExpiringStudentsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetExpiringStudentsResult
            {
                Result = result.Result,
                IsSuccess = true,
                Pagination = new()
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetExpiringStudentsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetEnrolleeMasterList")]
    [HttpGet]
    [ProducesResponseType(typeof(GetEnrolleeMasterListResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEnrolleeMasterList([FromQuery] GetEnrolleeMasterListArgs args)
    {
        try
        {
            var result =
                args.PageIndex.HasValue && args.CountPerPage.HasValue || args.ProviderId != 0 ?
                await studentRepository.GetEnrolleeMasterList(args.ProviderId, args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage) :
                await studentRepository.GetAllAsync();

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetEnrolleeMasterListResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            // get all without pagination to get all rows
            var all = args.PageIndex.HasValue && args.CountPerPage.HasValue || args.ProviderId != 0 ?
                await studentRepository.GetEnrolleeMasterList(args.ProviderId, null, null) :
                await studentRepository.GetAllAsync();

            if (!all.Succeeded || all.Result == null)
            {
                return new JsonResult(new GetEnrolleeMasterListResult { ErrorInfo = new ErrorInfo { Message = all.Message } });
            }

            var totalRecords = all.Result.Count();
            return new JsonResult(new GetEnrolleeMasterListResult
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
            return new JsonResult(new GetEnrolleeMasterListResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetEnrolledStudentsByProvider")]
    [HttpGet]
    [ProducesResponseType(typeof(GetEnrolledStudentsResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEnrolledStudentsByProvider([FromQuery] GetEnrolledStudentsArgs args)
    {
        try
        {
            var result =
                args.PageIndex.HasValue && args.CountPerPage.HasValue || args.ProviderId != 0 ?
                await studentRepository.GetEnrolledStudentsByProvider(args.ProviderId, args.SearchValue ?? string.Empty, args.SearchBy ?? 0 , args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage) :
                await studentRepository.GetAllAsync();

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetEnrolledStudentsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            // get all without pagination to get all rows
            var all = args.PageIndex.HasValue && args.CountPerPage.HasValue || args.ProviderId != 0 ?
                await studentRepository.GetEnrolledStudentsByProvider(args.ProviderId,args.SearchValue ?? string.Empty , args.SearchBy ?? 0, null, null) :
                await studentRepository.GetAllAsync();

            if (!all.Succeeded || all.Result == null)
            {
                return new JsonResult(new GetEnrolledStudentsResult { ErrorInfo = new ErrorInfo { Message = all.Message } });
            }

            var totalRecords = all.Result.Count();
            return new JsonResult(new GetEnrolledStudentsResult
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
            return new JsonResult(new GetEnrolledStudentsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}