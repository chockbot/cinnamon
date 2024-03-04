using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AdminService.Handlers;
using Cinnamon.Api.Core.Services.AdminService.Interactors;
using Cinnamon.Api.Core.Services.AdminService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AdminService;

public class GetDynamicContentHandler : IGetDynamicContentHandler
{
    private readonly IDynamicContentData dynamicContentData;

    public GetDynamicContentHandler(IDynamicContentData dynamicContentData)
    {
        this.dynamicContentData = dynamicContentData;
    }

    public AppResult<GetDynamicContentResult> Execute(GetDynamicContentArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<GetDynamicContentResult>> ExecuteAsync(GetDynamicContentArgs args)
    {
        try
        {
            var result = await dynamicContentData.GetDynamicContent(args.Identifier);
            if(!result.Succeeded || result.Result is null || !result.Result.IsSuccess)
            {
                return AppResult<GetDynamicContentResult>.CreateFailed(new ApplicationException(result.Result?.ErrorInfo?.Message), result.Message);
            }
            var content = result.Result.Result;

            return AppResult<GetDynamicContentResult>.CreateSucceeded(new GetDynamicContentResult {
                Content = content.Content,
                DateLastUpdated = content.DateLastUpdated,
                Description = content.Description,
                Id = content.Id,
                Identifier = content.Identifier,
                Title = content.Title
            }, "Successfully get dynamic content");
        }
        catch (Exception ex)
        {
            return AppResult<GetDynamicContentResult>.CreateFailed(ex, "An error occured in GetDynamicContentHandler.");
        }
    }
}