using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.ExternalLoginToken.Request;
using Cinnamon.Framework.ApiCommand.ApiData.ExternalLoginToken.Response;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Data.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExternalLoginTokenController : ControllerBase
{
    private readonly IExternalLoginTokenRepository externalLoginTokenRepository;

    public ExternalLoginTokenController(IExternalLoginTokenRepository externalLoginTokenRepository)
    {
        this.externalLoginTokenRepository = externalLoginTokenRepository;
    }

    [Route("GetLoginToken/{token}/{guid}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetExternalLoginTokenResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLoginToken(string token, string guid)
    {
        try
        {
            var result = await externalLoginTokenRepository.GetByTokenAsync(token, guid);
            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetExternalLoginTokenResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetExternalLoginTokenResult { IsSuccess = true,
                Result = new Framework.ApiCommand.ApiData.DTO.ExternalLoginToken.ExternalLoginTokenDTO {
                DateGenerated = result.Result.DateGenerated,
                Id = result.Result.Id,
                IsUsed = result.Result.IsUsed,
                Token = result.Result.Token,
                Email = result.Result.Email,
                FirstName = result.Result.FirstName,
                Guid = result.Result.Guid,
                LastName = result.Result.LastName
            }});
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetExternalLoginTokenResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CreateToken")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateExternalLoginTokenResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateToken([FromBody] CreateExterLoginTokenArgs args)
    {
        try
        {
            var result = await externalLoginTokenRepository.CreateTokenAsync(args.Token, args.Guid, args.Email, 
                args.DateGenerated, args.FirstName ?? string.Empty, args.LastName ?? string.Empty);
            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreateExternalLoginTokenResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateExternalLoginTokenResult { IsSuccess = true,
                Result = new Framework.ApiCommand.ApiData.DTO.ExternalLoginToken.ExternalLoginTokenDTO {
                DateGenerated = result.Result.DateGenerated,
                Id = result.Result.Id,
                IsUsed = result.Result.IsUsed,
                Token = result.Result.Token,
                Email = result.Result.Email,
                Guid = result.Result.Guid,
                FirstName = result.Result.FirstName,
                LastName = result.Result.LastName
            }});
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateExternalLoginTokenResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UpdateToken")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateExternalLoginTokenResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateToken([FromBody] UpdateExternalLoginTokenArgs args)
    {
        try
        {
            var result = await externalLoginTokenRepository.UpdateTokenAsync(args.Id, args.IsUsed);
            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateExternalLoginTokenResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateExternalLoginTokenResult { IsSuccess = true,
                Result = new Framework.ApiCommand.ApiData.DTO.ExternalLoginToken.ExternalLoginTokenDTO {
                DateGenerated = result.Result.DateGenerated,
                Id = result.Result.Id,
                IsUsed = result.Result.IsUsed,
                Token = result.Result.Token,
                Email = result.Result.Email,
                FirstName = result.Result.FirstName,
                Guid = result.Result.Guid,
                LastName = result.Result.LastName
            }});
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateExternalLoginTokenResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}