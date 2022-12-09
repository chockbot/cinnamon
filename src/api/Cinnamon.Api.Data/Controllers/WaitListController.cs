using Microsoft.AspNetCore.Mvc;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Api.Data.Models.Waitlist.Request;
using Cinnamon.Api.Data.Models.Waitlist.Response;

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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetWaitlistById(int id)
    {
        try
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var result = await waitListRepository.GetByIdAsync(id);
            if (!result.Succeeded)
            {
                return new JsonResult(new GetWaitlistResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
            }

            if (result.Result == null)
            {
                return NotFound();
            }

            return new JsonResult(new GetWaitlistResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetWaitlistResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetWaitlistByGuid/{guid}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetWaitlistResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetWaitlistByGuid(string guid)
    {
        try
        {
            var result = await waitListRepository.GetByGuidAsync(guid);
            if (!result.Succeeded)
            {
                return new JsonResult(new GetWaitlistResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
            }

            if (result.Result == null)
            {
                return NotFound();
            }

            return new JsonResult(new GetWaitlistResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetWaitlistResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetWaitlistByEmail/{email}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetWaitlistResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetWaitlistByEmail(string email)
    {
        try
        {
            var result = await waitListRepository.GetByEmailAsync(email);
            if (!result.Succeeded)
            {
                return new JsonResult(new GetWaitlistResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
            }

            if (result.Result == null)
            {
                return NotFound();
            }

            return new JsonResult(new GetWaitlistResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetWaitlistResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetAllWaitlist")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllWaitlistResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
                return NotFound();
            }

            // get all without pagination to get all rows
            var all = args.PageIndex.HasValue && args.CountPerPage.HasValue ?
                await waitListRepository.GetAllAsync(null, null) :
                await waitListRepository.GetAllAsync();

            if (!all.Succeeded || all.Result == null)
            {
                return NotFound();
            }

            var totalRecords = all.Result.Count();
            return new JsonResult(new GetAllWaitlistResult
            {
                Result = result.Result,
                IsSuccess = true,
                Pagination = new Models.Pagination
                {
                    PageIndex = args.PageIndex,
                    PerPage = args.CountPerPage,
                    TotalRecords = totalRecords,
                    TotalPages = args.CountPerPage.HasValue && args.PageIndex.HasValue ?
                                (int)Math.Ceiling(Convert.ToDouble(totalRecords / args.CountPerPage.Value)) : null
                }
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetAllWaitlistResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CreateWaitlist")]
    [HttpPost]
    [ProducesResponseType(typeof(CreatedWaitlistResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateWaitlist([FromBody] CreateWaitlistArgs args)
    {
        try
        {
            var result = await waitListRepository.Create(args.Email, args.Guid, args.Token, args.IsVerified);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreatedWaitlistResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreatedWaitlistResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreatedWaitlistResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UpdateWaitlist")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateWaitlistResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateWaitlist([FromBody] UpdateWaitlistArgs args)
    {
        try
        {
            var result = await waitListRepository.Update(args.Email, args.Guid, args.Token, args.IsVerified);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateWaitlistResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateWaitlistResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateWaitlistResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
        }
    }
}