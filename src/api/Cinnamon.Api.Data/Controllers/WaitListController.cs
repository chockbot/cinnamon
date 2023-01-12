using Microsoft.AspNetCore.Mvc;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.Waitlist.Response;
using Cinnamon.Framework.ApiCommand.ApiData.Waitlist.Request;

namespace Cinnamon.Api.Data.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WaitListController : ControllerBase
{
    private readonly IWaitListRepository waitListRepository;

    public WaitListController(IWaitListRepository waitListRepository)
    {
        this.waitListRepository = waitListRepository;
    }

    [Route("GetWaitlistById/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetWaitlistResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWaitlistById(int id)
    {
        try
        {
            var result = await waitListRepository.GetByIdAsync(id);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetWaitlistResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetWaitlistResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetWaitlistResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetWaitlistByGuid/{guid}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetWaitlistResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWaitlistByGuid(string guid)
    {
        try
        {
            var result = await waitListRepository.GetByGuidAsync(guid);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetWaitlistResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetWaitlistResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetWaitlistResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetWaitlistByEmail/{email}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetWaitlistResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWaitlistByEmail(string email)
    {
        try
        {
            var result = await waitListRepository.GetByEmailAsync(email);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetWaitlistResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetWaitlistResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetWaitlistResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetAllWaitlist")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllWaitlistResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllWaitlist([FromQuery] GetAllWaitlistArgs args)
    {
        try
        {
            var result =
                args.PageIndex.HasValue && args.CountPerPage.HasValue ?
                await waitListRepository.GetAllAsync(args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage) :
                await waitListRepository.GetAllAsync();

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllWaitlistResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            // get all without pagination to get all rows
            var all = args.PageIndex.HasValue && args.CountPerPage.HasValue ?
                await waitListRepository.GetAllAsync(null, null) :
                await waitListRepository.GetAllAsync();

            if (!all.Succeeded || all.Result == null)
            {
                return new JsonResult(new GetAllWaitlistResult { ErrorInfo = new ErrorInfo { Message = all.Message } });
            }

            var totalRecords = all.Result.Count();
            return new JsonResult(new GetAllWaitlistResult
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
            return new JsonResult(new GetAllWaitlistResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CreateWaitlist")]
    [HttpPost]
    [ProducesResponseType(typeof(CreatedWaitlistResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateWaitlist([FromBody] CreateWaitlistArgs args)
    {
        try
        {
            var result = await waitListRepository.Create(args.Email, args.Guid, args.Token, args.IsVerified);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreatedWaitlistResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreatedWaitlistResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreatedWaitlistResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UpdateWaitlist")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateWaitlistResult), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> UpdateWaitlist([FromBody] UpdateWaitlistArgs args)
    {
        try
        {
            var result = await waitListRepository.Update(args.Email, args.Guid, args.Token, args.IsVerified);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateWaitlistResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateWaitlistResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateWaitlistResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}