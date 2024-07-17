using AutoMapper;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.OteWaitlist.Request;
using Cinnamon.Framework.ApiCommand.ApiData.OteWaitlist.Response;
using Dto = Cinnamon.Framework.ApiCommand.ApiData.DTO;
using Microsoft.AspNetCore.Mvc;
namespace Cinnamon.Api.Data.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OteWaitlistController : ControllerBase
{
    private readonly IOteWaitlistRepository oteWaitlistRepository;
    private readonly IMapper mapper;
    public OteWaitlistController(IOteWaitlistRepository oteWaitlistRepository, IMapper mapper)
    {
        this.oteWaitlistRepository = oteWaitlistRepository;
        this.mapper = mapper;
    }

    [Route("{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetWaitListResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWaitList(int id)
    {
        try
        {
            var result = await oteWaitlistRepository.GetWaitList(id);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new GetWaitListResult { ErrorInfo = new ErrorInfo { Message = result.Message } }); 
            }

            return new JsonResult(new GetWaitListResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetWaitListResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CreateOteWaitlist")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateOteWaitlistResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateOteWaitlist([FromBody] CreateOteWaitlistArgs args)
    {
        try
        {
            var dtoWaitlist = mapper.Map<Dto.OteWaitlist.OteWaitlistDTO>(args);
            var result = await oteWaitlistRepository.CreateOteWaitlist(dtoWaitlist);
            if (!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new CreateOteWaitlistResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            return new JsonResult(new CreateOteWaitlistResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateOteWaitlistResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UpdateOteWaitlist")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateOteWaitlistResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateOteWaitlist([FromBody] UpdateOteWaitlistArgs args)
    {
        try
        {
            var dtoWaitlist = mapper.Map<Dto.OteWaitlist.OteWaitlistDTO>(args);
            var result = await oteWaitlistRepository.UpdateOteWaitlist(dtoWaitlist);
            if (!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new UpdateOteWaitlistResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            return new JsonResult(new UpdateOteWaitlistResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateOteWaitlistResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetWaitlistByProvider")]
    [HttpGet]
    [ProducesResponseType(typeof(GetOteWaitlistByProviderResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWaitlistByProvider([FromQuery] GetOteWaitlistByProviderArgs args)
    {
        try
        {
            var result = await oteWaitlistRepository.GetWaitlistByProvider(args.ProviderId, args.ActivityId, args.Status);
            if (!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new GetOteWaitlistByProviderResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            return new JsonResult(new GetOteWaitlistByProviderResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetOteWaitlistByProviderResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
    [Route("DeleteOteWaitlist")]
    [HttpPost]
    [ProducesResponseType(typeof(DeleteOteWaitlistResult), StatusCodes.Status202Accepted)]
    public async Task<IActionResult>DeleteOteWaitlist([FromBody] DeleteOteWaitlistArgs args)
    {
        try
        {
            var result = await oteWaitlistRepository.DeleteOteWaitlist(args.Id);
            if (!result.Succeeded || !result.Result)
            {
                return new JsonResult(new DeleteOteWaitlistResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new DeleteOteWaitlistResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new DeleteOteWaitlistResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}
