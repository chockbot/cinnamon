using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.ExperienceCreationType.Response;
using Cinnamon.Framework.ApiCommand.ApiData.ExperienceType.Request;
using Cinnamon.Framework.ApiCommand.ApiData.ExperienceType.Response;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExperienceCreationTypeController : ControllerBase
    {
        private readonly IExperienceCreationTypeRepository _experienceCreationTypeRepository;
        public ExperienceCreationTypeController(IExperienceCreationTypeRepository experienceCreationTypeRepository)
        {
            _experienceCreationTypeRepository = experienceCreationTypeRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetExperienceCreationTypeResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllExperienceCreationTypes()
        {
            try
            {
                var result = await _experienceCreationTypeRepository.GetExperienceCreationTypes();
                if (!result.Succeeded)
                {
                    return new JsonResult(new GetExperienceCreationTypeResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }
                return new JsonResult(new GetExperienceCreationTypeResult { Result = result.Result, IsSuccess = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetExperienceCreationTypeResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }
    }
}
