using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.OteReminderFlags.Request;
using Cinnamon.Framework.ApiCommand.ApiData.OteReminderFlags.Response;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Data.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OteRemindersController : ControllerBase 
{
    private readonly IOteReminderRepository oteReminderRepository;

    public OteRemindersController(IOteReminderRepository oteReminderRepository)
    {
        this.oteReminderRepository = oteReminderRepository;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateReminderFlagResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateReminderFlag([FromBody] CreateReminderFlagArgs args)
    {
        try
        {
            var result = await oteReminderRepository.CreateReminderFlag(new Framework.ApiCommand.ApiData.DTO.OteReminderFlag.OteReminderFlagDTO {
                ActivityId = args.ActivityId,
                OteDateId = args.OteDateId
            });
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new CreateReminderFlagResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateReminderFlagResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateReminderFlagResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetReminderFlagsResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReminderFlags([FromQuery] GetReminderFlagsArgs args)
    {
        try
        {
            var result = await oteReminderRepository.GetReminderFlags(args.ActivityId, args.OteDateId);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new GetReminderFlagsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetReminderFlagsResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetReminderFlagsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("EventsForReminders")]
    [HttpGet]
    [ProducesResponseType(typeof(GetEventsForReminderResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEventsForReminder()
    {
        try
        {
            var result = await oteReminderRepository.GetEventsForReminder();
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new GetEventsForReminderResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetEventsForReminderResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetEventsForReminderResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CustomersForReminder")]
    [HttpGet]
    [ProducesResponseType(typeof(GetCustomersToRemindResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCustomersToRemind([FromQuery] GetCustomersToRemindArgs args)
    {
        try
        {
            var result = await oteReminderRepository.CustomersToRemind(args.ActivityId, args.OteDateId);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new GetCustomersToRemindResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetCustomersToRemindResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetCustomersToRemindResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}