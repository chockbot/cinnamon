using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.OteDate.Response;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Data.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OteDateController : ControllerBase
{
    private readonly IOteDateRepository oteDateRepository;

    public OteDateController(IOteDateRepository oteDateRepository)
    {
        this.oteDateRepository = oteDateRepository;
    }

    [Route("GetDate/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetOteDateByIdResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOteDate(int id)
    {
        try
        {
            var result = await this.oteDateRepository.GetOteDateById(id);
            if(!result.Succeeded || result.Result is null) 
            {
                return new JsonResult(new GetOteDateByIdResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetOteDateByIdResult {
                IsSuccess = true,
                Result = result.Result
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetOteDateByIdResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}