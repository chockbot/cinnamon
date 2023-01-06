using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.Address.Response;
using Microsoft.AspNetCore.Mvc;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.Address.Request;

namespace Cinnamon.Api.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressRepository _AddressRepository;

        public AddressController(IAddressRepository addressRepository)
        {
            _AddressRepository = addressRepository;
        }

        [Route("GetAddressById/{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(GetAddressResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAddressById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return NotFound();
                }

                var result = await _AddressRepository.GetByIdAsync(id);
                if (!result.Succeeded)
                {
                    return new JsonResult(new GetAddressResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                if (result.Result == null)
                {
                    return NotFound();
                }

                return new JsonResult(new GetAddressResult { Result = result.Result, IsSuccess = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetAddressResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("GetAddressByActivityId/{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(GetAddressResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAddressByActivityId(int id)
        {
            try
            {
                var result = await _AddressRepository.GetByActivityIdAsync(id);
                if (!result.Succeeded)
                {
                    return new JsonResult(new GetAddressResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                if (result.Result == null)
                {
                    return NotFound();
                }

                return new JsonResult(new GetAddressResult { Result = result.Result, IsSuccess = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetAddressResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [HttpGet]
        [Route("GetAllAddress")]
        [ProducesResponseType(typeof(GetAllAddressResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllAddressAsync()
        {
            try
            {
                var result = await _AddressRepository.GetAllAsync();
                if (!result.Succeeded)
                {
                    return new JsonResult(new GetAllAddressResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }
                return new JsonResult(new GetAllAddressResult { Result = result.Result, IsSuccess = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetAllAddressResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("CreateAddress")]
        [HttpPost]
        [ProducesResponseType(typeof(CreateAddressResult), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAddress(CreateAddressArgs addressArgs)
        {
            try
            {
                var result = await _AddressRepository.CreateAddress(addressArgs.ActivityId, addressArgs.Address1, addressArgs.Address2
                                                              , addressArgs.District, addressArgs.City);
                if (!result.Succeeded)
                {
                    return new JsonResult(new CreateAddressResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }
                return new JsonResult(new CreateAddressResult { IsSuccess = true, Result = result.Result });
            }
            catch(Exception ex)
            {
                return new JsonResult(new CreateAddressResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("UpdateAddress")]
        [HttpPost]
        [ProducesResponseType(typeof(UpdatedAddressResult), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAddress(UpdateAddressArgs addressArgs)
        {
            try
            {
                var result = await _AddressRepository.UpdateAddress(addressArgs.AddressId, addressArgs.Address1, addressArgs.Address2,
                                                              addressArgs.District, addressArgs.City);
                if(!result.Succeeded)
                {
                    return new JsonResult(new UpdatedAddressResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }
                return new JsonResult(new UpdatedAddressResult { IsSuccess = true, Result = result.Result });
            }
            catch(Exception ex)
            {
                return new JsonResult(new UpdatedAddressResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }
    }
}
