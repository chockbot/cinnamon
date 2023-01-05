using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class GetAllActivitiesHandler:IGetAllActivitiesHandler
{
    private readonly IActivityData activityData;
	public GetAllActivitiesHandler(IActivityData activityData)
	{
		this.activityData = activityData;	
	}

	public AppResult<GetAllActivitiesResult> Execute(GetAllActivitiesArgs args)
	{
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetAllActivitiesResult>.CreateFailed(ex, "An error occured in GetAllActivitiesHandler");
        }
    }

	public async Task<AppResult<GetAllActivitiesResult>> ExecuteAsync(GetAllActivitiesArgs interactor)
	{
		try
		{
			var result = await activityData.GetAllActivities(new Framework.ApiCommand.ApiData.Activity.Request.GetAllActivities { });
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<GetAllActivitiesResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }
            if (result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetAllActivitiesResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetAllActivitiesHandler");
            }
            return AppResult<GetAllActivitiesResult>.CreateSucceeded(new GetAllActivitiesResult
            {
                Activities = result.Result.Result.Select(e => {
                    return new GetAllActivitiesResult.Activity
                    {
                        Id = e.Id,
                        SubTitle = e.SubTitle,
                        Title=e.Title,  
                        Description=e.Description,  
                        SpecificsYouWillProvide=e.SpecificsYouWillProvide,
                        CustomerBringWithThem = e.CustomerBringWithThem,
                        AdditionalRequirements = e.AdditionalRequirements,
                        ActivityLevel = e.ActivityLevel,
                        SkillLevel = e.SkillLevel,
                        MinimumAge = e.MinimumAge,
                        CanAdultsJoin = e.CanAdultsJoin,
                        Price= e.Price,
                        Remarks = e.Remarks,
                        Address1 = e.Address1,
                        Address2 = e.Address2,
                        District = e.District,
                        City = e.City,
                        SearchTags = e.SearchTags,
                        ExperienceType = e.ExperienceType,
                        IsPublished = e.IsPublished,
                        ExperienceCategoryId = e.ExperienceCategoryId,
                        SubCategoryId = e.SubCategoryId
                    };
                })
            }, "Successfully get all activities");
        }
        catch (Exception ex)
		{

			throw;
		}
	}
}
