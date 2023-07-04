using Cinnamon.Api.Core.Services.OnGoingActivityService.Handlers;
using Cinnamon.Framework.ApiCommand.ApiCore;
using Cinnamon.Framework.ApiCommand.ApiCore.DTO.Reviews;
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
    private readonly IGetCompletedStudentsByIdHandler getCompletedStudentsByIdHandler;
    private readonly ICreateReviewHandler createReviewHandler;
    private readonly IGetAllStudentsByIdHandler getAllStudentsByIdHandler;
    private readonly IGetReviewsByMakerIdHandler getReviewsByMakerIdHandler;
    private readonly IGetReviewsByActivityIdHandler getReviewsByActivityIdHandler;
    private readonly IGetReviewsByCustomerIdHandler getReviewsByCustomerIdHandler;
    public OnGoingActivitiesController(IGetAllOngoingActivitiesHandler getAllOngoingActivitiesHandler, IGetOngoingActivityByIdHandler getOngoingActivityByIdHandler,
        IUpdateOngoingActivityHadler updateOngoingActivityHadler, IAddActivityExpirationHandler addActivityExpirationHandler, IGetEnrolledStudentsHandler getEnrolledStudentsHandler, 
        IGetCompletedStudentsByIdHandler getCompletedStudentsByIdHandler, ICreateReviewHandler createReviewHandler, IGetAllStudentsByIdHandler getAllStudentsByIdHandler,
        IGetReviewsByMakerIdHandler getReviewsByMakerIdHandler, IGetReviewsByActivityIdHandler getReviewsByActivityIdHandler, IGetReviewsByCustomerIdHandler getReviewsByCustomerIdHandler)
    {
        this.getAllOngoingActivitiesHandler   = getAllOngoingActivitiesHandler;   
        this.getOngoingActivityByIdHandler    = getOngoingActivityByIdHandler;
        this.updateOngoingActivityHadler      = updateOngoingActivityHadler;
        this.addActivityExpirationHandler     = addActivityExpirationHandler;
        this.getEnrolledStudentsHandler       = getEnrolledStudentsHandler;
        this.getCompletedStudentsByIdHandler  = getCompletedStudentsByIdHandler;
        this.createReviewHandler              = createReviewHandler;
        this.getAllStudentsByIdHandler        = getAllStudentsByIdHandler;
        this.getReviewsByMakerIdHandler       = getReviewsByMakerIdHandler;
        this.getReviewsByActivityIdHandler    = getReviewsByActivityIdHandler;
        this.getReviewsByCustomerIdHandler    = getReviewsByCustomerIdHandler;
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
                    NumberOfBacktracking = objResult.NumberOfBacktracking,
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
                NumberOfBacktracking = args.NumberOfBacktracking,
                Status= args.Status,
                StudentNo= args.StudentNo,
                ExpirationStartDate = args.ExpirationStartDate,
                ExpirationEndDate = args.ExpirationEndDate,
                HasReview = args.HasReview
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
                   NumberOfBacktracking = result.Result.NumberOfBacktracking,
                   Name= result.Result.Name,
                   Id= result.Result.Id,
                   ExpirationStartDate= result.Result.ExpirationStartDate,
                   ExpirationEndDate= result.Result.ExpirationEndDate,
                   HasReview = result.Result.HasReview
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

    [Route("GetCompletedStudentsById")]
    [HttpGet]
    [ProducesResponseType(typeof(GetCompletedStudentsByIdResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCompletedStudentsById([FromQuery] GetCompletedStudentsByIdArgs args)
    {
        try
        {
            var result = await getCompletedStudentsByIdHandler.ExecuteAsync(new Services.OnGoingActivityService.Interactors.GetCompletedStudentsByIdArgs
            {
                CustomerId = args.CustomerId,
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetCompletedStudentsByIdResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            return new JsonResult(new GetCompletedStudentsByIdResult
            {
                IsSuccess = true,
                Result = result.Result.Student.Select(s => {
                    return new Framework.ApiCommand.ApiCore.DTO.Student.StudentDTO
                    {
                        Id = s.StudentId,
                        ActivityId = s.ActivityId,
                        Name = s.StudentName,
                        NumberOfSessions = s.NumberOfSessions,
                        SessionsAttended = s.SessionsAttended,
                        ScheduleId = s.ScheduleId,
                        HasReview = s.HasReview
                    };
                })
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetCompletedStudentsByIdResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CreateReview")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateReviewResult), StatusCodes.Status201Created)]

    public async Task<IActionResult> CreateReview([FromBody] CreateReviewArgs args)
    {
        try
        {
            var result = await createReviewHandler.ExecuteAsync(new Services.OnGoingActivityService.Interactors.CreateReviewArgs
            {
                CustomerId  = args.CustomerId,
                MakerId     = args.MakerId,
                ActivityId  = args.ActivityId,
                ScheduleId  = args.ScheduleId,
                StudentId   = args.StudentId,
                Rating      = args.Rating,
                Review      = args.Review,
                ReviewDate  = args.ReviewDate
            });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreateReviewResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            return new JsonResult(new CreateReviewResult
            {
                Result = new ReviewsDTO
                {
                    CustomerId  = result.Result.CustomerId,
                    MakerId     = result.Result.MakerId,
                    ActivityId  = result.Result.ActivityId,
                    ScheduleId  = result.Result.ScheduleId,
                    StudentId   = result.Result.StudentId,
                    Rating      = result.Result.Rating,
                    Review      = result.Result.Review,
                    ReviewDate  = result.Result.ReviewDate

                },
                IsSuccess = true,
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateReviewResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetAllStudentsById")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllStudentsByIdResult), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllStudentsById([FromQuery] GetAllStudentsByIdArgs args)
    {
        try
        {
            var result = await getAllStudentsByIdHandler.ExecuteAsync(new Services.OnGoingActivityService.Interactors.GetAllStudentsByIdArgs
            {
                CustomerId = args.CustomerId,
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllStudentsByIdResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            return new JsonResult(new GetAllStudentsByIdResult
            {
                IsSuccess = true,
                Result = result.Result.Student.Select(e => {
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
                        HasReview = e.HasReview
                    };
                })
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetAllStudentsByIdResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetReviewsByMakerId")]
    [HttpGet]
    [ProducesResponseType(typeof(GetReviewsByMakerIdResult), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> GetReviewsByMakerId([FromQuery] GetReviewsByMakerIdArgs args)
    {
        try
        {
            var result = await getReviewsByMakerIdHandler.ExecuteAsync(new Services.OnGoingActivityService.Interactors.GetReviewsByMakerIdArgs
            {
                MakerId = args.MakerId,
                PageIndex = args.PageIndex,
                CountPerPage = args.CountPerPage,
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetReviewsByMakerIdResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            return new JsonResult(new GetReviewsByMakerIdResult
            {
                IsSuccess = true,
                Pagination = result.Result.Pagination,
                ErrorInfo = result.Result.ErrorInfo,
                Result = result.Result.Review.Select(s => {
                    return new Framework.ApiCommand.ApiCore.DTO.Reviews.ReviewsDTO
                    {
                        Id          = s.Id,
                        CustomerId  = s.CustomerId,
                        MakerId     = s.MakerId,
                        ActivityId  = s.ActivityId,
                        ScheduleId  = s.ScheduleId,
                        StudentId   = s.StudentId,
                        Rating      = s.Rating,
                        Review      = s.Review,
                        ReviewDate  = s.ReviewDate
                    };
                })
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetReviewsByMakerIdResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetReviewsByCustomerId")]
    [HttpGet]
    [ProducesResponseType(typeof(GetReviewsByCustomerIdResult), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> GetReviewsByCustomerId([FromQuery] GetReviewsByCustomerIdArgs args)
    {
        try
        {
            var result = await getReviewsByCustomerIdHandler.ExecuteAsync(new Services.OnGoingActivityService.Interactors.GetReviewsByCustomerIdArgs
            {
                CustomerId = args.CustomerId,
                PageIndex = args.PageIndex,
                CountPerPage = args.CountPerPage,
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetReviewsByCustomerIdResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            return new JsonResult(new GetReviewsByCustomerIdResult
            {
                IsSuccess = true,
                Pagination = result.Result.Pagination,
                ErrorInfo = result.Result.ErrorInfo,
                Result = result.Result.Review.Select(s => {
                    return new Framework.ApiCommand.ApiCore.DTO.Reviews.ReviewsDTO
                    {
                        Id = s.Id,
                        CustomerId = s.CustomerId,
                        MakerId = s.MakerId,
                        ActivityId = s.ActivityId,
                        ScheduleId = s.ScheduleId,
                        StudentId = s.StudentId,
                        Rating = s.Rating,
                        Review = s.Review,
                        ReviewDate = s.ReviewDate
                    };
                })
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetReviewsByCustomerIdResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetReviewsByActivityId")]
    [HttpGet]
    [ProducesResponseType(typeof(GetReviewsByActivityIdResult), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> GetReviewsByActivityId([FromQuery] GetReviewsByActivityIdArgs args)
    {
        try
        {
            var result = await getReviewsByActivityIdHandler.ExecuteAsync(new Services.OnGoingActivityService.Interactors.GetReviewsByActivityIdArgs
            {
                ActivityId = args.ActivityId,
                PageIndex = args.PageIndex,
                CountPerPage = args.CountPerPage,
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetReviewsByActivityIdResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            return new JsonResult(new GetReviewsByActivityIdResult
            {
                IsSuccess = true,
                Pagination = result.Result.Pagination,
                ErrorInfo = result.Result.ErrorInfo,
                Result = result.Result.Review.Select(s => {
                    return new Framework.ApiCommand.ApiCore.DTO.Reviews.ReviewsDTO
                    {
                        Id = s.Id,
                        CustomerId = s.CustomerId,
                        MakerId = s.MakerId,
                        ActivityId = s.ActivityId,
                        ScheduleId = s.ScheduleId,
                        StudentId = s.StudentId,
                        Rating = s.Rating,
                        Review = s.Review,
                        ReviewDate = s.ReviewDate
                    };
                })
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetReviewsByActivityIdResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}
