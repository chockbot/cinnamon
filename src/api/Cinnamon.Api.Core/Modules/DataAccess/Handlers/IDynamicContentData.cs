using Cinnamon.Framework.ApiCommand.ApiData.DynamicContent.Request;
using Cinnamon.Framework.ApiCommand.ApiData.DynamicContent.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface IDynamicContentData 
{
    Task<AppResult<CreateDynamicContentResult>> CreateDynamicContent(CreateDynamicContentArgs args);
    Task<AppResult<GetDynamicContentResult>> GetDynamicContent(string identifier);
    Task<AppResult<UpdateDynamicContentResult>> UpdateDynamicContent(UpdateDynamicContentArgs args);
    Task<AppResult<CreateEmailTemplateResult>> CreateEmailTemplate(CreateEmailTemplateArgs args);
    Task<AppResult<GetEmailTemplatesResult>> GetEmailTemplates(GetEmailTemplatesArgs args);
    Task<AppResult<UpdateEmailTemplateResult>> UpdateEmailTemplate(UpdateEmailTemplateArgs args, int templateId);
}