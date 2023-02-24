using System.Globalization;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.FailedLogin.Request;
using Cinnamon.Framework.ApiCommand.ApiData.FailedLogin.Response;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Data.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FailedLoginController : ControllerBase 
{
    private readonly IFailedLoginRepository failedLoginRepository;

    public FailedLoginController(IFailedLoginRepository failedLoginRepository)
    {
        this.failedLoginRepository = failedLoginRepository;
    }

    [Route("CreateFailedLogin")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateFailedLoginResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateFailedLogin([FromBody] CreateFailedLoginArgs args)
    {
        try
        {
            var result = await failedLoginRepository.CreateAsync(args.Email, args.Metadata ?? string.Empty, args.LoginDate);
            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreateFailedLoginResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }

            return new JsonResult( new CreateFailedLoginResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateFailedLoginResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("GetFailedLogins")]
    [HttpGet]
    [ProducesResponseType(typeof(GetFailedLoginsResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFailedLogins([FromQuery] GetFailedLoginsArgs args)
    {
        try
        {
            // check date if valid
            DateTime from = DateTime.ParseExact(args.DateFrom, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            DateTime to = DateTime.ParseExact(args.DateTo, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

            var result = await failedLoginRepository.GetFailedLogins(args.Email, from, to);
            if(!result.Succeeded ||  result.Result == null)
            {
                return new JsonResult(new CreateFailedLoginResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }

            return new JsonResult( new GetFailedLoginsResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateFailedLoginResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }
}