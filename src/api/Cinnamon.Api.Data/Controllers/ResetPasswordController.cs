using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.ResetPassword.Request;
using Cinnamon.Framework.ApiCommand.ApiData.ResetPassword.Response;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Data.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ResetPasswordController : ControllerBase 
{
    private readonly IResetPasswordRepository resetPasswordRepository;

    public ResetPasswordController(IResetPasswordRepository resetPasswordRepository)
    {
        this.resetPasswordRepository = resetPasswordRepository;
    }
    
    [Route("GetResetPasswordById/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetResetPasswordResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetResetPasswordById(int id)
    {
        try
        {
            var result = await resetPasswordRepository.GetByIdAsync(id);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetResetPasswordResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetResetPasswordResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetResetPasswordResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetResetPasswordByGuidToken/{guid}/{token}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetResetPasswordResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetResetPasswordByGuidToken(string guid, string token)
    {
        try
        {
            var result = await resetPasswordRepository.GetByGuidTokenAsync(guid, token);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetResetPasswordResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetResetPasswordResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetResetPasswordResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetAllResetPassword")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllResetPasswordResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllResetPassword([FromQuery] GetAllResetPasswordArgs args)
    {
        try
        {
            var result =
                args.PageIndex.HasValue && args.CountPerPage.HasValue ?
                await resetPasswordRepository.GetAllAsync(args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage) :
                await resetPasswordRepository.GetAllAsync();

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllResetPasswordResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            // get all without pagination to get all rows
            var all = args.PageIndex.HasValue && args.CountPerPage.HasValue ?
                await resetPasswordRepository.GetAllAsync(null, null) :
                await resetPasswordRepository.GetAllAsync();

            if (!all.Succeeded || all.Result == null)
            {
                return new JsonResult(new GetAllResetPasswordResult { ErrorInfo = new ErrorInfo { Message = all.Message } });
            }

            var totalRecords = all.Result.Count();
            return new JsonResult(new GetAllResetPasswordResult
            {
                Result = result.Result,
                IsSuccess = true,
                Pagination = new Pagination
                {
                    PageIndex = args.PageIndex,
                    PerPage = args.CountPerPage,
                    TotalRecords = totalRecords,
                    TotalPages = args.CountPerPage.HasValue && args.PageIndex.HasValue ?
                                (int)Math.Ceiling((double)totalRecords / args.CountPerPage.Value) : null
                }
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetAllResetPasswordResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CreateResetPassword")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateResetPasswordResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateResetPassword([FromBody] CreateResetPasswordArgs args)
    {
        try
        {
            var result = await resetPasswordRepository.Create(args.Email, args.Guid, args.Token, args.IsUsed, args.GeneratedToken);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreateResetPasswordResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateResetPasswordResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateResetPasswordResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UpdateResetPassword")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateResetPasswordResult), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> UpdateResetPassword([FromBody] UpdateResetPasswordArgs args)
    {
        try
        {
            var result = await resetPasswordRepository.Update(args.Id, args.IsUsed);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateResetPasswordResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateResetPasswordResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateResetPasswordResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}