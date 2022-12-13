using Cinnamon.Api.Data.Models.Activity.Response;
using Cinnamon.Api.Data.Models.Address.Request;
using Cinnamon.Api.Data.Models.Address.Response;
using Cinnamon.Api.Data.Services.Repository.Activity;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
                    return new JsonResult(new GetAddressResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
                }

                if (result.Result == null)
                {
                    return NotFound();
                }

                return new JsonResult(new GetAddressResult { Result = result.Result, IsSuccess = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetAddressResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
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
                    return new JsonResult(new GetAllAddressResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
                }
                return new JsonResult(new GetAllAddressResult { Result = result.Result, IsSuccess = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetAllAddressResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
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
                    return new JsonResult(new CreateAddressResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
                }
                return new JsonResult(new CreateAddressResult { IsSuccess = true, Result = result.Result });
            }
            catch(Exception ex)
            {
                return new JsonResult(new CreateAddressResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
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
                    return new JsonResult(new UpdatedAddressResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
                }
                return new JsonResult(new UpdatedAddressResult { IsSuccess = true, Result = result.Result });
            }
            catch(Exception ex)
            {
                return new JsonResult(new UpdatedAddressResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
            }
        }
    }
}
