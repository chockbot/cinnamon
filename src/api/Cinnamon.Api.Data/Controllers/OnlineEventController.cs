using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.OnlineEvent.Request;
using Cinnamon.Framework.ApiCommand.ApiData.OnlineEvent.Response;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Data.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OnlineEventController : ControllerBase
{
    private readonly IOnlineEventRepository _onlineEventRepository;
    public OnlineEventController(IOnlineEventRepository onlineEventRepository)
    {
        _onlineEventRepository = onlineEventRepository;
    }
    [Route("DeleteOnlineEvent")]
    [HttpPost]
    [ProducesResponseType(typeof(DeleteOnlineEventResult), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> DeleteAddOn([FromBody] DeleteOnlineEventArgs args)
    {
        try
        {
            var result = await _onlineEventRepository.DeleteOnlineEvent(args.Id);
            if (!result.Succeeded || !result.Result)
            {
                return new JsonResult(new DeleteOnlineEventResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new DeleteOnlineEventResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new DeleteOnlineEventResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}
