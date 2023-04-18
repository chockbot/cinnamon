using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.PayoutAccount.Request;
using Cinnamon.Framework.ApiCommand.ApiData.PayoutAccount.Response;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Data.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PayoutAccountController : ControllerBase 
{
    private readonly IPayoutAccountRepository payoutAccountRepository;

    public PayoutAccountController(IPayoutAccountRepository payoutAccountRepository)
    {
        this.payoutAccountRepository = payoutAccountRepository;
    }

    [Route("GetPayoutAccountById/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetPayoutAccountResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPayoutAccountById(int id)
    {
        try
        {
            var result = await payoutAccountRepository.GetByIdAsync(id);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetPayoutAccountResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetPayoutAccountResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetPayoutAccountResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetPayoutAccountByCustomerId/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetPayoutAccountResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPayoutAccountByCustomerId(int id)
    {
        try
        {
            var result = await payoutAccountRepository.GetByCustomerIdAsync(id);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetPayoutAccountResult { ErrorInfo = new ErrorInfo { Message = result.Message, Code = "NOTEXIST" } });
            }

            return new JsonResult(new GetPayoutAccountResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetPayoutAccountResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetAllPayoutAccounts")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllPayoutAccountsResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllPayoutAccounts([FromQuery] GetAllPayoutAccountsArgs args)
    {
        try
        {
            var result =
                args.PageIndex.HasValue && args.CountPerPage.HasValue ?
                await payoutAccountRepository.GetAllAsync(args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage) :
                await payoutAccountRepository.GetAllAsync();

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllPayoutAccountsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            // get all without pagination to get all rows
            var all = args.PageIndex.HasValue && args.CountPerPage.HasValue ?
                await payoutAccountRepository.GetAllAsync(null, null) :
                await payoutAccountRepository.GetAllAsync();

            if (!all.Succeeded || all.Result == null)
            {
                return new JsonResult(new GetAllPayoutAccountsResult { ErrorInfo = new ErrorInfo { Message = all.Message } });
            }

            var totalRecords = all.Result.Count();
            return new JsonResult(new GetAllPayoutAccountsResult
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
            return new JsonResult(new GetAllPayoutAccountsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CreatePayoutAccount")]
    [HttpPost]
    [ProducesResponseType(typeof(CreatePayoutAccountResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreatePayoutAccount([FromBody] CreatePayoutAccountArgs args)
    {
        try
        {
            var result = await payoutAccountRepository.Create(args.CustomerId, args.AccountNumber, args.AccountHolder, args.Payload ?? string.Empty, args.BankChannel);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreatePayoutAccountResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreatePayoutAccountResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreatePayoutAccountResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UpdatePayoutAccount")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdatePayoutAccountResult), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> UpdatePayoutAccount([FromBody] UpdatePayoutAccountArgs args)
    {
        try
        {
            var result = await payoutAccountRepository.Update(args.Id, args.AccountNumber, args.AccountHolder, args.Payload, args.BankChannel);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdatePayoutAccountResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdatePayoutAccountResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdatePayoutAccountResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}