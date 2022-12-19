using Cinnamon.Framework.ApiCommand.ApiData.Address.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Address.Response;
using Cinnamon.Framework.ApiCommand.ApiData.Customer.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Customer.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers
{
    public interface IAddressData
    {
        Task<AppResult<GetAddressResult>> GetAddressById(int id);
        Task<AppResult<GetAllAddressResult>> GetAllAddress();
        Task<AppResult<CreateAddressResult>> CreateAddress(CreateAddressArgs args);
        Task<AppResult<UpdatedAddressResult>> UpdateAddress(UpdateAddressArgs args);
    }
}
