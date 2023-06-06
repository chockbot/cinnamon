using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Student;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.Reviews.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Reviews.Response;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Data.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly IReviewsRepository reviews;

    public ReviewController(IReviewsRepository reviews)
    {
        this.reviews = reviews;
    }
    [Route("GetReviewById/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetReviewResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        try
        {
            var result = await reviews.GetByIdAsync(id);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetReviewResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            return new JsonResult(new GetReviewResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetReviewResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetAllReviews")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllReviewsResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllReviews([FromQuery] GetAllReviewsArgs args)
    {
        try
        {
            var result =
               args.PageIndex.HasValue && args.CountPerPage.HasValue ?
               await reviews.GetAllAsync(args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage) :
               await reviews.GetAllAsync();
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllReviewsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            // get all without pagination to get all rows
            var all = args.PageIndex.HasValue && args.CountPerPage.HasValue ?
            await reviews.GetAllAsync(null, null) :
            await reviews.GetAllAsync();

            if (!all.Succeeded || all.Result == null)
            {
                return new JsonResult(new GetAllReviewsResult { ErrorInfo = new ErrorInfo { Message = all.Message } });
            }

            var totalRecords = all.Result.Count();
            return new JsonResult(new GetAllReviewsResult
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
            return new JsonResult(new GetAllReviewsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CreateReview")]
    [HttpPost]
    [ProducesResponseType(typeof(CreatedReviewResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCategory([FromBody] CreateReviewArgs args)
    {
        try
        {
            var result = await reviews.CreateExperienceCategoryAsync(args.CustomerId, args.MakerId, args.ActivityId, args.ScheduleId, args.StudentId, args.Rating, args.Review, args.ReviewDate);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreatedReviewResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreatedReviewResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreatedReviewResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UpdateReview")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdatedReviewResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCategory([FromBody] UpdateReviewArgs args)
    {
        try
        {
            var result = await reviews.UpdateExperienceCategoryAsync(args.Id, args.CustomerId, args.MakerId, args.ActivityId, args.ScheduleId, args.StudentId, args.Rating, args.Review, args.ReviewDate);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdatedReviewResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdatedReviewResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdatedReviewResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetReviewsByMakerId")]
    [HttpGet]
    [ProducesResponseType(typeof(GetReviewsByMakerIdResult), StatusCodes.Status200OK)]

    public async Task<IActionResult> GetReviewsById([FromQuery] GetReviewsByMakerIdArgs args)
    {
        try
        {
            var result =
                args.PageIndex.HasValue && args.CountPerPage.HasValue || args.MakerId != 0 ?
                await reviews.GetAllReviewsById(args.MakerId, args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage) :
                await reviews.GetAllAsync();

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetReviewsByMakerIdResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            // get all without pagination to get all rows
            var all = args.PageIndex.HasValue && args.CountPerPage.HasValue || args.MakerId != 0 ?
                await reviews.GetAllReviewsById(0, null, null) :
                await reviews.GetAllAsync();

            if (!all.Succeeded || all.Result == null)
            {
                return new JsonResult(new GetReviewsByMakerIdResult { ErrorInfo = new ErrorInfo { Message = all.Message } });
            }

            var totalRecords = all.Result.Count();
            return new JsonResult(new GetReviewsByMakerIdResult
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
            return new JsonResult(new GetReviewsByMakerIdResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}
