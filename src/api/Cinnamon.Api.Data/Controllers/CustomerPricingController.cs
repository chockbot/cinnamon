using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.CustomerPricing.Request;
using Cinnamon.Framework.ApiCommand.ApiData.CustomerPricing.Response;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Data.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerPricingController : ControllerBase 
{
    private readonly ICustomerPricingRepository customerPricingRepository;

    public CustomerPricingController(ICustomerPricingRepository customerPricingRepository)
    {
        this.customerPricingRepository = customerPricingRepository;
    }

    [Route("GetCustomerPricingById/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetCustomerPricingResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCustomerPricingById(int id)
    {
        try
        {
            var result = await customerPricingRepository.GetByIdAsync(id);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetCustomerPricingResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetCustomerPricingResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetCustomerPricingResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetCustomerPricingByCustomerId/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetCustomerPricingResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCustomerPricingByCustomerId(int id)
    {
        try
        {
            var result = await customerPricingRepository.GetByCustomerIdAsync(id);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetCustomerPricingResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetCustomerPricingResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetCustomerPricingResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetAllCustomerPricing")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllCustomerPricingResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCustomerPricing([FromQuery] GetAllCustomerPricingArgs args)
    {
        try
        {
            var result =
                args.PageIndex.HasValue && args.CountPerPage.HasValue ?
                await customerPricingRepository.GetAllAsync(args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage) :
                await customerPricingRepository.GetAllAsync();

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllCustomerPricingResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            // get all without pagination to get all rows
            var all = args.PageIndex.HasValue && args.CountPerPage.HasValue ?
                await customerPricingRepository.GetAllAsync(null, null) :
                await customerPricingRepository.GetAllAsync();

            if (!all.Succeeded || all.Result == null)
            {
                return new JsonResult(new GetAllCustomerPricingResult { ErrorInfo = new ErrorInfo { Message = all.Message } });
            }

            var totalRecords = all.Result.Count();
            return new JsonResult(new GetAllCustomerPricingResult
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
            return new JsonResult(new GetAllCustomerPricingResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CreateCustomerPricing")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateCustomerPricingResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCustomerPricing([FromBody] CreateCustomerPricingArgs args)
    {
        try
        {
            var result = await customerPricingRepository.Create(args.CustomerId, args.Email, args.Rate, args.IsManualPayment);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreateCustomerPricingResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateCustomerPricingResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateCustomerPricingResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UpdateCustomerPricing")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateCustomerPricingResult), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> UpdateCustomerPricing([FromBody] UpdateCustomerPricingArgs args)
    {
        try
        {
            var result = await customerPricingRepository.Update(args.Id, args.Rate.HasValue ? args.Rate.Value : 0, 
                args.IsManualPayment.HasValue ? args.IsManualPayment.Value : false);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateCustomerPricingResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateCustomerPricingResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateCustomerPricingResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}