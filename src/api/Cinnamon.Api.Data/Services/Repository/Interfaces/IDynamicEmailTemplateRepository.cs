using Cinnamon.Framework.ApiCommand.ApiData.DTO.DynamicContent;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IDynamicEmailTemplateRepository
{
    Task<AppResult<DynamicEmailTemplateDTO>> CreateEmailTemplate(DynamicEmailTemplateDTO emailTemplate);
    Task<AppResult<DynamicEmailTemplateDTO>> UpdateEmailTemplate(DynamicEmailTemplateDTO emailTemplate);
    Task<AppResult<IEnumerable<DynamicEmailTemplateDTO>>> GetEmailTemplates(int? activityId, int? providerId, string? templateType);
}