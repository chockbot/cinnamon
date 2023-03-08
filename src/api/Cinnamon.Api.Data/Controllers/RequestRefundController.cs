using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.RequestRefund.Request;
using Cinnamon.Framework.ApiCommand.ApiData.RequestRefund.Response;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Data.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RequestRefundController : ControllerBase 
{
    private readonly IRequestRefundRepository requestRefundRepository;

    public RequestRefundController(IRequestRefundRepository requestRefundRepository)
    {
        this.requestRefundRepository = requestRefundRepository;
    }

    [Route("GetRequestRefundById/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetRequestRefundResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRequestRefundById(int id)
    {
        try
        {
            var result = await requestRefundRepository.GetByIdAsync(id);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetRequestRefundResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetRequestRefundResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetRequestRefundResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetAllRequestRefund")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllRequestRefundResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllRequestRefund([FromQuery] GetAllRequestRefundArgs args)
    {
        try
        {
            bool isHaveFilter = (args.PageIndex.HasValue && args.CountPerPage.HasValue) || args.CustomerId.HasValue || args.Status.HasValue;

            var result = isHaveFilter?
                await requestRefundRepository.GetAllAsync(args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage,
                    args.IncludePurchaseOrder, args.IncludeCustomer, args.CustomerId, args.Status) :
                await requestRefundRepository.GetAllAsync();

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllRequestRefundResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            // get all without pagination to get all rows
            var all = isHaveFilter ?
                await requestRefundRepository.GetAllAsync(null, null, null, null, null, null) :
                await requestRefundRepository.GetAllAsync();

            if (!all.Succeeded || all.Result == null)
            {
                return new JsonResult(new GetAllRequestRefundResult { ErrorInfo = new ErrorInfo { Message = all.Message } });
            }

            var totalRecords = all.Result.Count();
            return new JsonResult(new GetAllRequestRefundResult
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
            return new JsonResult(new GetAllRequestRefundResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CreateRequestRefund")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateRequestRefundResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateRequestRefund([FromBody] CreateRequestRefundArgs args)
    {
        try
        {
            var result = await requestRefundRepository.Create(args.CustomerId, args.PurchaseOrderId, 
                args.ExperienceTitle, args.Status, args.Reason);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreateRequestRefundResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateRequestRefundResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateRequestRefundResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UpdateRequestRefund")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateRequestRefundResult), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> UpdateRequestRefund([FromBody] UpdateRequestRefundArgs args)
    {
        try
        {
            var result = await requestRefundRepository.Update(args.RefundId, args.Status);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateRequestRefundResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateRequestRefundResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateRequestRefundResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}