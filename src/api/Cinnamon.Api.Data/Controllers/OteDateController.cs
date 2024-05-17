using System.Globalization;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.OteDate.Request;
using Cinnamon.Framework.ApiCommand.ApiData.OteDate.Response;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Data.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OteDateController : ControllerBase
{
    private readonly IOteDateRepository oteDateRepository;

    public OteDateController(IOteDateRepository oteDateRepository)
    {
        this.oteDateRepository = oteDateRepository;
    }

    [Route("GetDate")]
    [HttpGet]
    [ProducesResponseType(typeof(GetOteDateResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOteDate([FromQuery] GetOteDateArgs args)
    {
        try
        {
            DateTime? dateFrom = null;
            if(!string.IsNullOrEmpty(args.From))
            {
                dateFrom = DateTime.ParseExact(args.From, "yyyyMMdd", CultureInfo.InvariantCulture);
            }

            DateTime? dateTo = null;
            if(!string.IsNullOrEmpty(args.To))
            {
                dateTo = DateTime.ParseExact(args.To, "yyyyMMdd", CultureInfo.InvariantCulture);
            }

            var result = await this.oteDateRepository.GetOteDate(args.ActivityId, dateFrom, dateTo);
            if(!result.Succeeded || result.Result is null) 
            {
                return new JsonResult(new GetOteDateResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetOteDateResult {
                IsSuccess = true,
                Result = result.Result
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetOteDateResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetDate/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetOteDateByIdResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOteDate(int id)
    {
        try
        {
            var result = await this.oteDateRepository.GetOteDateById(id);
            if(!result.Succeeded || result.Result is null) 
            {
                return new JsonResult(new GetOteDateByIdResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetOteDateByIdResult {
                IsSuccess = true,
                Result = result.Result
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetOteDateByIdResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}