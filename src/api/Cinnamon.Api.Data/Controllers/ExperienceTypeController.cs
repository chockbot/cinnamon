using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.ExperienceType.Request;
using Cinnamon.Framework.ApiCommand.ApiData.ExperienceType.Response;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExperienceTypeController : ControllerBase
    {
        private readonly IExperienceTypeRepository _experienceTypeRepository;
        public ExperienceTypeController(IExperienceTypeRepository experienceTypeRepository)
        {
            _experienceTypeRepository = experienceTypeRepository;
        }

        [Route("GetExperienceType/{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(GetExperienceTypeResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetExperienceTypeById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return NotFound();
                }

                var result = await _experienceTypeRepository.GetByIdAsync(id);
                if (!result.Succeeded)
                {
                    return new JsonResult(new GetExperienceTypeResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                if (result.Result == null)
                {
                    return NotFound();
                }

                return new JsonResult(new GetExperienceTypeResult { Result = result.Result, IsSuccess = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetExperienceTypeResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [HttpGet]
        [Route("GetAllExperienceType")]
        [ProducesResponseType(typeof(GetAllExperienceTypeResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllExperienceType()
        {
            try
            {
                var result = await _experienceTypeRepository.GetAllAsync();
                if (!result.Succeeded)
                {
                    return new JsonResult(new GetAllExperienceTypeResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }
                return new JsonResult(new GetAllExperienceTypeResult { Result = result.Result, IsSuccess = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetAllExperienceTypeResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("CreateExperienceType")]
        [HttpPost]
        [ProducesResponseType(typeof(CreateExperienceTypeResult), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateExperienceType(CreateExperienceTypeArgs args)
        {
            try
            {
                var result = await _experienceTypeRepository.CreateExperienceType(args.Name);
                if (!result.Succeeded)
                {
                    return new JsonResult(new CreateExperienceTypeResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }
                return new JsonResult(new CreateExperienceTypeResult { IsSuccess = true, Result = result.Result });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CreateExperienceTypeResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("UpdateExperienceType")]
        [HttpPost]
        [ProducesResponseType(typeof(UpdateExperienceTypeResult), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateExperienceType(UpdateExperienceTypeArgs args)
        {
            try
            {
                var result = await _experienceTypeRepository.UpdateExperienceType(args.Id, args.Name);
                if (!result.Succeeded)
                {
                    return new JsonResult(new UpdateExperienceTypeResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }
                return new JsonResult(new UpdateExperienceTypeResult { IsSuccess = true, Result = result.Result });
            }
            catch (Exception ex)
            {
                return new JsonResult(new UpdateExperienceTypeResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }
    }
}
