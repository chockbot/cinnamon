using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Api.Data.Models.ResendEmail.Request;
using Cinnamon.Api.Data.Models.ResendEmail.Response;

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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetResendEmailById(int id)
    {
        try
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var result = await resendEmailRepository.GetByIdAsync(id);
            if (!result.Succeeded)
            {
                return new JsonResult(new GetResendEmailResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
            }

            if (result.Result == null)
            {
                return NotFound();
            }

            return new JsonResult(new GetResendEmailResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetResendEmailResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetResendEmailByEmailDateRange")]
    [HttpGet]
    [ProducesResponseType(typeof(GetResendEmailByEmailDateRange), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetResendEmailByEmailDateRange([FromQuery] GetResendEmailByEmailDateRangeArgs args)
    {
        try
        {
            // check date if valid
            DateTime from = DateTime.ParseExact(args.DateFrom, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            DateTime to = DateTime.ParseExact(args.DateTo, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

            var result = await resendEmailRepository.GetByEmailDateRange(args.Email, from, to);
            if (!result.Succeeded)
            {
                return new JsonResult(new GetResendEmailByEmailDateRange { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
            }

            if (result.Result == null)
            {
                return NotFound();
            }

            return new JsonResult(new GetResendEmailByEmailDateRange { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetResendEmailByEmailDateRange { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetAllResendEmail")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllResendEmailResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
                return NotFound();
            }

            // get all without pagination to get all rows
            var all = args.PageIndex.HasValue && args.CountPerPage.HasValue ?
                await resendEmailRepository.GetAllAsync(null, null) :
                await resendEmailRepository.GetAllAsync();

            if (!all.Succeeded || all.Result == null)
            {
                return NotFound();
            }

            var totalRecords = all.Result.Count();
            return new JsonResult(new GetAllResendEmailResult
            {
                Result = result.Result,
                IsSuccess = true,
                Pagination = new Models.Pagination
                {
                    PageIndex = args.PageIndex,
                    PerPage = args.CountPerPage,
                    TotalRecords = totalRecords,
                    TotalPages = args.CountPerPage.HasValue && args.PageIndex.HasValue ?
                                (int)Math.Ceiling(Convert.ToDouble(totalRecords / args.CountPerPage.Value)) : null
                }
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetAllResendEmailResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CreateEmailResend")]
    [HttpPost]
    [ProducesResponseType(typeof(CreatedEmailResendResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateEmailResend([FromBody] CreateEmailResendArgs args)
    {
        try
        {
            var result = await resendEmailRepository.Create(args.Email, args.DateResend);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreatedEmailResendResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreatedEmailResendResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreatedEmailResendResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UpdateEmailResend")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateEmailResendResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateEmailResend([FromBody] UpdateResendEmailArgs args)
    {
        try
        {
            var result = await resendEmailRepository.Update(args.ResendEmailId, args.Email, args.DateResend);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateEmailResendResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateEmailResendResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateEmailResendResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
        }
    }
}