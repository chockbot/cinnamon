using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class EmailTemplateHandler : IEmailTemplateHandler
{
    private readonly IGetProfileHandler getProfileHandler;
    private readonly IDynamicContentData dynamicContentData;

    public EmailTemplateHandler(IGetProfileHandler getProfileHandler, IDynamicContentData dynamicContentData)
    {
        this.getProfileHandler = getProfileHandler;
        this.dynamicContentData = dynamicContentData;
    }
    
    public AppResult<EmailTemplateResult> Execute(EmailTemplateArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<EmailTemplateResult>> ExecuteAsync(EmailTemplateArgs args)
    {
        try
        {
            var profileRes = await getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs {});
            if(!profileRes.Succeeded || profileRes.Result is null)
            {
                return AppResult<EmailTemplateResult>.CreateFailed(new ApplicationException(profileRes.Message), profileRes.Message);
            }
            var profile = profileRes.Result;

            var getTemplateRes = await dynamicContentData.GetEmailTemplates(new Framework.ApiCommand.ApiData.DynamicContent.Request.GetEmailTemplatesArgs {
                ActivityId = args.ActivityId,
                ProviderId = profile.Id,
                TemplateType = args.TemplateType
            });
            if(!getTemplateRes.Succeeded || getTemplateRes.Result is null || !getTemplateRes.Result.IsSuccess)
            {
                return AppResult<EmailTemplateResult>.CreateFailed(new ApplicationException(getTemplateRes.Result?.ErrorInfo?.Message), getTemplateRes.Message);
            }
            var templates = getTemplateRes.Result.Result;

            if(!templates.Any())
            {
                return AppResult<EmailTemplateResult>.CreateFailed(new ApplicationException("Unable to find template."), "Unable to find template.");
            }

            var template = templates.First();
            return AppResult<EmailTemplateResult>.CreateSucceeded(new EmailTemplateResult {
                ActivityId = template.ActivityId,
                Body = template.Body,
                ProviderId = template.ProviderId,
                Subject = template.Subject,
                Id = template.Id
            }, "Successfully get template.");
        }
        catch (Exception ex)
        {
            return AppResult<EmailTemplateResult>.CreateFailed(ex, "An error occured in EmailTemplateHandler.");
        }
    }
}