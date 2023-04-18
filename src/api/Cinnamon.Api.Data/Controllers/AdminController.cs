using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.AdminUser.Request;
using Cinnamon.Framework.ApiCommand.ApiData.AdminUser.Response;
using Cinnamon.Framework.ApiCommand.ApiData.Location.Response;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminUserRepository _adminUserRepository;

        public AdminController(IAdminUserRepository adminUserRepository)
        {
            _adminUserRepository = adminUserRepository;
        }

        [Route("User")]
        [HttpGet]
        [ProducesResponseType(typeof(GetAdminUserByEmailResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAdminUserByEmail([FromQuery] GetAdminUserByEmailArgs args)
        {
            try
            {
                var result = await _adminUserRepository.GetAdminUserByEmailAsync(args.Email);

                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new GetAdminUserByEmailResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                return new JsonResult(new GetAdminUserByEmailResult
                {
                    Result = result.Result,
                    IsSuccess = true
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetAllRegionResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }
    }
}
