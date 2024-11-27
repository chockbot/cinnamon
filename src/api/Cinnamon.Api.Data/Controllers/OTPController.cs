using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.GuestOTP.Request;
using Cinnamon.Framework.ApiCommand.ApiData.GuestOTP.Response;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Data.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OTPController : ControllerBase
{
    private readonly IGuestOTPRepository guestOTPRepository;   
    public OTPController(IGuestOTPRepository guestOTPRepository)
    {
        this.guestOTPRepository = guestOTPRepository;
    }
    [Route("CreateOTP")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateGuestOTPResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateOTP([FromBody] CreateGuestOTPArgs args)
    {
        try
        {
            var result = await guestOTPRepository.CreateGuestOTP(new Framework.ApiCommand.ApiData.DTO.GuestOTP.GuestOTPDTO
            {
                Email = args.Email,
                OTPCode = args.OTECode
            });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreateGuestOTPResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateGuestOTPResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateGuestOTPResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
    [Route("GetOTPByEmail")]
    [HttpGet]
    [ProducesResponseType(typeof(GetGuestOTPByEmailResult), StatusCodes.Status200OK)]
    public async Task<IActionResult>GetOTPByEmail(string email)
    {
        try
        {
            var result = await guestOTPRepository.GetByEmailAsync(email);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetGuestOTPByEmailResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetGuestOTPByEmailResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetGuestOTPByEmailResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}
