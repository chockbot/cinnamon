using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class SaveEmailTemplateHandler : ISaveEmailTemplateHandler
{
    private readonly IDynamicContentData dynamicContentData;

    public SaveEmailTemplateHandler(IDynamicContentData dynamicContentData)
    {
        this.dynamicContentData = dynamicContentData;
    }

    public AppResult<SaveEmailTemplateResult> Execute(SaveEmailTemplateArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<SaveEmailTemplateResult>> ExecuteAsync(SaveEmailTemplateArgs args)
    {
        try
        {
            var checkTemplateRes = await dynamicContentData.GetEmailTemplates(new Framework.ApiCommand.ApiData.DynamicContent.Request.GetEmailTemplatesArgs {
                ActivityId = args.ActivityId,
                ProviderId = args.ProviderId,
                TemplateType = args.TemplateType.ToString()
            });
            if(!checkTemplateRes.Succeeded || checkTemplateRes.Result is null || !checkTemplateRes.Result.IsSuccess)
            {
                return AppResult<SaveEmailTemplateResult>.CreateFailed(new ApplicationException(checkTemplateRes.Result?.ErrorInfo?.Message), checkTemplateRes.Message);
            }
            var templates = checkTemplateRes.Result.Result;

            int templateId = 0;

            // create if don't have yet template
            if(!templates.Any())
            {
                var createTemplate = await dynamicContentData.CreateEmailTemplate(new Framework.ApiCommand.ApiData.DynamicContent.Request.CreateEmailTemplateArgs {
                    ActivityId = args.ActivityId,
                    Body = args.Body,
                    ProviderId = args.ProviderId,
                    Subject = args.Subject,
                    TemplateType = args.TemplateType.ToString()
                });
                if(!createTemplate.Succeeded || createTemplate.Result is null || !createTemplate.Result.IsSuccess)
                {
                    return AppResult<SaveEmailTemplateResult>.CreateFailed(
                        new ApplicationException("An error occured when creating new template."), "An error occured when creating new template.");
                }
                templateId = createTemplate.Result.Result.Id;
            }

            // update if already have template
            if(templates.Any())
            {
                var template = templates.First();
                var updateTemplate = await dynamicContentData.UpdateEmailTemplate(new Framework.ApiCommand.ApiData.DynamicContent.Request.UpdateEmailTemplateArgs {
                    Body = args.Body,
                    Subject = args.Subject,
                }, template.Id);
                if(!updateTemplate.Succeeded || updateTemplate.Result is null || !updateTemplate.Result.IsSuccess)
                {
                    return AppResult<SaveEmailTemplateResult>.CreateFailed(
                        new ApplicationException("An error occured when updating the template."), "An error occured when updating the template.");
                }
                templateId = updateTemplate.Result.Result.Id;
            }

            return AppResult<SaveEmailTemplateResult>.CreateSucceeded(new SaveEmailTemplateResult {Id = templateId}, "Successfully saved email template.");
        }
        catch (Exception ex)
        {
            return AppResult<SaveEmailTemplateResult>.CreateFailed(ex, "An error occured in SaveEmailTemplateHandler.");
        }
    }
}