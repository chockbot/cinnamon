using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class GetActivityHandler : IGetActivityHandler
{
    private readonly IActivityData activityData;

    public GetActivityHandler(IActivityData activityData)
    {
        this.activityData = activityData;
    }

    public AppResult<GetActivityResult> Execute(GetActivityArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetActivityResult>.CreateFailed(ex, "An error occured in GetActivityHandler");
        }
    }

    public async Task<AppResult<GetActivityResult>> ExecuteAsync(GetActivityArgs args)
    {
        try
        {
            var result = await activityData.GetActivityById(args.ActivityId, 
                new Framework.ApiCommand.ApiData.Activity.Request.GetActivityArgs {
                    IncludeAddress = args.IncludeActivityAddress,
                    IncludeDescription = args.IncludeActivityDescription,
                    IncludeImages = args.IncludeActivityImages,
                    IncludeSchedules = args.IncludeAtivitySchedules,
                    IncludeSearchTags = args.IncludeActivitySearchTags,
                    IsActive = args.IsActive,
                    CustomerId = args.CustomerId,
                    IncludeCustomer = args.IncludeCustomer,
                    IncludeStudents = args.IncludeStudents,
                    IncludeTickets = args.IncludeTickets,
                    IncludeAddOns = args.IncludeAddOns,
                    IncludeOteSchedule = true
                }
            );

            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<GetActivityResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if(result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetActivityResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetActivityHandler");
            }
            var activity = result.Result.Result;

            var activityEntity = new GetActivityResult {
                ActivityLevel = activity.ActivityLevel,
                AdditionalRequirements = activity.AdditionalRequirements,
                Address1 = activity.Address1,
                Address2 = activity.Address2,
                CanAdultsJoin = activity.CanAdultsJoin,
                City = activity.City,
                CityName = activity.CityName,
                Subdivision = activity.Subdivision,
                Region= activity.Region,
                RegionName = activity.RegionName,
                PostalCode = activity.PostalCode,   
                Barangay= activity.Barangay,
                BarangayName = activity.BarangayName,
                CustomerBringWithThem = activity.CustomerBringWithThem,
                Description = activity.Description,
                District = activity.District,
                ExperienceCategoryId = activity.ExperienceCategoryId,
                ExperienceTypeId = activity.ExperienceTypeId,
                Id = activity.Id,
                IsPublished = activity.IsPublished,
                MinimumAge = activity.MinimumAge,
                Price = activity.Price,
                Remarks = activity.Remarks,
                SearchTags = activity.SearchTags,
                SkillLevel = activity.SkillLevel,
                SpecificsYouWillProvide = activity.SpecificsYouWillProvide,
                SubCategoryId = activity.SubCategoryId,
                Title = activity.Title,
                CreatedBy = activity.CreatedBy,
                MarDetails = activity.MapDetails,
                Handler = activity.Handler,
                PinnedLocation= activity.PinnedLocation,
                Status = activity.Status,
                ExperienceCreationType = activity.ExperienceCreationType,
                IsComingSoon = activity.IsComingSoon,
                ClassPolicies = activity.ClassPolicies,
                VideoLink = activity.VideoLink,
                ActivitySchedules = activity.Schedules != null ? activity.Schedules.Select(s => {
                    return new GetActivityResult.ActivitySchedule {
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
                        IsActiveSchedule = s.IsActiveSchedule,
                        IsSetSession = s.IsSetSession,
                        SessionName = s.SessionName,
                        HasExpiration  = s.HasExpiration,
                        StartDate = s.StartDate,
                        PriceType = s.PriceType,
                        ScheduleType = s.ScheduleType,
                        SchedulingUrl = s.SchedulingUrl,
                        ActivityScheduleTimes = s.ActivityScheduleTimes.Select(act => new GetActivityResult.ActivityScheduleTime
                        {
                            ActivityScheduleId = act.ActivityScheduleId,
                            ActivityScheduleTimeId = act.ActivityScheduleTimeId,
                            DayOfWeek = act.DayOfWeek,
                            EndTime = act.EndTime,
                            StartTime = act.StartTime,
                            IsEnabled = act.IsEnabled,
                        }).ToList()
                    };
                }) : Enumerable.Empty<GetActivityResult.ActivitySchedule>(),
                Images = activity.Images != null ? activity.Images.OrderBy(i => i.Order).Select(i => {
                    return new GetActivityResult.ActivityImage {
                        Id = i.Id,
                        ImageSrc = i.ImageLocation,
                        Name = i.ImageName,
                        Order = i.Order
                    };
                }) : Enumerable.Empty<GetActivityResult.ActivityImage>(),
                Owner = activity.Owner != null ? new GetActivityResult.CustomerOwner {
                    Handler = activity.Owner.Handler,
                    Id = activity.Owner.Id,
                    FirstName = activity.Owner.FirstName,
                    LastName = activity.Owner.LastName,
                    Email = activity.Owner.Email,
                    ImageSrc = activity.Owner.ProfileImg ?? string.Empty,
                    IsVerified = activity.Owner.IsVerified,
                    IsOG = activity.Owner.IsOG,
                    IsOfficial = activity.Owner.IsOfficial,
                    PhoneNumber = activity.Owner.PhoneNumber,
                } : null,
                Schedule = activity.OteSchedule is not null ? new GetActivityResult.OteSchedule {
                    From = activity.OteSchedule.ScheduleFrom,
                    To = activity.OteSchedule.ScheduleTo
                } : null,
                AddOns = activity.AddOns != null ? activity.AddOns.Select(s => {
                    return new GetActivityResult.AddOn
                    {
                        Id          = s.Id,
                        ActivityId  = s.ActivityId,
                        Name        = s.Name,
                        Price       = s.Price,
                        UnitPrice   = s.UnitPrice,
                        Description = s.Description,
                        Order       = s.Order
                    };
                }) : Enumerable.Empty<GetActivityResult.AddOn>(),
                OngoingStudents = activity.OngoingStudents,
                CompletedStudents = activity.CompletedStudents,
                NumberOfTickets = activity.NumberOfTickets,
                ForceDisable = activity.ForceDisable
            };

            return AppResult<GetActivityResult>.CreateSucceeded(activityEntity, "Successfully get activity");
        }
        catch (Exception ex)
        {
            return AppResult<GetActivityResult>.CreateFailed(ex, "An error occured in GetActivityHandler");
        }
    }
}