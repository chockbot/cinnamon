using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.AddOns.Request;
using Cinnamon.Framework.ApiCommand.ApiData.AddOns.Response;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.AddOns;
using Cinnamon.Framework.ApiCommand.ApiData.Schedule.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Schedule.Response;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddOnController : ControllerBase
    {
        private readonly IAddOnsRepository _addOnsRepository;
        public AddOnController(IAddOnsRepository addOnsRepository)
        {
            _addOnsRepository = addOnsRepository;
        }

        [Route("GetAddOnById/{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(GetAddOnResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAddOnById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return NotFound();
                }

                var result = await _addOnsRepository.GetByIdAsync(id);
                if (!result.Succeeded)
                {
                    return new JsonResult(new GetAddOnResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }
                if (result.Result == null)
                {
                    return NotFound();
                }
                return new JsonResult(new GetAddOnResult { Result = result.Result, IsSuccess = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetAddOnResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("GetAllAddOns")]
        [HttpGet]
        [ProducesResponseType(typeof(GetAllAddOnResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllAddOnsAsync()
        {
            try
            {
                var result = await _addOnsRepository.GetAllAsync();
                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new GetAllAddOnResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }
                return new JsonResult(new GetAllAddOnResult { Result = result.Result, IsSuccess = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetAllAddOnResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("CreateAddOn")]
        [HttpPost]
        [ProducesResponseType(typeof(CreateAddOnResult), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAddOnAsync(CreateAddOnArgs addOnArgs)
        {
            try
            {
                var result = await _addOnsRepository.CreateAddOn(addOnArgs.ActivityId, addOnArgs.Name, addOnArgs.Price,
                                                                 addOnArgs.UnitPrice, addOnArgs.Description, addOnArgs.Order);
                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new CreateAddOnResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }
                return new JsonResult(new CreateAddOnResult { IsSuccess = true, Result = result.Result });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CreateAddOnResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("UpdateAdOn")]
        [HttpPost]
        [ProducesResponseType(typeof(UpdateAddOnResult), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAddOnAsync(UpdateAddOnArgs updateAddOnArgs)
        {
            try
            {
                var result = await _addOnsRepository.UpdateAddOn(updateAddOnArgs.Id, updateAddOnArgs.ActivityId, updateAddOnArgs.Name,
                                                                 updateAddOnArgs.Price, updateAddOnArgs.UnitPrice, updateAddOnArgs.Description, updateAddOnArgs.Order);

                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new UpdateAddOnResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }
                return new JsonResult(new UpdateAddOnResult { IsSuccess = true, Result = result.Result });
            }
            catch (Exception ex)
            {
                return new JsonResult(new UpdateAddOnResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("CreateManyAddOns")]
        [HttpPost]
        [ProducesResponseType(typeof(CreateAddOnsResult), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateManyAddOns([FromBody] CreateAddOnsArgs args)
        {
            try
            {
                var addOns = args.AddOns.Select(s =>
                {
                    return new AddOnsDTO
                    {
                        ActivityId  = args.ActivityId,
                        Name        = s.Name,
                        Price       = s.Price,
                        UnitPrice   = s.UnitPrice,
                        Description = s.Description,
                        Order       = s.Order
                    };
                }).ToList();

                var result = await _addOnsRepository.CreateAddOns(args.ActivityId, addOns);
                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new CreateAddOnsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                return new JsonResult(new CreateAddOnsResult { IsSuccess = true, Result = result.Result });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CreateAddOnsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("UpdateManyAddOns")]
        [HttpPost]
        [ProducesResponseType(typeof(UpdateAddOnsResult), StatusCodes.Status202Accepted)]
        public async Task<IActionResult> UpdateManyAddOns([FromBody] UpdateAddOnsArgs args)
        {
            try
            {
                var result = await _addOnsRepository.UpdateAddOns(args.AddOns.Select(s =>
                {
                    return new AddOnsDTO
                    {
                        Id          = s.Id,
                        ActivityId  = s.ActivityId,
                        Name        = s.Name ?? string.Empty,
                        Price       = s.Price ?? 0,
                        UnitPrice   = s.UnitPrice ?? string.Empty,
                        Description = s.Description ?? string.Empty,
                        Order       = s.Order ?? 0
                    };
                }));
                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new UpdateAddOnsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                return new JsonResult(new UpdateAddOnsResult { IsSuccess = true, Result = result.Result });
            }
            catch (Exception ex)
            {
                return new JsonResult(new UpdateAddOnsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("DeleteManyAddOns")]
        [HttpPost]
        [ProducesResponseType(typeof(DeleteAddOnsResult), StatusCodes.Status202Accepted)]
        public async Task<IActionResult> DeleteManyAddOns([FromBody] DeleteAddOnsArgs args)
        {
            try
            {
                var result = await _addOnsRepository.DeleteManyAddOns(args.AddOnsId);
                if (!result.Succeeded || !result.Result)
                {
                    return new JsonResult(new DeleteAddOnsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                return new JsonResult(new DeleteAddOnsResult { IsSuccess = true, Result = result.Result });
            }
            catch (Exception ex)
            {
                return new JsonResult(new DeleteAddOnsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }
    }
}
