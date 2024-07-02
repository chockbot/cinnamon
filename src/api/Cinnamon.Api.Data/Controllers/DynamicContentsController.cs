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
    private readonly IDynamicEmailTemplateRepository dynamicEmailTemplateRepository;
    private readonly IMapper mapper;

    public DynamicContentsController(IDynamicContnetRepository dynamicContnetRepository, 
        IDynamicEmailTemplateRepository dynamicEmailTemplateRepository, IMapper mapper )
    {
        this.dynamicContnetRepository = dynamicContnetRepository;
        this.dynamicEmailTemplateRepository = dynamicEmailTemplateRepository;
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

    [HttpPost]
    [Route("EmailTemplates")]
    [ProducesResponseType(typeof(CreateEmailTemplateResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateEmailTemplate([FromBody] CreateEmailTemplateArgs args)
    {
        try
        {
            var dto = mapper.Map<DynamicEmailTemplateDTO>(args);
            
            var result = await dynamicEmailTemplateRepository.CreateEmailTemplate(dto);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new CreateEmailTemplateResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            
            return new JsonResult(new CreateEmailTemplateResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateEmailTemplateResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [HttpGet]
    [Route("EmailTemplates")]
    [ProducesResponseType(typeof(GetEmailTemplatesResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetEmailTemplates([FromQuery] GetEmailTemplatesArgs args)
    {
        try
        {
            var result = await dynamicEmailTemplateRepository.GetEmailTemplates(args.ActivityId, args.ProviderId, args.TemplateType);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new GetEmailTemplatesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            
            return new JsonResult(new GetEmailTemplatesResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetEmailTemplatesResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [HttpPatch]
    [Route("EmailTemplates/{id}")]
    [ProducesResponseType(typeof(UpdateEmailTemplateResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateEmailTemplate([FromBody] UpdateEmailTemplateArgs args, int id)
    {
        try
        {
            var result = await dynamicEmailTemplateRepository.UpdateEmailTemplate(new DynamicEmailTemplateDTO {
                Id = id,
                Body = args.Body,
                Subject = args.Subject
            });
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new UpdateEmailTemplateResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            
            return new JsonResult(new UpdateEmailTemplateResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateEmailTemplateResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}