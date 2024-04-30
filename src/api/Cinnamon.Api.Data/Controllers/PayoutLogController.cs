using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.PurchaseOrder;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.PayoutLog.Request;
using Cinnamon.Framework.ApiCommand.ApiData.PayoutLog.Response;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Cinnamon.Api.Data.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PayoutLogController : ControllerBase 
{
    private readonly IPayoutLogRepository payoutLogRepository;

    public PayoutLogController(IPayoutLogRepository payoutLogRepository)
    {
        this.payoutLogRepository = payoutLogRepository;
    }

    [Route("GetPayoutLogById/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetPayoutLogResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPayoutLogById(int id)
    {
        try
        {
            var result = await payoutLogRepository.GetByIdAsync(id);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetPayoutLogResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetPayoutLogResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetPayoutLogResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetAllPayoutLogs")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllPayoutLogsResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllPayoutLogs([FromQuery] GetAllPayoutLogsArgs args)
    {
        try
        {
            var result =
                args.PageIndex.HasValue && args.CountPerPage.HasValue ?
                await payoutLogRepository.GetAllAsync(args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage) :
                await payoutLogRepository.GetAllAsync();

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllPayoutLogsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            // get all without pagination to get all rows
            var all = args.PageIndex.HasValue && args.CountPerPage.HasValue ?
                await payoutLogRepository.GetAllAsync(null, null) :
                await payoutLogRepository.GetAllAsync();

            if (!all.Succeeded || all.Result == null)
            {
                return new JsonResult(new GetAllPayoutLogsResult { ErrorInfo = new ErrorInfo { Message = all.Message } });
            }

            var totalRecords = all.Result.Count();
            return new JsonResult(new GetAllPayoutLogsResult
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
            return new JsonResult(new GetAllPayoutLogsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CreatePayoutLog")]
    [HttpPost]
    [ProducesResponseType(typeof(CreatePayoutLogResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreatePayoutLog([FromBody] CreatePayoutLogArgs args)
    {
        try
        {
            var result = await payoutLogRepository.Create(args.PurchaseOrderId, args.CustomerId, 
                args.Amount, args.Status, args.Remarks, args.Payload);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreatePayoutLogResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreatePayoutLogResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreatePayoutLogResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UpdatePayoutLog")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdatePayoutLogResult), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> UpdatePayoutLog([FromBody] UpdatePayoutLogArgs args)
    {
        try
        {
            var result = await payoutLogRepository.Update(args.Id, args.Status, args.Remarks);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdatePayoutLogResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdatePayoutLogResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdatePayoutLogResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetPayoutsByProvider")]
    [HttpGet]
    [ProducesResponseType(typeof(GetPayoutsByProviderResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGrossSalesByProvider([FromQuery] GetPayoutsByProviderArgs args)
    {
        try
        {
            DateTime From = DateTime.ParseExact(args.DateFrom, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

            var result = await payoutLogRepository.GetPayoutByProvider(args.Id, From, args.Status);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetPayoutsByProviderResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetPayoutsByProviderResult
            {
                Result = result.Result,
                IsSuccess = true,
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetPayoutsByProviderResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}