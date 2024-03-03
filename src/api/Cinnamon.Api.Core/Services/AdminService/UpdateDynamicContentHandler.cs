using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AdminService.Handlers;
using Cinnamon.Api.Core.Services.AdminService.Interactors;
using Cinnamon.Api.Core.Services.AdminService.Interactors.Results;
using Cinnamon.Framework.Common;
using Ganss.XSS;

namespace Cinnamon.Api.Core.Services.AdminService;

public class UpdateDynamicContentHandler : IUpdateDynamicContentHandler
{
    private readonly IDynamicContentData dynamicContentData;
    private readonly HtmlSanitizer htmlSanitizer;
    private readonly IGetProfileHandler getProfileHandler;
    private readonly IAdminUserData adminUserData;

    public UpdateDynamicContentHandler(IDynamicContentData dynamicContentData, HtmlSanitizer htmlSanitizer,
        IGetProfileHandler getProfileHandler, IAdminUserData adminUserData)
    {
        this.dynamicContentData = dynamicContentData;
        this.getProfileHandler = getProfileHandler;
        this.adminUserData = adminUserData;

        this.htmlSanitizer = new 
            HtmlSanitizer(
                allowedTags: new string[] {"p","strong", "em", "ul", "ol", "li", "br", "div", "label", "u", "b", "span", "i"});
    }

    public AppResult<UpdateDynamicContentResult> Execute(UpdateDynamicContentArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<UpdateDynamicContentResult>> ExecuteAsync(UpdateDynamicContentArgs args)
    {
        try
        {
            var getProfileRes = await getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs());
            if(!getProfileRes.Succeeded || getProfileRes.Result == null)
            {
                return AppResult<UpdateDynamicContentResult>.CreateFailed(new ApplicationException(getProfileRes.Message), getProfileRes.Message);
            }

            // check if account is admin
            var adminRes = await adminUserData.GetAdminUserByEmail(new Framework.ApiCommand.ApiData.AdminUser.Request.GetAdminUserByEmailArgs {
                Email = getProfileRes.Result.Email
            });
            if(!adminRes.Succeeded || adminRes.Result == null || !adminRes.Result.IsSuccess)
            {
                return AppResult<UpdateDynamicContentResult>.CreateFailed(
                    new ApplicationException("Action not allowed. Invalid request."), "Action not allowed. Invalid request.");
            }

            var dynamicContentRes = await dynamicContentData.GetDynamicContent(args.Identifier);
        
            // if can't find dynamic content then create it.
            if(!dynamicContentRes.Succeeded || dynamicContentRes.Result is null || !dynamicContentRes.Result.IsSuccess)
            {
                var createRes = await dynamicContentData.CreateDynamicContent(new Framework.ApiCommand.ApiData.DynamicContent.Request.CreateDynamicContentArgs {
                    Content = htmlSanitizer.Sanitize(args.Content),
                    DateLastUpdated = args.DateLastUpdated,
                    Description = htmlSanitizer.Sanitize(args.Description),
                    Identifier = args.Identifier,
                    Title = args.Title
                });
                if(!createRes.Succeeded || createRes.Result is null || !createRes.Result.IsSuccess)
                {
                    return AppResult<UpdateDynamicContentResult>.CreateFailed(new ApplicationException(createRes.Result?.ErrorInfo?.Message), createRes.Message);
                }
                var created = createRes.Result.Result;

                return AppResult<UpdateDynamicContentResult>.CreateSucceeded(new UpdateDynamicContentResult {
                    Content = created.Content,
                    DateLastUpdated = created.DateLastUpdated,
                    Description = created.Description,
                    Id = created.Id,
                    Identifier = created.Identifier,
                    Title = created.Title
                }, "Dynamic content successfully created.");
            }

            var updatedRes = await dynamicContentData.UpdateDynamicContent(new Framework.ApiCommand.ApiData.DynamicContent.Request.UpdateDynamicContentArgs {
                Content = htmlSanitizer.Sanitize(args.Content),
                DateLastUpdated = args.DateLastUpdated,
                Description = htmlSanitizer.Sanitize(args.Description),
                Id = dynamicContentRes.Result.Result.Id,
                Identifier = args.Identifier,
                Title = args.Title
            });
            if(!updatedRes.Succeeded || updatedRes.Result is null || !updatedRes.Result.IsSuccess)
            {
                return AppResult<UpdateDynamicContentResult>.CreateFailed(new ApplicationException(updatedRes.Result?.ErrorInfo?.Message), updatedRes.Message);
            }
            var updated = updatedRes.Result.Result;

            return AppResult<UpdateDynamicContentResult>.CreateSucceeded(new UpdateDynamicContentResult {
                Content = updated.Content,
                DateLastUpdated = updated.DateLastUpdated,
                Description = updated.Description,
                Id = updated.Id,
                Identifier = updated.Identifier,
                Title = updated.Title
            }, "Dynamic content successfully updated.");
        }
        catch (Exception ex)
        {
            return AppResult<UpdateDynamicContentResult>.CreateFailed(ex, "An error occured when updating dynamic content.");
        }
    }
}