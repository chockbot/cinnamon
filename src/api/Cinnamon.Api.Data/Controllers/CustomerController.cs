using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.Customer.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Customer.Response;
using Microsoft.AspNetCore.Mvc;

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
                return new JsonResult(new GetCustomerResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetCustomerResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetCustomerResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetCustomerByEmail/{email}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetCustomerResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCustomerByEmail(string email)
    {
        try
        {
            var result = await customerRepository.GetByEmailAsync(email);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetCustomerResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetCustomerResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetCustomerResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetGovernmendId/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetGovernmentIdResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGovernmendId(int id)
    {
        try
        {
            var result = await customerRepository.GetGovermentId(id);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetGovernmentIdResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetGovernmentIdResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetGovernmentIdResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetProfilePicture/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetProfilePictureResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfilePicture(int id)
    {
        try
        {
            var result = await customerRepository.GetProfilePicture(id);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetProfilePictureResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetProfilePictureResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetProfilePictureResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetByHandler/{handler}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetCustomerResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByHandler(string handler)
    {
        try
        {
            var result = await customerRepository.GetByHandlerAsync(handler);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetCustomerResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetCustomerResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetCustomerResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
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
            bool applyFilters = (args.PageIndex.HasValue && args.CountPerPage.HasValue) || !string.IsNullOrEmpty(args.HandlerLike) || 
                                    args.IsOfficialPartner.HasValue || !string.IsNullOrEmpty(args.SearchValue);

            var result = applyFilters ?
                await customerRepository.GetAllAsync(args.IsVerified,args.SearchValue, 
                    args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage, 
                    args.HandlerLike, args.IsOfficialPartner, args.HasVerification) :
                await customerRepository.GetAllAsync();

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllCustomerResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            // get all without pagination to get all rows
            var all = applyFilters ?
                await customerRepository.GetAllAsync(args.IsVerified, args.SearchValue, null, null, args.HandlerLike, args.IsOfficialPartner) :
                await customerRepository.GetAllAsync();

            if (!all.Succeeded || all.Result == null)
            {
                return new JsonResult(new GetAllCustomerResult { ErrorInfo = new ErrorInfo { Message = all.Message } });
            }

            var totalRecords = all.Result.Count();
            return new JsonResult(new GetAllCustomerResult
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
            return new JsonResult(new GetAllCustomerResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
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
                args.PhoneNumber ,args.About, args.ProfilePath, args.IsMaker, args.ExternalLogin, args.Handler, args.HasAcceptedTerms);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreateCustomerResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateCustomerResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateCustomerResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CreateCustomerWithPassword")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateCustomerResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCustomerWithPassword([FromBody] CreateCustomerWithPasswordArgs args)
    {
        try
        {
            var result = await customerRepository.CreateWithPassword(args.FirstName, args.LastName, args.Email, args.Birthdate,
                args.PhoneNumber ,args.About, args.ProfilePath, args.IsMaker, args.ExternalLogin, args.Password, args.Handler, args.HasAcceptedTerms);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreateCustomerResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateCustomerResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateCustomerResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
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
                args.Birthdate, args.PhoneNumber, args.About, args.ProfilePath, args.IsMaker, args.ExternalLogin, 
                args.IsVerified,args.IsVerifiedDate , args.FrontIdImagePath, args.BackIdImagePath, args.TotalCredits, 
                args.IsOG, args.IsOGDate, args.IsOF, args.IsOFDate, args.ConnectionId, args.IsAccountBan, args.Handler);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateCustomerResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateCustomerResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateCustomerResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CheckCustomerLogin")]
    [HttpPost]
    [ProducesResponseType(typeof(CheckCustomerLoginResult), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> CheckCustomerLogin([FromBody] CheckCustomerLoginArgs args)
    {
        try
        {
            var result = await customerRepository.CheckLogin(args.Email, args.Password);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CheckCustomerLoginResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CheckCustomerLoginResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CheckCustomerLoginResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GenerateResetPasswordToken")]
    [HttpPost]
    [ProducesResponseType(typeof(GenerateResetPasswordTokenResult), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> GenerateResetPasswordToken([FromBody] GenerateResetPasswordTokenArgs args)
    {
        try
        {
            var result = await customerRepository.GenerateResetPasswordToken(args.Email);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GenerateResetPasswordTokenResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GenerateResetPasswordTokenResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GenerateResetPasswordTokenResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("ResetPassword")]
    [HttpPost]
    [ProducesResponseType(typeof(ResetPasswordResult), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordArgs args)
    {
        try
        {
            var result = await customerRepository.ResetPassword(args.Email, args.Token, args.Password);
            if (!result.Succeeded)
            {
                return new JsonResult(new ResetPasswordResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new ResetPasswordResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new ResetPasswordResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
    [Route("ChangeEmailAddress")]
    [HttpPost]
    [ProducesResponseType(typeof(ChangeEmailAddressResult), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> ChangeEmailAddress([FromBody] ChangeEmailAddressArgs args)
    {
        try
        {
            var result = await customerRepository.ChangeEmailAddress(args.CurrentEmail, args.NewEmail);
            if (!result.Succeeded)
            {
                return new JsonResult(new ChangeEmailAddressResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new ChangeEmailAddressResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new ChangeEmailAddressResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}