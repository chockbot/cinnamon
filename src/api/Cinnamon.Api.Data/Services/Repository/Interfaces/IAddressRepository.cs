using Cinnamon.Framework.Common;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Address;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces
{
    public interface IAddressRepository
    {
        Task<AppResult<AddressDTO>> GetByIdAsync(int id);
        Task<AppResult<IEnumerable<AddressDTO>>> GetAllAsync();
        Task<AppResult<AddressDTO>> UpdateAddress(int AddressId, string Address1, string Address2, string District, string City);
        Task<AppResult<AddressDTO>> RemoveAddress(int AddressId);
        Task<AppResult<AddressDTO>> CreateAddress(int ActivityId, string Address1, string Address2, string District, string City);
    }
}
