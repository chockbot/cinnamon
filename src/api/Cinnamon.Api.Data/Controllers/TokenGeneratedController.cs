using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.TokenGenerated.Request;
using Cinnamon.Framework.ApiCommand.ApiData.TokenGenerated.Response;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Data.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TokenGeneratedController : ControllerBase
{
    private readonly ITokenGeneratedRepository tokenGeneratedRepository;

    public TokenGeneratedController(ITokenGeneratedRepository tokenGeneratedRepository)
    {
        this.tokenGeneratedRepository = tokenGeneratedRepository;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateTokenResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateToken([FromBody] CreateTokenArgs args)
    {
        try
        {
            var result = await tokenGeneratedRepository.CreateTokenGenearted(args.TokenType, args.Guid, args.Token, args.Payload);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new CreateTokenResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateTokenResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateTokenResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("{guid}/{token}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetTokenResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetToken(string guid, string token)
    {
        try
        {
            var result = await tokenGeneratedRepository.GetTokenGenerated(guid, token);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new GetTokenResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetTokenResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetTokenResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [HttpPost]
    [Route("{id}")]
    [ProducesResponseType(typeof(UpdateTokenResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateToken([FromBody] UpdateTokenArgs args, int id)
    {
        try
        {
            var result = await tokenGeneratedRepository.UpdateTokenGenearted(id, args.TokenType, args.Guid, args.Token, args.Payload);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new UpdateTokenResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateTokenResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateTokenResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}