using Cinnamon.Api.Data.Services.Repository.ActivityAddress;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.Description.Response;
using Microsoft.AspNetCore.Mvc;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.Description.Request;

namespace Cinnamon.Api.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DescriptionController : ControllerBase
    {
        private readonly IDescriptionRepository _descriptionRepository;
        public DescriptionController(IDescriptionRepository descriptionRepository)
        {
            _descriptionRepository = descriptionRepository;
        }

        [Route("GetDescription/{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(GetDescriptionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDescriptionById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return NotFound();
                }

                var result = await _descriptionRepository.GetByIdAsync(id);
                if (!result.Succeeded)
                {
                    return new JsonResult(new GetDescriptionResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                if (result.Result == null)
                {
                    return NotFound();
                }

                return new JsonResult(new GetDescriptionResult { Result = result.Result, IsSuccess = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetDescriptionResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("GetDescriptionByActivityId/{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(GetDescriptionResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDescriptionByActivityId(int id)
        {
            try
            {
                var result = await _descriptionRepository.GetByIdAsync(id);
                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new GetDescriptionResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                return new JsonResult(new GetDescriptionResult { Result = result.Result, IsSuccess = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetDescriptionResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [HttpGet]
        [Route("GetAllDescription")]
        [ProducesResponseType(typeof(GetAllDescriptionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllDescriptionAsync()
        {
            try
            {
                var result = await _descriptionRepository.GetAllAsync();
                if (!result.Succeeded)
                {
                    return new JsonResult(new GetAllDescriptionResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }
                return new JsonResult(new GetAllDescriptionResult { Result = result.Result, IsSuccess = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetAllDescriptionResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("CreateDescription")]
        [HttpPost]
        [ProducesResponseType(typeof(CreateDescriptionResult), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateDescription(CreateDescriptionArgs descriptionArgs)
        {
            try
            {
                var result = await _descriptionRepository.CreateDescription(descriptionArgs.ActivityId, descriptionArgs.Description, descriptionArgs.SpecificsYouWillProvide
                                                              , descriptionArgs.CustomerBringWithThem, descriptionArgs.AdditionalRequirements, descriptionArgs.ActivityLevel, 
                                                              descriptionArgs.SkillLevel, descriptionArgs.MinimumAge, descriptionArgs.CanAdultsJoin);
                if (!result.Succeeded)
                {
                    return new JsonResult(new CreateDescriptionResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }
                return new JsonResult(new CreateDescriptionResult { IsSuccess = true, Result = result.Result });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CreateDescriptionResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("UpdateDescription")]
        [HttpPost]
        [ProducesResponseType(typeof(UpdatedDescriptionResult), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateDescription(UpdateDescriptionArgs updateDescription)
        {
            try
            {
                var result = await _descriptionRepository.UpdateDescription(updateDescription.Id, updateDescription.Description, updateDescription.SpecificsYouWillProvide,
                                                              updateDescription.CustomerBringWithThem, updateDescription.AdditionalRequirements, updateDescription.ActivityLevel,
                                                              updateDescription.SkillLevel, updateDescription.MinimumAge, updateDescription.CanAdultsJoin);
                if (!result.Succeeded)
                {
                    return new JsonResult(new UpdatedDescriptionResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }
                return new JsonResult(new UpdatedDescriptionResult { IsSuccess = true, Result = result.Result });
            }
            catch (Exception ex)
            {
                return new JsonResult(new UpdatedDescriptionResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }
    }
}
