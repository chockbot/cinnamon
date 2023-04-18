using Cinnamon.Framework.Common;
using Cinnamon.Framework.ApiCommand.ApiData.RequestRefund.Request;
using Cinnamon.Framework.ApiCommand.ApiData.RequestRefund.Response;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface IRequestRefundData 
{
    Task<AppResult<GetRequestRefundResult>> GetRequestRefundById(int id);
    Task<AppResult<GetAllRequestRefundResult>> GetAllRequestRefund(GetAllRequestRefundArgs args);
    Task<AppResult<CreateRequestRefundResult>> CreateRequestRefund(CreateRequestRefundArgs args);
    Task<AppResult<UpdateRequestRefundResult>> UpdateRequestRefund(UpdateRequestRefundArgs args);
}