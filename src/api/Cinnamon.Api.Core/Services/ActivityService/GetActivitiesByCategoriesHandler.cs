using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;
public class GetActivitiesByCategoriesHandler: IGetActiviesByCategoriesHandler
{
    private readonly IActivityData activityData;
	public GetActivitiesByCategoriesHandler(IActivityData activityData)
	{
		this.activityData = activityData;
	}

    public AppResult<GetActivitiesByCategoriesResult> Execute(GetActivitiesByCategoriesArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetActivitiesByCategoriesResult>.CreateFailed(ex, "An error occured in GetActivitiesByCategoriesHandler");
        }
    }

    public async Task<AppResult<GetActivitiesByCategoriesResult>> ExecuteAsync(GetActivitiesByCategoriesArgs args)
    {
        try
        {
            var result = await activityData.GetActivitiesByCategories(args.CategoryId, new Framework.ApiCommand.ApiData.Activity.Request.GetActivityArgs
            {
                IncludeAddress = args.IncludeActivityAddress,
                IncludeDescription = args.IncludeActivityDescription,
                IncludeImages = args.IncludeActivityImages,
                IncludeSchedules = args.IncludeAtivitySchedules,
                IncludeSearchTags = args.IncludeActivitySearchTags,
                IsActive = args.IsActive
            });

            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<GetActivitiesByCategoriesResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if (result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetActivitiesByCategoriesResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetActivitiesByCategoriesHandler");
            }
            return AppResult<GetActivitiesByCategoriesResult>.CreateSucceeded(new GetActivitiesByCategoriesResult{
                Activities = result.Result.Result.Select(a => {
                    return new GetActivitiesByCategoriesResult.Activity
                    {
                        ActivityLevel = a.ActivityLevel,
                        AdditionalRequirements = a.AdditionalRequirements,
                        Address1 = a.Address1,
                        Address2 = a.Address2,
                        CanAdultsJoin = a.CanAdultsJoin,
                        City = a.City,
                        CustomerBringWithThem = a.CustomerBringWithThem,
                        Description = a.Description,
                        District = a.District,
                        ExperienceCategoryId = a.ExperienceCategoryId,
                        ExperienceTypeId = a.ExperienceTypeId,
                        Id = a.Id,
                        IsPublished = a.IsPublished,
                        MinimumAge = a.MinimumAge,
                        Price = a.Price,
                        Remarks = a.Remarks,
                        SearchTags = a.SearchTags,
                        SkillLevel = a.SkillLevel,
                        SpecificsYouWillProvide = a.SpecificsYouWillProvide,
                        SubCategoryId = a.SubCategoryId,
                        Title = a.Title,
                        ActivitySchedules = a.Schedules != null ? a.Schedules.Select(s => {
                            return new GetActivitiesByCategoriesResult.Activity.ActivitySchedule
                            {
                                DateTime = s.DateTime,
                                Name = s.Name,
                                PerUnit1 = s.PerUnit1,
                                PerUnit2 = s.PerUnit2,
                                Price = s.Price,
                                PriceUnit1 = s.PriceUnit1,
                                PriceUnit2 = s.PriceUnit2,
                                UnitPrice = s.UnitPrice
                            };
                        }) : Enumerable.Empty<GetActivitiesByCategoriesResult.Activity.ActivitySchedule>(),
                        Images = a.Images != null ? a.Images.OrderBy(i => i.Order).Select(i => {
                            return new GetActivitiesByCategoriesResult.Activity.ActivityImage
                            {
                                ImageSrc = i.ImageLocation,
                                Name = i.ImageName,
                                Order = i.Order
                            };
                        }) : Enumerable.Empty<GetActivitiesByCategoriesResult.Activity.ActivityImage>()
                    };
                })
            }, "Successfully get activities by categories");
        }
        catch (Exception ex)
        {
            return AppResult<GetActivitiesByCategoriesResult>.CreateFailed(ex, "An error occured in GetActivitiesByCategoriesHandler");
        }
    }
}
