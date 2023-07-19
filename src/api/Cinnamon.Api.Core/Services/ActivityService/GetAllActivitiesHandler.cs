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

	public async Task<AppResult<GetAllActivitiesResult>> ExecuteAsync(GetAllActivitiesArgs args)
	{
		try
		{
			var result = await activityData.GetAllActivities(new Framework.ApiCommand.ApiData.Activity.Request.GetAllActivities {
                IncludeAddress = args.IncludeActivityAddress,
                IncludeDescription = args.IncludeActivityDescription,
                IncludeSchedules = args.IncludeAtivitySchedules,
                IncludeImages = args.IncludeActivityImages,
                IncludeSearchTags = args.IncludeActivitySearchTags,
                IsActive = args.IsActive,
                IncludeCustomer = args.IncludeCustomer,
                IncludeExperienceTypes = args.IncludeExperienceTypes,
                IncludeExperienceCategories = args.IncludeExperienceCategories,
                IncludeSubCategories = args.IncludeSubCategories,
                PageIndex = args.PageIndex,
                CountPerPage = args.CountPerPage,
                ExperienceCategoryId  = args.ExperienceCategoryId,
                SearchValue = args.SearchValue,
                IncludeStudents = args.IncludeStudents,
                IsDeactivated = args.IsDeactivated,
                Status = args.Status,
                IsAdmin = args.IsAdmin,
                IncludeReviews = args.IncludeReviews
            });
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
                        Id                      = e.Id,
                        ExperienceCategoryId    = e.ExperienceCategoryId,
                        ExperienceTypeId        = e.ExperienceTypeId,
                        ExperienceType          = e.ExperienceType,
                        SubCategoryId           = e.SubCategoryId,
                        ExperienceCategory      = e.ExperienceCategory,
                        SubCategory             = e.SubCategory,
                        Title                   = e.Title,  
                        Description             = e.Description,  
                        SpecificsYouWillProvide = e.SpecificsYouWillProvide,
                        CustomerBringWithThem   = e.CustomerBringWithThem,
                        AdditionalRequirements  = e.AdditionalRequirements,
                        ActivityLevel           = e.ActivityLevel,
                        SkillLevel              = e.SkillLevel,
                        MinimumAge              = e.MinimumAge,
                        CanAdultsJoin           = e.CanAdultsJoin,
                        Price                   = e.Price,
                        Remarks                 = e.Remarks,
                        Address1                = e.Address1,
                        Address2                = e.Address2,
                        District                = e.District,
                        City                    = e.City,
                        Subdivision             = e.Subdivision,
                        Region                  = e.Region,
                        Barangay                = e.Barangay,
                        BarangayName            = e.BarangayName,
                        RegionName              = e.RegionName,
                        CityName                = e.CityName,
                        PostalCode              = e.PostalCode,
                        SearchTags              = e.SearchTags != null ? e.SearchTags.ToList() : Enumerable.Empty<string>().ToList(),
                        IsPublished             = e.IsPublished,
                        CreatedBy               = e.CreatedBy,
                        MapDetails              = e.MapDetails,
                        Handler                 = e.Handler,
                        IsDeactivated           = e.IsDeactivated,
                        ActivitySchedules       = e.Schedules != null ? e.Schedules.Select(s => {
                            return new GetAllActivitiesResult.Activity.ActivitySchedule
                            {
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
                        }) : Enumerable.Empty<GetAllActivitiesResult.Activity.ActivitySchedule>(),
                        Images = e.Images != null ? e.Images.OrderBy(i => i.Order).Select(i => {
                            return new GetAllActivitiesResult.Activity.ActivityImage
                            {
                                ImageSrc = i.ImageLocation,
                                Name = i.ImageName,
                                Order = i.Order
                            };
                        }) : Enumerable.Empty<GetAllActivitiesResult.Activity.ActivityImage>(),
                        Owner = e.Owner != null ? new GetAllActivitiesResult.Activity.CustomerOwner {
                            Handler = e.Owner.Handler,
                            Id = e.Owner.Id,
                            IsVerified = e.Owner.IsVerified,
                            Email = e.Owner.Email,
                            FirstName = e.Owner.FirstName,
                            LastName = e.Owner.LastName,
                        } : null,
                        IsNew = e.IsNew,
                        CompletedStudents = e.CompletedStudents,
                        OngoingStudents = e.OngoingStudents,
                        AverageRating = e.AverageRating,
                        NumberOfReviews = e.NumberOfReviews,
                    };
                }),
                ErrorInfo = new Framework.ApiCommand.ApiCore.ErrorInfo
                {
                    Code = result?.Result?.ErrorInfo?.Code,
                    Description= result?.Result?.ErrorInfo?.Description,
                    Message = result?.Result?.ErrorInfo?.Message
                },
                Pagination = new Framework.ApiCommand.ApiCore.Pagination
                {
                    PageIndex = result.Result.Pagination.PageIndex,
                    PerPage = result.Result.Pagination.PerPage,
                    TotalPages = result.Result.Pagination.TotalPages,
                    TotalRecords = result.Result.Pagination.TotalRecords 
                }
            }, "Successfully get all activities");
        }
        catch (Exception ex)
		{
            return AppResult<GetAllActivitiesResult>.CreateFailed(ex, "An error occured in GetAllActivitiesHandler");
        }
	}
}
