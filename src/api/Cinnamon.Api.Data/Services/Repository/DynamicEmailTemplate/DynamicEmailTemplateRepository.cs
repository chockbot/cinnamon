using System.Linq.Expressions;
using AutoMapper;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.DynamicContent;
using Cinnamon.Framework.Common;
using Entities = Cinnamon.Api.Data.Repository.Entities;

namespace Cinnamon.Api.Data.Services.Repository.DynamicEmailTemplate;

public class DynamicEmailTemplateRepository : IDynamicEmailTemplateRepository
{
    private readonly IDataStore dataStore;
    private readonly IMapper mapper;

    public DynamicEmailTemplateRepository(IDataStore dataStore, IMapper mapper)
    {
        this.dataStore = dataStore;
        this.mapper = mapper;
    }

    public async Task<AppResult<DynamicEmailTemplateDTO>> CreateEmailTemplate(DynamicEmailTemplateDTO emailTemplate)
    {
        try
        {
            var entity = mapper.Map<Entities.DynamicEmailTemplate>(emailTemplate);
            
            var createRes = await dataStore.DynamicEmailTemplate.Add(entity);
            if(!createRes.Succeeded || createRes.Result is null)
            {
                return AppResult<DynamicEmailTemplateDTO>.CreateFailed(new ApplicationException(createRes.Message), createRes.Message);
            }

            var result = mapper.Map<DynamicEmailTemplateDTO>(createRes.Result);
            return AppResult<DynamicEmailTemplateDTO>.CreateSucceeded(result, "Email template successfully created.");
        }
        catch (Exception ex)
        {
            return AppResult<DynamicEmailTemplateDTO>.CreateFailed(ex, "An error occured when creating email template.");
        }
    }

    public async Task<AppResult<DynamicEmailTemplateDTO>> UpdateEmailTemplate(DynamicEmailTemplateDTO emailTemplate)
    {
        try
        {
            var toUpdateRes = await dataStore.DynamicEmailTemplate.FindFirstAsync(d => d.Id == emailTemplate.Id);
            if(!toUpdateRes.Succeeded || toUpdateRes.Result is null)
            {
                return AppResult<DynamicEmailTemplateDTO>.CreateFailed(new ApplicationException("Unable to locate email template."), "Unable to locate email template.");
            }

            var entity = toUpdateRes.Result;
            entity.Body = emailTemplate.Body ?? entity.Body;
            entity.Subject = emailTemplate.Subject ?? entity.Subject;

            var updateRes = await dataStore.DynamicEmailTemplate.Update(entity);
            if(!updateRes.Succeeded || updateRes.Result is null)
            {
                return AppResult<DynamicEmailTemplateDTO>.CreateFailed(new ApplicationException(updateRes.Message), updateRes.Message);
            }

            var result = mapper.Map<DynamicEmailTemplateDTO>(updateRes.Result);
            return AppResult<DynamicEmailTemplateDTO>.CreateSucceeded(result, "Email template successfully updated.");
        }
        catch (Exception ex)
        {
            return AppResult<DynamicEmailTemplateDTO>.CreateFailed(ex, "An error occured when updating email template.");
        }
    }

    public async Task<AppResult<IEnumerable<DynamicEmailTemplateDTO>>> GetEmailTemplates(int? activityId, int? providerId, string? templateType)
    {
        try
        {
            Expression<Func<Entities.DynamicEmailTemplate, bool>> filter = d => 
                (activityId.HasValue ? d.ActivityId == activityId.Value : true) &&
                (providerId.HasValue ? d.ProviderId == providerId.Value : true) &&
                (!string.IsNullOrEmpty(templateType) ? d.TemplateType.ToLower() == templateType.ToLower() : true);
            
            var templatesRes = await dataStore.DynamicEmailTemplate.FindAsync(filter);
            if(!templatesRes.Succeeded || templatesRes.Result is null)
            {
                return AppResult<IEnumerable<DynamicEmailTemplateDTO>>.CreateFailed(new ApplicationException(templatesRes.Message), templatesRes.Message);
            }

            var result = mapper.Map<IEnumerable<DynamicEmailTemplateDTO>>(templatesRes.Result);
            return AppResult<IEnumerable<DynamicEmailTemplateDTO>>.CreateSucceeded(result, "Successfully get email templates.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<DynamicEmailTemplateDTO>>.CreateFailed(ex, "An error occured when getting email templates.");
        }
    }
}
