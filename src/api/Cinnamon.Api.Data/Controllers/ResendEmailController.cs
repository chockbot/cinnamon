using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.ResendEmail.Response;
using Cinnamon.Framework.ApiCommand.ApiData.ResendEmail.Request;

namespace Cinnamon.Api.Data.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ResendEmailController : ControllerBase
{
    private readonly IResendEmailRepository resendEmailRepository;

    public ResendEmailController(IResendEmailRepository resendEmailRepository)
    {
        this.resendEmailRepository = resendEmailRepository;
    }

    [Route("GetResendEmailById/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetResendEmailResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetResendEmailById(int id)
    {
        try
        {
            var result = await resendEmailRepository.GetByIdAsync(id);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetResendEmailResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetResendEmailResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetResendEmailResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetResendEmailByEmailDateRange")]
    [HttpGet]
    [ProducesResponseType(typeof(GetResendEmailByEmailDateRange), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetResendEmailByEmailDateRange([FromQuery] GetResendEmailByEmailDateRangeArgs args)
    {
        try
        {
            // check date if valid
            DateTime from = DateTime.ParseExact(args.DateFrom, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            DateTime to = DateTime.ParseExact(args.DateTo, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

            var result = await resendEmailRepository.GetByEmailDateRange(args.Email, from, to);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetResendEmailByEmailDateRange { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetResendEmailByEmailDateRange { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetResendEmailByEmailDateRange { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetAllResendEmail")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllResendEmailResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllResendEmail([FromQuery] GetAllResendEmailArgs args)
    {
        try
        {
            var result =
                args.PageIndex.HasValue && args.CountPerPage.HasValue ?
                await resendEmailRepository.GetAllAsync(args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage) :
                await resendEmailRepository.GetAllAsync();

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllResendEmailResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            // get all without pagination to get all rows
            var all = args.PageIndex.HasValue && args.CountPerPage.HasValue ?
                await resendEmailRepository.GetAllAsync(null, null) :
                await resendEmailRepository.GetAllAsync();

            if (!all.Succeeded || all.Result == null)
            {
                return new JsonResult(new GetAllResendEmailResult { ErrorInfo = new ErrorInfo { Message = all.Message } });
            }

            var totalRecords = all.Result.Count();
            return new JsonResult(new GetAllResendEmailResult
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
            return new JsonResult(new GetAllResendEmailResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CreateEmailResend")]
    [HttpPost]
    [ProducesResponseType(typeof(CreatedEmailResendResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateEmailResend([FromBody] CreateEmailResendArgs args)
    {
        try
        {
            var result = await resendEmailRepository.Create(args.Email, args.DateResend);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreatedEmailResendResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreatedEmailResendResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreatedEmailResendResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UpdateEmailResend")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateEmailResendResult), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> UpdateEmailResend([FromBody] UpdateResendEmailArgs args)
    {
        try
        {
            var result = await resendEmailRepository.Update(args.ResendEmailId, args.Email, args.DateResend);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateEmailResendResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateEmailResendResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateEmailResendResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}