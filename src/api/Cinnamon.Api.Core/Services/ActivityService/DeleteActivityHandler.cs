using System.Security.Claims;
using AngleSharp.Dom;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;
using Ganss.XSS;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class DeleteActivityHandler : IDeleteActivityHandler
{
    private readonly IHttpContextAccessor httpContext;
    private readonly IActivityData activityData;
    private readonly IExperienceCategoryData categoryData;
    private readonly ISubCategoryData subCategoryData;
    private readonly IExperienceTypeData experienceTypeData;
    private readonly IScheduleData scheduleData;
    private readonly HtmlSanitizer htmlSanitizer;

    public DeleteActivityHandler(IActivityData activityData, IExperienceCategoryData categoryData,
        ISubCategoryData subCategoryData, IExperienceTypeData experienceTypeData, IScheduleData scheduleData,
        IHttpContextAccessor httpContext)
    {
        this.activityData = activityData;
        this.categoryData = categoryData;
        this.subCategoryData = subCategoryData;
        this.experienceTypeData = experienceTypeData;
        this.scheduleData = scheduleData;
        this.httpContext = httpContext;

        this.htmlSanitizer = new 
            HtmlSanitizer(
                allowedTags: new string[] {"p","strong", "em", "ul", "ol", "li", "br"});
    }

    public AppResult<DeleteActivityResult> Execute(DeleteActivityArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<DeleteActivityResult>.CreateFailed(ex, "An error occured in DeleteActivityHandler");
        }
    }

    public async Task<AppResult<DeleteActivityResult>> ExecuteAsync(DeleteActivityArgs args)
    {
        var removeActivity = await activityData.DeleteActivityById(new Framework.ApiCommand.ApiData.Activity.Request.DeleteActivityArgs
        {
            ActivityId = args.ActivityId
        });
        if (!removeActivity.Succeeded || removeActivity.Result == null)
        {
            return AppResult<DeleteActivityResult>.CreateFailed(new ApplicationException(removeActivity.Message), removeActivity.Message);
        }
        if (removeActivity.Succeeded && !removeActivity.Result.IsSuccess)
        {
            return AppResult<DeleteActivityResult>.CreateFailed(
                new ApplicationException(removeActivity.Result.ErrorInfo?.Message), "An error occured in DeleteActivityHandler");
        }

        return AppResult<DeleteActivityResult>.CreateSucceeded(new DeleteActivityResult
        {
            IsSuccess = removeActivity.Result.IsSuccess
        }, "Successfully removed activity");
    }
}