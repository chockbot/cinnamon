using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;
public class GetMakerActivitiesHandler: IGetMakerActivitiesHandler
{
    private readonly IActivityData activityData;
    public GetMakerActivitiesHandler(IActivityData activityData)
    {
        this.activityData = activityData;
    }

    public AppResult<GetMakerActivitiesResult> Execute(GetMakerActivitiesArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetMakerActivitiesResult>.CreateFailed(ex, "An error occured in GetMakerActivitiesHandler");
        }
    }

    public async Task<AppResult<GetMakerActivitiesResult>> ExecuteAsync(GetMakerActivitiesArgs args)
    {
        try
        {
            var result = await activityData.GetAllActivities(new Framework.ApiCommand.ApiData.Activity.Request.GetAllActivities
            {
                CustomerId = args.CustomerId,
                IncludeAddress = args.IncludeActivityAddress,
                IncludeDescription = args.IncludeActivityDescription,
                IncludeSchedules = args.IncludeAtivitySchedules,
                IncludeImages = args.IncludeActivityImages,
                IncludeSearchTags = args.IncludeActivitySearchTags,
                IsActive = args.IsActive,
                IncludeCustomer = args.IncludeCustomer,
                IncludeStudents = args.IncludeStudents,

            });

            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<GetMakerActivitiesResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if (result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetMakerActivitiesResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetOwnedActivitiesHandler");
            }

            return AppResult<GetMakerActivitiesResult>.CreateSucceeded(new GetMakerActivitiesResult
            {
                Activities = result.Result.Result.Select(a => {
                    return new GetMakerActivitiesResult.Activity
                    {
                        ActivityLevel = a.ActivityLevel,
                        AdditionalRequirements = a.AdditionalRequirements,
                        Address1 = a.Address1,
                        Address2 = a.Address2,
                        CanAdultsJoin = a.CanAdultsJoin,
                        City = a.City,
                        CityName = a.CityName,
                        Region = a.Region,
                        RegionName = a.RegionName,
                        Barangay = a.Barangay,
                        BarangayName = a.BarangayName,
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
                        Handler = a.Handler,
                        IsSetSession = a.IsSetSession,
                        SessionName = a.SessionName,
                        IsNew = a.IsNew,
                        OngoingStudents = a.OngoingStudents,
                        CompletedStudents = a.CompletedStudents,
                        ActivitySchedules = a.Schedules != null ? a.Schedules.Select(s => {
                            return new GetMakerActivitiesResult.Activity.ActivitySchedule
                            {
                                Id = s.Id,
                                DateTime = s.DateTime,
                                Name = s.Name,
                                PerUnit1 = s.PerUnit1,
                                PerUnit2 = s.PerUnit2,
                                Price = s.Price,
                                PriceUnit1 = s.PriceUnit1,
                                PriceUnit2 = s.PriceUnit2,
                                UnitPrice = s.UnitPrice,
                                Order = s.Order,
                                IsActiveSchedule = s.IsActiveSchedule
                            };
                        }) : Enumerable.Empty<GetMakerActivitiesResult.Activity.ActivitySchedule>(),
                        Images = a.Images != null ? a.Images.OrderBy(i => i.Order).Select(i => {
                            return new GetMakerActivitiesResult.Activity.ActivityImage
                            {
                                ImageSrc = i.ImageLocation,
                                Name = i.ImageName,
                                Order = i.Order
                            };
                        }) : Enumerable.Empty<GetMakerActivitiesResult.Activity.ActivityImage>(),
                        Owner = a.Owner != null ? new GetMakerActivitiesResult.Activity.CustomerOwner
                        {
                            Handler = a.Owner.Handler,
                            Id = a.Owner.Id,
                            IsVerified = a.Owner.IsVerified,
                        } : null
                    };
                })
            }, "Successfully get owned activities");
        }
        catch (Exception ex)
        {
            return AppResult<GetMakerActivitiesResult>.CreateFailed(ex, "An error occured in GetMakerActivitiesHandler");
        }
    }
}
