using AutoMapper;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.DynamicContent;
using Cinnamon.Framework.ApiCommand.ApiData.DynamicContent.Request;
using Cinnamon.Framework.ApiCommand.ApiData.DynamicContent.Response;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Data.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DynamicContentsController : ControllerBase
{

    private readonly IDynamicContnetRepository dynamicContnetRepository;
    private readonly IMapper mapper;

    public DynamicContentsController(IDynamicContnetRepository dynamicContnetRepository, IMapper mapper)
    {
        this.dynamicContnetRepository = dynamicContnetRepository;
        this.mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateDynamicContentResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Index([FromBody] CreateDynamicContentArgs args)
    {
        try
        {
            var dto = mapper.Map<DynamicContentDTO>(args);
            
            var result = await dynamicContnetRepository.CreateDynamicContent(dto);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new CreateDynamicContentResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            
            return new JsonResult(new CreateDynamicContentResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateDynamicContentResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("{identifier}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetDynamicContentResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetDynamicContent(string identifier)
    {
        try
        {   
            var result = await dynamicContnetRepository.GetDynamicContent(identifier);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new GetDynamicContentResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            
            return new JsonResult(new GetDynamicContentResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetDynamicContentResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [HttpPost]
    [Route("UpdateDynamicContent")]
    [ProducesResponseType(typeof(UpdateDynamicContentResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateDynamicContent([FromBody] UpdateDynamicContentArgs args)
    {
        try
        {
            var dto = mapper.Map<DynamicContentDTO>(args);
            
            var result = await dynamicContnetRepository.UpdateDynamicContent(dto);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new UpdateDynamicContentResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            
            return new JsonResult(new UpdateDynamicContentResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateDynamicContentResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}