using Cinnamon.Framework.ApiCommand.ApiCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Cinnamon.Framework.ApiCommand.ApiCore.System.Response;
using Cinnamon.Api.Core.Services.SystemService.Handlers;

namespace Cinnamon.Api.Core.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SystemController : ControllerBase
{
    private readonly IGetSystemDateHandler getSystemDateHandler;

    public SystemController(IGetSystemDateHandler getSystemDateHandler)
    {
        this.getSystemDateHandler = getSystemDateHandler;
    }

    [Route("GetServerDate")]
    [HttpGet]
    [ProducesResponseType(typeof(GetServerDateResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetServerDate()
    {
        try
        {
            var result = await getSystemDateHandler.ExecuteAsync(new Services.SystemService.Interactors.GetSystemDateArgs {});

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetServerDateResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetServerDateResult 
                {
                    IsSuccess = true, 
                    Result = result.Result.ServerDate
                } );
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetServerDateResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }
}