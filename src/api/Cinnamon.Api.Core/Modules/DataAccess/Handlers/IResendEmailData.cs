using Cinnamon.Framework.ApiCommand.ApiData.ResendEmail.Response;
using Cinnamon.Framework.ApiCommand.ApiData.ResendEmail.Request;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface IResendEmailData 
{
    Task<AppResult<GetResendEmailResult>> GetResendEmailById(int id);
    Task<AppResult<GetResendEmailByEmailDateRange>> GetResendEmailByDataRange(GetResendEmailByEmailDateRangeArgs args);
    Task<AppResult<GetAllResendEmailResult>> GetAllResendEmails(GetAllResendEmailArgs args);
    Task<AppResult<CreatedEmailResendResult>> CreateEmailResend(CreateEmailResendArgs args);
    Task<AppResult<UpdateEmailResendResult>> UpdateEmailResend(UpdateResendEmailArgs args);
}