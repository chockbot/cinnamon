using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.ProviderCustomQuestion.Request;
using Cinnamon.Framework.ApiCommand.ApiData.ProviderCustomQuestion.Response;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Data.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProviderCustomQuestionsController : ControllerBase 
{
    private readonly IProviderCustomQuestionRepository providerCustomQuestionRepository;

    public ProviderCustomQuestionsController(IProviderCustomQuestionRepository providerCustomQuestionRepository)
    {
        this.providerCustomQuestionRepository = providerCustomQuestionRepository;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateCustomQuestionResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> Index([FromBody] CreateCustomQuestionArgs args)
    {
        try
        {
            var result = await providerCustomQuestionRepository.CreateCustomQuestion(new Framework.ApiCommand.ApiData.DTO.ProviderCustomQuestion.ProviderCustomQuestionDTO {
                ActivityId = args.ActivityId,
                FieldLabel = args.FieldLabel,
                FieldType = args.FieldType,
                ProviderId = args.ProviderId,
                Required = args.Required,
                Options = args.Options
            });
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new CreateCustomQuestionResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateCustomQuestionResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateCustomQuestionResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetCustomQuestionsResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> Index([FromQuery] GetCustomQuestionsArgs args)
    {
        try
        {
            var result = await providerCustomQuestionRepository.GetCustomQuestions(args.ProviderId, args.ActivityId);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new GetCustomQuestionsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetCustomQuestionsResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetCustomQuestionsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("Bulk/Delete")]
    [HttpPost]
    [ProducesResponseType(typeof(DeleteCustomQuestionsResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> DeleteCustomQuestions([FromBody] DeleteCustomQuestionsArgs args)
    {
        try
        {
            var result = await providerCustomQuestionRepository.DeleteCustomQuestions(args.Ids);
            if(!result.Succeeded)
            {
                return new JsonResult(new DeleteCustomQuestionsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new DeleteCustomQuestionsResult { IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new DeleteCustomQuestionsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("Bulk/Update")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateCustomQuestionsResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> UpdateCustomQuestions([FromBody] UpdateCustomQuestionsArgs args)
    {
        try
        {
            var dtos = args.Questions.Select(q => new Framework.ApiCommand.ApiData.DTO.ProviderCustomQuestion.ProviderCustomQuestionDTO {
                ActivityId = q.ActivityId,
                FieldLabel = q.FieldLabel,
                FieldType  = q.FieldType,
                Id         = q.Id,
                ProviderId = q.ProviderId,
                Required   = q.Required,
                Options    = q.Options
            });
            
            var result = await providerCustomQuestionRepository.UpdateCustomerQuestions(dtos);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new UpdateCustomQuestionsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateCustomQuestionsResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateCustomQuestionsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}