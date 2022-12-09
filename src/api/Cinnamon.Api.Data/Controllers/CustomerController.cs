using Microsoft.AspNetCore.Mvc;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Api.Data.Models.Customer.Response;
using Cinnamon.Api.Data.Models.Customer.Request;

namespace Cinnamon.Api.Data.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly ICustomerRepository customerRepository;

    public CustomerController(ICustomerRepository customerRepository)
    {
        this.customerRepository = customerRepository;
    }

    [Route("GetCustomerById/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetCustomerResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCustomerById(int id)
    {
        try
        {
            var result = await customerRepository.GetByIdAsync(id);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetCustomerResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetCustomerResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetCustomerResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetAllCustomers")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllCustomerResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllCustomers([FromQuery] GetAllCustomersArgs args)
    {
        try
        {
            var result =
                args.PageIndex.HasValue && args.CountPerPage.HasValue ?
                await customerRepository.GetAllAsync(args.IsVerified, args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage) :
                await customerRepository.GetAllAsync();

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllCustomerResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
            }

            // get all without pagination to get all rows
            var all = args.PageIndex.HasValue && args.CountPerPage.HasValue ?
                await customerRepository.GetAllAsync(args.IsVerified, null, null) :
                await customerRepository.GetAllAsync();

            if (!all.Succeeded || all.Result == null)
            {
                return new JsonResult(new GetAllCustomerResult { ErrorInfo = new Models.ErrorInfo { Message = all.Message } });
            }

            var totalRecords = all.Result.Count();
            return new JsonResult(new GetAllCustomerResult
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
            return new JsonResult(new GetAllCustomerResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CreateCustomer")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateCustomerResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerArgs args)
    {
        try
        {
            var result = await customerRepository.Create(args.UserId, args.FirstName, args.LastName, args.Email, args.Birthdate,
                args.About, args.ProfilePath, args.IsMaker, args.ExternalLogin);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreateCustomerResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateCustomerResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateCustomerResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UpdateCustomer")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateCustomerResult), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> UpdateCustomer([FromBody] UpdateCustomerArgs args)
    {
        try
        {
            var result = await customerRepository.Update(args.CustomerId, args.FirstName, args.LastName, args.Email,
                args.Birthdate, args.About, args.ProfilePath, args.IsMaker, args.ExternalLogin, args.IsVerified, args.FrontIdImagePath, args.BackIdImagePath);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateCustomerResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateCustomerResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateCustomerResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
        }
    }
}