using Microsoft.AspNetCore.Mvc;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.OngoingActivity.Response;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.OngoingActivity.Request;

namespace Cinnamon.Api.Data.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OngoingActivityController : ControllerBase 
{
    private readonly IOngoingActivityRepository ongoingActivityRepository;

    public OngoingActivityController(IOngoingActivityRepository ongoingActivityRepository)
    {
        this.ongoingActivityRepository = ongoingActivityRepository;
    }

    [Route("GetOngoingActivityById/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetOngoingActivityResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOngoingActivityById(int id)
    {
        try
        {
            var result = await ongoingActivityRepository.GetByIdAsync(id);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetOngoingActivityResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetOngoingActivityResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetOngoingActivityResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetAllOngoingActivities")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllOngoingActivityResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllOngoingActivities([FromQuery] GetAllOngoingActivityArgs args)
    {
        try
        {
            var result =
                args.PageIndex.HasValue && args.CountPerPage.HasValue || args.CustomerId.HasValue ?
                await ongoingActivityRepository.GetAllAsync(args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage, 
                    args.CustomerId, args.IsIncludeActivity ?? false) :
                await ongoingActivityRepository.GetAllAsync();

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllOngoingActivityResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            // get all without pagination to get all rows
            var all = args.PageIndex.HasValue && args.CountPerPage.HasValue ?
                await ongoingActivityRepository.GetAllAsync(null, null, null) :
                await ongoingActivityRepository.GetAllAsync();

            if (!all.Succeeded || all.Result == null)
            {
                return new JsonResult(new GetAllOngoingActivityResult { ErrorInfo = new ErrorInfo { Message = all.Message } });
            }

            var totalRecords = all.Result.Count();
            return new JsonResult(new GetAllOngoingActivityResult
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
            return new JsonResult(new GetAllOngoingActivityResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CreateOngoingActivity")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateOngoingActivityResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateOngoingActivity([FromBody] CreateOngoingActivityArgs args)
    {
        try
        {
            var result = await ongoingActivityRepository.Create(args.ActivityId, args.CustomerId, args.ScheduleId, args.PurchaseOrderId);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreateOngoingActivityResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateOngoingActivityResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateOngoingActivityResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UpdateOngoingActivity")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateongoingActivityResult), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> UpdateOngoingActivity([FromBody] UpdateOngoingActivityArgs args)
    {
        try
        {
            var result = await ongoingActivityRepository.Update(args.OngoingActivityId, args.ActivityId,
                args.CustomerId, args.PurchaseOrderId);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateongoingActivityResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateongoingActivityResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateongoingActivityResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}