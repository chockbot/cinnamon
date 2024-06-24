using AutoMapper;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Framework.ApiCommand.ApiCore;
using Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Activity.Response;
using Cinnamon.Framework.ApiCommand.ApiCore.ExperienceCreationType.Response;
using Cinnamon.Framework.ApiCommand.ApiCore.Favorite.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Favorite.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ActivityResults = Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Api.Core.Services.DashboardService.Handlers;
using CoreDto = Cinnamon.Framework.ApiCommand.ApiCore.DTO;
using System.Globalization;
using Cinnamon.Framework.ApiCommand.ApiCore.DTO.Activity;

namespace Cinnamon.Api.Core.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ActivityController : ControllerBase
{
    private readonly ICreateActivityHandler createActivityHandler;
    private readonly IGetExperienceTypesHandler getExperienceTypesHandler;
    private readonly IGetExperienceCategoriesHandler getExperienceCategoriesHandler;
    private readonly IGetSubCategoriesHandler getSubCategoriesHandler;
    private readonly IGetOwnedActivitiesHandler getOwnedActivitiesHandler;
    private readonly IUpdateActivityHandler updateActivityHandler;
    private readonly IGetAllActivitiesHandler getAllActivitiesHandler;
    private readonly IGetOwnedActivityHandler getOwnedActivityHandler;
    private readonly IUploadActivityImageHandler uploadActivityImageHandler;
    private readonly IGetActivityHandler getActivityHandler;
    private readonly IGetAddressHandler getAddressHandler;
    private readonly IGetActivityImagesHandler getActivityImagesHandler;
    private readonly IGetActiviesByCategoriesHandler getActiviesByCategoriesHandler;
    private readonly IGetActivitiesBySubCategoriesHandler getActivitiesBySubCategoriesHandler;
    private readonly IGetEnrolledActivitiesHandler getEnrolledActivitiesHandler;
    private readonly IUpdateActivityImageOrderHandler updateActivityImageOrderHandler;
    private readonly IGetOwnedActivityByHandler getOwnedActivityByHandler;
    private readonly IGetMakerActivitiesHandler getMakerActivitiesHandler;
    private readonly IGetActivityByHandler getActivityByHandler;
    private readonly IGetAllRegionsHandler getAllRegionsHandler;
    private readonly IGetAllCitiesHandler getAllCitiesHandler;
    private readonly IGetAllBarangaysHandler getAllBarangaysHandler;
    private readonly IGetPopularActivitiesHandler getPopularActivitiesHandler;
    private readonly IGetRefundableExperienceHandler getRefundableExperienceHandler;
    private readonly IUpdateActivityScheduleHandler updateActivityScheduleHandler;
    private readonly IDeleteActivityHandler deleteActivityHandler;
    private readonly IOwnerPricingInclusiveHandler ownerPricingInclusiveHandler;
    private readonly IProviderCreateCouponHandler providerCreateCouponHandler;
    private readonly IGetCouponsHandler getCouponsHandler;
    private readonly IUpdateCouponStatusHandler updateCouponStatusHandler;
    private readonly IValidateCouponCodeHandler validateCouponCodeHandler;
    private readonly IUpdateCouponHandler updateCouponHandler;

    private readonly ICreateFavoriteHandler createFavoriteHandler;
    private readonly IRemoveFavoriteHandler removeFavoriteHandler;
    private readonly IGetFavoritesByCustomerHandler getFavoritesByCustomerHandler;
    private readonly IGetExperienceCreationTypeHandler getExperienceCreationTypeHandler;
    private readonly IGetActivityScheduleTimesHandler getActivityScheduleTimesHandler;
    private readonly ICreateOngoingActivityScheduleHandler createOngoingActivityScheduleHandler;

    private readonly ILogger _logger;
    private readonly IRecommendedActivitiesHandler recommendedActivitiesHandler;
    private readonly IPopularActivitiesHandler popularActivitiesHandler;
    private readonly IOteCreateHandler oteCreateHandler;
    private readonly IOteUpdateHandler oteUpdateHandler;
    private readonly IOteFindByHandler oteFindByHandler;
    private readonly IMapper mapper;
    private readonly IOteTicketDetailsHandler oteTicketDetailsHandler;
    private readonly ICustomerOteHandler customerOteHandler;
    private readonly IOteVerificationHandler oteVerificationHandler;
    private readonly IDeleteAddOnsHandler deleteAddOnsHandler;
    private readonly IDeleteAddOnHandler deleteAddOnHandler;
    private readonly IDeleteTicketHandler deleteTicketHandler;
    private readonly IGetOtePerDayHandler getOtePerDayHandler;
    private readonly IGenerateEventSharedLinkHandler generateEventSharedLinkHandler;
    private readonly IOteValidateSharedLinkHandler oteValidateSharedLinkHandler;
    private readonly IOteSharedLinkVerificationHandler oteSharedLinkVerificationHandler;
    private readonly IDeleteOnlineEventHandler deleteOnlineEventHandler;
    private readonly IOteUpdateSharedLinkStatusHandler oteUpdateSharedLinkStatusHandler;
    private readonly IActivityFeedHandler activityFeedHandler;
    private readonly IOteAlreadyBookedHandler oteAlreadyBookedHandler;
    private readonly IOteTicketBookedCountHandler oteTicketBookedCountHandler;
    private readonly IOteScheduleDatesHandler oteScheduleDatesHandler;

    public ActivityController(ICreateActivityHandler createActivityHandler, IGetExperienceTypesHandler getExperienceTypesHandler,
        IGetExperienceCategoriesHandler getExperienceCategoriesHandler, IGetSubCategoriesHandler getSubCategoriesHandler,
        IGetOwnedActivitiesHandler getOwnedActivitiesHandler, IUpdateActivityHandler updateActivityHandler,
        IGetOwnedActivityHandler getOwnedActivityHandler, IUploadActivityImageHandler uploadActivityImageHandler,
        IGetActivityHandler getActivityHandler, IGetAddressHandler getAddressHandler, IGetAllActivitiesHandler getAllActivitiesHandler,
        IGetActivityImagesHandler getActivityImagesHandler, IGetActiviesByCategoriesHandler getActiviesByCategoriesHandler,
        IGetActivitiesBySubCategoriesHandler getActivitiesBySubCategoriesHandler, IGetEnrolledActivitiesHandler getEnrolledActivitiesHandler,
        IUpdateActivityImageOrderHandler updateActivityImageOrderHandler, IGetOwnedActivityByHandler getOwnedActivityByHandler, IGetMakerActivitiesHandler getMakerActivitiesHandler,
        IGetActivityByHandler getActivityByHandler, IGetAllRegionsHandler getAllRegionsHandler, IGetAllCitiesHandler getAllCitiesHandler,
        IGetAllBarangaysHandler getAllBarangaysHandler, IGetPopularActivitiesHandler getPopularActivitiesHandler, ILogger<ActivityController> logger,
        IGetRefundableExperienceHandler getRefundableExperienceHandler, IUpdateActivityScheduleHandler updateActivityScheduleHandler,
        IDeleteActivityHandler deleteActivityHandler, IOwnerPricingInclusiveHandler ownerPricingInclusiveHandler,
        IProviderCreateCouponHandler providerCreateCouponHandler, IGetCouponsHandler getCouponsHandler,
        IUpdateCouponStatusHandler updateCouponStatusHandler, ICreateFavoriteHandler createFavoriteHandler,
        IRemoveFavoriteHandler removeFavoriteHandler, IGetFavoritesByCustomerHandler getFavoritesByCustomerHandler,
        IValidateCouponCodeHandler validateCouponCodeHandler, IUpdateCouponHandler updateCouponHandler,
        IRecommendedActivitiesHandler recommendedActivitiesHandler, IGetExperienceCreationTypeHandler getExperienceCreationTypeHandler,
        IGetActivityScheduleTimesHandler getActivityScheduleTimesHandler, ICreateOngoingActivityScheduleHandler createOngoingActivityScheduleHandler,
        IPopularActivitiesHandler popularActivitiesHandler, IOteCreateHandler oteCreateHandler, IOteUpdateHandler oteUpdateHandler, 
        IOteFindByHandler oteFindByHandler, IMapper mapper, IOteTicketDetailsHandler oteTicketDetailsHandler,
        ICustomerOteHandler customerOteHandler, IOteVerificationHandler oteVerificationHandler, IDeleteAddOnsHandler deleteAddOnsHandler, 
        IDeleteAddOnHandler deleteAddOnHandler, IGetOtePerDayHandler getOtePerDayHandler, 
        IGenerateEventSharedLinkHandler generateEventSharedLinkHandler, IOteValidateSharedLinkHandler oteValidateSharedLinkHandler,
        IOteSharedLinkVerificationHandler oteSharedLinkVerificationHandler, IDeleteOnlineEventHandler deleteOnlineEventHandler,
        IOteUpdateSharedLinkStatusHandler oteUpdateSharedLinkStatusHandler, IActivityFeedHandler activityFeedHandler, 
        IDeleteTicketHandler deleteTicketHandler, IOteAlreadyBookedHandler oteAlreadyBookedHandler,
        IOteTicketBookedCountHandler oteTicketBookedCountHandler, IOteScheduleDatesHandler oteScheduleDatesHandler)
    {
        _logger = logger;

        this.createActivityHandler = createActivityHandler;
        this.getExperienceTypesHandler = getExperienceTypesHandler;
        this.getExperienceCategoriesHandler = getExperienceCategoriesHandler;
        this.getSubCategoriesHandler = getSubCategoriesHandler;
        this.getAllActivitiesHandler = getAllActivitiesHandler;
        this.getOwnedActivitiesHandler = getOwnedActivitiesHandler;
        this.updateActivityHandler = updateActivityHandler;
        this.getOwnedActivityHandler = getOwnedActivityHandler;
        this.uploadActivityImageHandler = uploadActivityImageHandler;
        this.getAddressHandler = getAddressHandler;
        this.getActivityImagesHandler = getActivityImagesHandler;
        this.getActiviesByCategoriesHandler = getActiviesByCategoriesHandler;
        this.getActivityHandler = getActivityHandler;
        this.getActivitiesBySubCategoriesHandler = getActivitiesBySubCategoriesHandler;
        this.getEnrolledActivitiesHandler = getEnrolledActivitiesHandler;
        this.updateActivityImageOrderHandler = updateActivityImageOrderHandler;
        this.getOwnedActivityByHandler = getOwnedActivityByHandler;
        this.getMakerActivitiesHandler = getMakerActivitiesHandler;
        this.getActivityByHandler = getActivityByHandler;
        this.getAllRegionsHandler = getAllRegionsHandler;
        this.getAllCitiesHandler = getAllCitiesHandler;
        this.getAllBarangaysHandler = getAllBarangaysHandler;
        this.getPopularActivitiesHandler = getPopularActivitiesHandler;
        this.getRefundableExperienceHandler = getRefundableExperienceHandler;
        this.updateActivityScheduleHandler = updateActivityScheduleHandler;
        this.deleteActivityHandler = deleteActivityHandler;
        this.ownerPricingInclusiveHandler = ownerPricingInclusiveHandler;
        this.providerCreateCouponHandler = providerCreateCouponHandler;
        this.getCouponsHandler = getCouponsHandler;
        this.updateCouponStatusHandler = updateCouponStatusHandler;
        this.createFavoriteHandler = createFavoriteHandler;
        this.removeFavoriteHandler = removeFavoriteHandler;
        this.getFavoritesByCustomerHandler = getFavoritesByCustomerHandler;
        this.validateCouponCodeHandler = validateCouponCodeHandler;
        this.updateCouponHandler = updateCouponHandler;
        this.recommendedActivitiesHandler = recommendedActivitiesHandler;
        this.popularActivitiesHandler = popularActivitiesHandler;
        this.getExperienceCreationTypeHandler = getExperienceCreationTypeHandler;
        this.getActivityScheduleTimesHandler = getActivityScheduleTimesHandler;
        this.createOngoingActivityScheduleHandler = createOngoingActivityScheduleHandler;
        this.oteCreateHandler = oteCreateHandler;
        this.oteUpdateHandler = oteUpdateHandler;
        this.oteFindByHandler = oteFindByHandler;
        this.mapper = mapper;
        this.oteTicketDetailsHandler = oteTicketDetailsHandler;
        this.customerOteHandler = customerOteHandler;
        this.oteVerificationHandler = oteVerificationHandler;
        this.deleteAddOnsHandler = deleteAddOnsHandler;
        this.deleteAddOnHandler = deleteAddOnHandler;
        this.getOtePerDayHandler = getOtePerDayHandler;
        this.generateEventSharedLinkHandler = generateEventSharedLinkHandler;
        this.oteValidateSharedLinkHandler = oteValidateSharedLinkHandler;
        this.oteSharedLinkVerificationHandler = oteSharedLinkVerificationHandler;
        this.deleteOnlineEventHandler = deleteOnlineEventHandler;
        this.oteUpdateSharedLinkStatusHandler = oteUpdateSharedLinkStatusHandler;
        this.deleteTicketHandler = deleteTicketHandler;
        this.activityFeedHandler = activityFeedHandler;
        this.oteAlreadyBookedHandler = oteAlreadyBookedHandler;
        this.oteTicketBookedCountHandler = oteTicketBookedCountHandler;
        this.oteScheduleDatesHandler = oteScheduleDatesHandler;
    }

    [Route("CreateActivity")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateActivityResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateActivity([FromBody] CreateActivityArgs args)
    {
        try
        {
            var result = await createActivityHandler.ExecuteAsync(new Services.ActivityService.Interactors.CreateActivityArgs {
                ActivityLevel = args.ActivityLevel,
                ActivitySchedules = args.ActivitySchedules is not null ? args.ActivitySchedules.Select(s => {
                    return new Services.ActivityService.Interactors.CreateActivityArgs.ActivitySchedule {
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
                        SessionName = s.SessionName ?? string.Empty,
                        HasExpiration = s.HasExpiration,
                        StartDate = s.StartDate ?? DateTime.MinValue,
                        ScheduleType = s.ScheduleType,
                        PriceType = s.PriceType,
                        SchedulingUrl = s.SchedulingUrl ?? string.Empty,
                        ActivityScheduleTimes = s.ActivityScheduleTimes is not null ? s.ActivityScheduleTimes.Select(s => new Services.ActivityService.Interactors.CreateActivityArgs.ActivityScheduleTime
                        {
                            DayOfWeek = s.DayOfWeek,
                            EndTime = s.EndTime,
                            StartTime = s.StartTime,
                            IsEnabled = s.IsEnabled
                        }) : Enumerable.Empty<Services.ActivityService.Interactors.CreateActivityArgs.ActivityScheduleTime>()
                    };
                }) : Enumerable.Empty<Services.ActivityService.Interactors.CreateActivityArgs.ActivitySchedule>(),
                AddOns = args.AddOns is not null ? args.AddOns.Select(s => {
                    return new Services.ActivityService.Interactors.CreateActivityArgs.AddOn
                    {
                        ActivityId  = s.ActivityId,
                        Name        = s.Name,
                        Price       = s.Price,
                        UnitPrice   = s.UnitPrice,
                        Description = s.Description,
                        Order       = s.Order
                    };
                }) : Enumerable.Empty<Services.ActivityService.Interactors.CreateActivityArgs.AddOn>(),
                AdditionalRequirements = args.AdditionalRequirements ?? string.Empty,
                Address1 = args.Address1 ?? string.Empty,
                Address2 = args.Address2 ?? string.Empty,
                CanAdultsJoin = args.CanAdultsJoin,
                City = args.City ?? string.Empty,
                Subdivision = args.Subdivision ?? string.Empty,
                Region= args.Region ?? string.Empty,    
                Barangay=args.Barangay ?? string.Empty,
                PostalCode= args.PostalCode ?? string.Empty,    
                CustomerBringWithThem = args.CustomerBringWithThem ?? string.Empty,
                Description = args.Description,
                District = args.District ?? string.Empty,
                ExperienceCategoryId = args.ExperienceCategoryId,
                ExperienceTypeId = args.ExperienceTypeId,
                IsPublished = args.IsPublished,
                MinimumAge = args.MinimumAge,
                Price = args.Price,
                Remarks = args.Remarks ?? string.Empty,
                ScheduleIndicator = args.ScheduleIndicator ?? string.Empty,
                SearchTags = args.SearchTags,
                SkillLevel = args.SkillLevel,
                SpecificsYouWillProvide = args.SpecificsYouWillProvide ?? string.Empty,
                SubCategoryId = args.SubCategoryId,
                Title = args.Title,
                PinnedLocation = args.PinnedLocation ?? string.Empty,
                Status = args.Status,
                ExperienceCreationType = args.ExperienceCreationType,
                ClassPolicies = args.ClassPolicies ?? string.Empty,
                VideoLink = args.VideoLink ?? string.Empty
            });

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreateActivityResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }
            var activity = result.Result;

            return new JsonResult(new CreateActivityResult {IsSuccess = true, Result = new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO {
                ActivityId = activity.ActivityId,
                ActivityLevel = activity.ActivityLevel,
                ActivitySchedules = activity.ActivitySchedules.Select(s => {
                    return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.ActivitySchedule {
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
                        HasExpiration = s.HasExpiration,
                        StartDate = s.StartDate
                    };
                }),
                AdditionalRequirements = activity.AdditionalRequirements,
                Address1 = activity.Address1,
                Address2 = activity.Address2,
                CanAdultsJoin = activity.CanAdultsJoin,
                City = activity.City,
                Subdivision = activity.Subdivision,
                Region = activity.Region,
                Barangay = activity.Barangay,
                PostalCode = activity.PostalCode,
                CustomerBringWithThem = activity.CustomerBringWithThem,
                Description = activity.Description,
                District = activity.District,
                ExperienceCategoryId = activity.ExperienceCategoryId,
                ExperienceTypeId = activity.ExperienceTypeId,
                IsPublished = activity.IsPublished,
                MinimumAge = activity.MinimumAge,
                Price = activity.Price,
                Remarks = activity.Remarks,
                ScheduleIndicator = activity.ScheduleIndicator,
                SearchTags = activity.SearchTags,
                SkillLevel = activity.SkillLevel,
                SpecificsYouWillProvide = activity.SpecificsYouWillProvide,
                SubCategoryId = activity.SubCategoryId,
                Title = activity.Title,
                Handler = activity.Handler,
                ClassPolicies = activity.ClassPlicies,
                VideoLink   = activity.VideoLink
            }
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateActivityResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("UpdateActivity")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateActivityResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> UpdateActivity([FromBody] UpdateActivityArgs args)
    {
        try
        {
            var result = await updateActivityHandler.ExecuteAsync(new Services.ActivityService.Interactors.UpdateActivityArgs {
                ActivityId              = args.ActivityId,
                ActivityLevel           = args.ActivityLevel,
                AdditionalRequirements  = args.AdditionalRequirements,
                Address1                = args.Address1,
                Address2                = args.Address2,
                CanAdultsJoin           = args.CanAdultsJoin,
                City                    = args.City,
                Subdivision             = args.Subdivision,
                Region                  = args.Region ?? string.Empty,
                Barangay                = args.Barangay,
                PostalCode              = args.PostalCode,
                CustomerBringWithThem   = args.CustomerBringWithThem,
                Description             = args.Description,
                District                = args.District,
                ExperienceCategoryId    = args.ExperienceCategoryId,
                ExperienceTypeId        = args.ExperienceTypeId,
                IsPublished             = args.IsPublished,
                MinimumAge              = args.MinimumAge,
                Price                   = args.Price,
                Remarks                 = args.Remarks,
                ScheduleIndicator       = args.ScheduleIndicator,
                SearchTags              = args.SearchTags,
                SkillLevel              = args.SkillLevel,
                SpecificsYouWillProvide = args.SpecificsYouWillProvide,
                ClassPolicies           = args.ClassPolicies,
                SubCategoryId           = args.SubCategoryId,
                Title                   = args.Title,
                PinnedLocation          = args.PinnedLocation,
                IsDeactivated           = args.IsDeactivated,
                IsAdmin                 = args.IsAdmin,
                Status                  = args.Status,
                VideoLink               = args.VideoLink,
                ActivitySchedules = args.ActivitySchedules != null ? 
                    args.ActivitySchedules.Select(s => {
                        return new Services.ActivityService.Interactors.UpdateActivityArgs.ActivitySchedule {
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
                            HasExpiration = s.HasExpiration,
                            StartDate = s.StartDate,
                            PriceType = s.PriceType,
                            ScheduleType = s.ScheduleType,
                            SchedulingUrl = s.SchedulingUrl ?? string.Empty,
                            ActivityScheduleTimes = s.ActivityScheduleTimes is not null ? s.ActivityScheduleTimes.Select(a => new Services.ActivityService.Interactors.UpdateActivityArgs.ActivityScheduleTime
                            {
                                DayOfWeek = a.DayOfWeek,
                                EndTime = a.EndTime,
                                StartTime = a.StartTime,
                                ActivityScheduleId = a.ActivityScheduleId,
                                ActivityScheduleTimeId = a.ActivityScheduleTimeId,
                                ModelStatus = a.ModelStatus,
                                IsEnabled = a.IsEnabled
                            }) : Enumerable.Empty<Services.ActivityService.Interactors.UpdateActivityArgs.ActivityScheduleTime>()
                        };
                    }) : null,
                DeletedScheduleIds  = args.DeletedScheduleIds != null ? args.DeletedScheduleIds : Enumerable.Empty<int>(),
                AddOns = args.AddOns != null ? args.AddOns.Select(a => {
                    return new Services.ActivityService.Interactors.UpdateActivityArgs.AddOn
                    {
                        Id          = a.Id,
                        ActivityId  = a.ActivityId,
                        Name        = a.Name,
                        Description = a.Description,
                        Price       = a.Price,
                        UnitPrice   = a.UnitPrice,
                        Order       = a.Order
                    };
                }) : null,
                DeletedAddOnsIds = args.DeletedAddOnIds != null ? args.DeletedAddOnIds : Enumerable.Empty<int>()
            });
            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateActivityResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }
            var activity = result.Result;

            return new JsonResult(new UpdateActivityResult {IsSuccess = true, Result = new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO {
                ActivityId              = activity.ActivityId,
                ActivityLevel           = activity.ActivityLevel,
                AdditionalRequirements  = activity.AdditionalRequirements,
                Address1                = activity.Address1,
                Address2                = activity.Address2,
                CanAdultsJoin           = activity.CanAdultsJoin,
                City                    = activity.City,
                Subdivision             = activity.Subdivision,
                Region                  = activity.Region,
                Barangay                = activity.Barangay,
                PostalCode              = activity.PostalCode,
                CustomerBringWithThem   = activity.CustomerBringWithThem,
                Description             = activity.Description,
                District                = activity.District,
                ExperienceCategoryId    = activity.ExperienceCategoryId,
                ExperienceTypeId        = activity.ExperienceTypeId,
                IsPublished             = activity.IsPublished,
                MinimumAge              = activity.MinimumAge,
                Price                   = activity.Price,
                Remarks                 = activity.Remarks,
                ScheduleIndicator       = activity.ScheduleIndicator,
                SearchTags              = activity.SearchTags,
                SkillLevel              = activity.SkillLevel,
                SpecificsYouWillProvide = activity.SpecificsYouWillProvide,
                SubCategoryId           = activity.SubCategoryId,
                Title                   = activity.Title,
                Handler                 = activity.Handler,
                Status                  = activity.Status,
                ClassPolicies           = activity.ClassPolicies,
                VideoLink               = activity.VideoLink
            }});
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateActivityResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("GetExperienceTypes")]
    [HttpGet]
    [ProducesResponseType(typeof(GetExperienceTypesResult), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> GetExperienceTypes()
    {
        try
        {
            var result = await getExperienceTypesHandler.ExecuteAsync(new Services.ActivityService.Interactors.GetExperienceTypesArgs {});
            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetExperienceTypesResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }

            return new JsonResult(new GetExperienceTypesResult {Result = result.Result.ExperienceTypes.Select(e => {
                return new Framework.ApiCommand.ApiCore.DTO.ExperienceType.ExperienceTypeDTO {
                    Id = e.Id,
                    Name = e.Name
                };
            }), IsSuccess = true});
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetExperienceTypesResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("GetExperienceCategories")]
    [HttpGet]
    [ProducesResponseType(typeof(GetExperienceCategoriesResult), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> GetExperienceCategories()
    {
        try
        {
            var result = await getExperienceCategoriesHandler.ExecuteAsync(new Services.ActivityService.Interactors.GetExperienceCategoriesArgs{});
            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetExperienceCategoriesResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }

            return new JsonResult(new GetExperienceCategoriesResult {
                IsSuccess = true,
                Result = result.Result.ExperienceCategories.Select(e => {
                    return new Framework.ApiCommand.ApiCore.DTO.ExperienceCategory.ExperienceCategoryDTO {
                        IconPath = e.IconPath,
                        Id = e.Id,
                        Name = e.Category
                    };
                })
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetExperienceCategoriesResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("GetAllAddress")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAddressResult), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllAddress()
    {
        try
        {
            var result = await getAddressHandler.ExecuteAsync(new Services.ActivityService.Interactors.GetAddressArgs { });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAddressResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetAddressResult
            {
                IsSuccess = true,
                Result = result.Result.Addresses.Select(e => {
                    return new Framework.ApiCommand.ApiCore.DTO.Activity.AddressDTO
                    {
                        ActivityId= e.ActivityId,
                        Address1 = e.Address1,
                        Address2 = e.Address2,
                        City = e.City,
                        District = e.District,
                        Id = e.Id
                    };
                })
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetAddressResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetActivityImages")]
    [HttpGet]
    [ProducesResponseType(typeof(GetActivityImagesResult), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllActivityImages()
    {
        try
        {
            var result = await getActivityImagesHandler.ExecuteAsync(new Services.ActivityService.Interactors.GetActivityImagesArgs { });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetActivityImagesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            return new JsonResult(new GetActivityImagesResult
            {
                IsSuccess = true,
                Result = result.Result.ActivityImages.Select(e => {
                    return new Framework.ApiCommand.ApiCore.DTO.Activity.ImagesDTO
                    {
                        ActivityId = e.ActivityId,
                        Id = e.Id,
                        ImageLocation = e.ImageLocation,
                        ImageName = e.ImageName
                    };
                })
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetActivityImagesResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetSubCategories")]
    [HttpGet]
    [ProducesResponseType(typeof(GetSubCategoriesResult), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> GetSubCategories()
    {
        try
        {
            var result = await getSubCategoriesHandler.ExecuteAsync(new Services.ActivityService.Interactors.GetSubCategoriesArgs {});
            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetSubCategoriesResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }

            return new JsonResult(new GetSubCategoriesResult {
                IsSuccess = true,
                Result = result.Result.SubCategories.Select(s => {
                    return new Framework.ApiCommand.ApiCore.DTO.SubCategory.SubCategoryDTO {
                        CategoryId = s.CategoryId,
                        Id = s.Id,
                        Name = s.Name
                    };
                })
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetSubCategoriesResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("GetEnrolledActivities")]
    [HttpGet]
    [ProducesResponseType(typeof(GetEnrolledActivitiesResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEnrolledActivities([FromQuery] GetEnrolledActivitiesArgs args)
    {
        try
        {
            var result = await getEnrolledActivitiesHandler.ExecuteAsync(new Services.ActivityService.Interactors.GetEnrolledActivitiesArgs {
                IncludeActivityAddress = args.IncludeActivityAddress ?? false,
                IncludeActivityDescription = args.IncludeActivityDescription ?? false,
                IncludeActivityImages = args.IncludeActivityImages ?? false,
                IncludeActivitySearchTags = args.IncludeActivitySearchTags ?? false,
                IncludeAtivitySchedules = args.IncludeAtivitySchedules ?? false,
                IsActive = args.IsActive,
                IncludeCustomer = args.IncludeCustomer,
                IncludeStudents = args.IncludeStudents,
            });
            
            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetEnrolledActivitiesResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }

            return new JsonResult(new GetEnrolledActivitiesResult {
                IsSuccess = true,
                Result = result.Result.Activities.Select(a => {
                    return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO {
                        ActivityId = a.Id,
                        ActivityLevel = a.ActivityLevel,
                        ActivitySchedules = a.ActivitySchedules.Select(s => {
                            return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.ActivitySchedule {
                                DateTime = s.DateTime,
                                Name = s.Name,
                                PerUnit1 = s.PerUnit1,
                                PerUnit2 = s.PerUnit2,
                                Price = s.Price,
                                PriceUnit1 = s.PriceUnit1,
                                PriceUnit2 = s.PriceUnit2,
                                UnitPrice = s.UnitPrice,
                                Id = s.Id,
                                Order = s.Order,
                                IsActiveSchedule = s.IsActiveSchedule,
                                IsSetSession = s.IsSetSession,
                                SessionName = s.SessionName,
                                HasExpiration = s.HasExpiration,
                                StartDate = s.StartDate
                            };
                        }),
                        AdditionalRequirements = a.AdditionalRequirements,
                        Address1 = a.Address1,
                        Address2 = a.Address2,
                        PinnedLocation = a.PinnedLocation,
                        CanAdultsJoin = a.CanAdultsJoin,
                        City = a.City,
                        CustomerBringWithThem = a.CustomerBringWithThem,
                        Description = a.Description,
                        District = a.District,
                        ExperienceCategoryId = a.ExperienceCategoryId,
                        ExperienceTypeId = a.ExperienceTypeId,
                        Images = a.Images.Select(i => {
                            return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.ActivityImage {
                                ImageSrc = i.ImageSrc,
                                Name = i.Name,
                                Order = i.Order
                            };
                        }),
                        IsPublished = a.IsPublished,
                        MinimumAge = a.MinimumAge,
                        Price = a.Price,
                        Remarks = a.Remarks,
                        ScheduleIndicator = a.ScheduleIndicator,
                        SearchTags = a.SearchTags,
                        SkillLevel = a.SkillLevel,
                        SpecificsYouWillProvide = a.SpecificsYouWillProvide,
                        SubCategoryId = a.SubCategoryId,
                        Title = a.Title,
                        OngoingStudents = a.OngoingStudents,
                        CompletedStudents = a.CompletedStudents,
                        Owner = a.Owner != null ? new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.CustomerOwner {
                            Handler = a.Owner.Handler,
                            Id = a.Owner.Id,
                            ImageSrc = a.Owner.ImageSrc,
                            FirstName = a.Owner.FirstName,
                            LastName = a.Owner.LastName,
                            IsVerified = a.Owner.IsVerified,
                            IsOG = a.Owner.IsOG,
                            IsOfficial = a.Owner.IsOfficial
                        } : null
                    };
                })
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetEnrolledActivitiesResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("GetOwnedActivities")]
    [HttpGet]
    [ProducesResponseType(typeof(GetOwnedActivitiesResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOwnedActivities([FromQuery] GetOwnedActivitiesArgs args)
    {
        try
        {
            var result = await getOwnedActivitiesHandler.ExecuteAsync(new Services.ActivityService.Interactors.GetOwnedActivitiesArgs {
                IncludeActivityAddress = args.IncludeActivityAddress ?? false,
                IncludeActivityDescription = args.IncludeActivityDescription ?? false,
                IncludeActivityImages = args.IncludeActivityImages ?? false,
                IncludeActivitySearchTags = args.IncludeActivitySearchTags ?? false,
                IncludeAtivitySchedules = args.IncludeAtivitySchedules ?? false,
                IsActive = args.IsActive,
                IncludeCustomer = args.IncludeCustomer,
                IncludeStudents = args.IncludeStudents ?? false
            });
            
            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetOwnedActivitiesResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }

            return new JsonResult(new GetOwnedActivitiesResult {
                IsSuccess = true,
                Result = result.Result.Activities.Select(a => {
                    return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO {
                        ActivityId = a.Id,
                        ActivityLevel = a.ActivityLevel,
                        ActivitySchedules = a.ActivitySchedules.Select(s => {
                            return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.ActivitySchedule {
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
                                HasExpiration = s.HasExpiration,
                                StartDate = s.StartDate
                            };
                        }),
                        AdditionalRequirements = a.AdditionalRequirements,
                        Address1 = a.Address1,
                        Address2 = a.Address2,
                        CanAdultsJoin = a.CanAdultsJoin,
                        City = a.City,
                        CityName = a.CityName,
                        Region= a.Region,
                        RegionName= a.RegionName,
                        Barangay= a.Barangay,
                        BarangayName = a.BarangayName,
                        PinnedLocation = a.PinnedLocation,
                        CustomerBringWithThem = a.CustomerBringWithThem,
                        Description = a.Description,
                        District = a.District,
                        ExperienceCategoryId = a.ExperienceCategoryId,
                        ExperienceTypeId = a.ExperienceTypeId,
                        ExperienceCreationType = a.ExperienceCreationType,
                        CreatedOn = a.CreatedOn,
                        ForceDisable = a.ForceDisable,
                        Images = a.Images.Select(i => {
                            return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.ActivityImage {
                                ImageSrc = i.ImageSrc,
                                Name = i.Name,
                                Order = i.Order
                            };
                        }),
                        IsPublished = a.IsPublished,
                        MinimumAge = a.MinimumAge,
                        Price = a.Price,
                        Remarks = a.Remarks,
                        ScheduleIndicator = a.ScheduleIndicator,
                        SearchTags = a.SearchTags,
                        SkillLevel = a.SkillLevel,
                        SpecificsYouWillProvide = a.SpecificsYouWillProvide,
                        SubCategoryId = a.SubCategoryId,
                        Title = a.Title,
                        Handler = a.Handler,
                        IsNew = a.IsNew,
                        OngoingStudents = a.OngoingStudents,
                        CompletedStudents = a.CompletedStudents,
                        NumberOfReviews = a.NumberOfReviews,
                        Status = a.Status,
                        
                        Owner = a.Owner != null ? new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.CustomerOwner {
                            Handler = a.Owner.Handler,
                            Id  = a.Owner.Id,
                            IsVerified = a.Owner.IsVerified,
                        } : null,
                        
                    };
                })
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetOwnedActivitiesResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("GetMakerActivities")]
    [HttpGet]
    [ProducesResponseType(typeof(GetMakerActivitiesResult), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> GetMakerActivities([FromQuery] GetMakerActivitiesArgs args)
    {
        try
        {
            var result = await getMakerActivitiesHandler.ExecuteAsync(new Services.ActivityService.Interactors.GetMakerActivitiesArgs
            {
                CustomerId = args.CustomerId,
                IncludeActivityAddress = args.IncludeActivityAddress ?? false,
                IncludeActivityDescription = args.IncludeActivityDescription ?? false,
                IncludeActivityImages = args.IncludeActivityImages ?? false,
                IncludeActivitySearchTags = args.IncludeActivitySearchTags ?? false,
                IncludeAtivitySchedules = args.IncludeAtivitySchedules ?? false,
                IsActive = args.IsActive,
                IncludeCustomer = args.IncludeCustomer,
                IncludeStudents = args.IncludeStudents ?? false,
                IncludeReviews = args.IncludeReviews ?? false
            }) ;

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetMakerActivitiesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetMakerActivitiesResult
            {
                IsSuccess = true,
                Result = result.Result.Activities.Select(a => {
                    return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO
                    {
                        ActivityId = a.Id,
                        ActivityLevel = a.ActivityLevel,
                        ActivitySchedules = a.ActivitySchedules.Select(s => {
                            return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.ActivitySchedule
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
                        }),
                        AdditionalRequirements = a.AdditionalRequirements,
                        Address1 = a.Address1,
                        Address2 = a.Address2,
                        CanAdultsJoin = a.CanAdultsJoin,
                        City = a.City,
                        CityName = a.CityName,
                        Region = a.Region,
                        RegionName = a.RegionName,
                        PinnedLocation = a.PinnedLocation,
                        Barangay = a.Barangay,
                        BarangayName = a.BarangayName,
                        CustomerBringWithThem = a.CustomerBringWithThem,
                        Description = a.Description,
                        District = a.District,
                        ExperienceCategoryId = a.ExperienceCategoryId,
                        ExperienceTypeId = a.ExperienceTypeId,
                        Images = a.Images.Select(i => {
                            return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.ActivityImage
                            {
                                ImageSrc = i.ImageSrc,
                                Name = i.Name,
                                Order = i.Order
                            };
                        }),
                        IsPublished = a.IsPublished,
                        MinimumAge = a.MinimumAge,
                        Price = a.Price,
                        Remarks = a.Remarks,
                        ScheduleIndicator = a.ScheduleIndicator,
                        SearchTags = a.SearchTags,
                        SkillLevel = a.SkillLevel,
                        SpecificsYouWillProvide = a.SpecificsYouWillProvide,
                        SubCategoryId = a.SubCategoryId,
                        Title = a.Title,
                        Handler = a.Handler,
                        IsNew = a.IsNew,
                        OngoingStudents = a.OngoingStudents,
                        CompletedStudents = a.CompletedStudents,
                        AverageRating = a.AverageRating,
                        NumberOfReviews = a.NumberOfReviews,
                        ExperienceCreationType = a.ExperienceCreationType,
                        Owner = a.Owner != null ? new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.CustomerOwner
                        {
                            Handler = a.Owner.Handler,
                            Id = a.Owner.Id,
                            IsVerified = a.Owner.IsVerified,
                        } : null
                    };
                })
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetMakerActivitiesResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetAllActivities")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllActivitiesResult), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllActivities([FromQuery] GetAllActivitiesArgs args)
    {
        try
        {
            var result = await getAllActivitiesHandler.ExecuteAsync(new Services.ActivityService.Interactors.GetAllActivitiesArgs
            {
                IncludeActivityAddress = args.IncludeActivityAddress ?? false,
                IncludeActivityDescription = args.IncludeActivityDescription ?? false,
                IncludeActivityImages = args.IncludeActivityImages ?? false,
                IncludeActivitySearchTags = args.IncludeActivitySearchTags ?? false,
                IncludeAtivitySchedules = args.IncludeAtivitySchedules ?? false,
                IsActive = args.IsActive,
                IncludeCustomer = args.IncludeCustomer,
                IncludeExperienceTypes = args.IncludeExperienceTypes ?? false,
                IncludeExperienceCategories = args.IncludeExperienceCategories ?? false,
                IncludeSubCategories = args.IncludeSubCategories ?? false,
                PageIndex = args.PageIndex,
                CountPerPage = args.CountPerPage,
                SearchValue= string.IsNullOrEmpty(args.SearchValue) ? string.Empty : args.SearchValue,
                ExperienceCategoryId = args.ExperienceCategoryId.GetValueOrDefault(),
                IncludeStudents = args.IncludeStudents ?? false,
                IsDeactivated = args.IsDeactivated,
                Status = args.Status,
                IsAdmin = args.IsAdmin,
                IncludeReviews = args.IncludeReviews ?? false,
                IncludeTickets = args.IncludeTickets ?? false,
                ForceDisable = args.ForceDisable
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllActivitiesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetAllActivitiesResult
            {
                IsSuccess = true,
                Pagination = result.Result.Pagination,
                ErrorInfo = result.Result.ErrorInfo,
                Result = result.Result.Activities.Select(a => {
                    return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO
                    {
                        ActivityId = a.Id,
                        ActivityLevel = a.ActivityLevel,
                        ActivitySchedules = a.ActivitySchedules.Select(s => {
                            return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.ActivitySchedule
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
                        }),
                        AdditionalRequirements = a.AdditionalRequirements,
                        Address1 = a.Address1,
                        Address2 = a.Address2,
                        CanAdultsJoin = a.CanAdultsJoin,
                        City = a.City,
                        Subdivision = a.Subdivision,
                        Region = a.Region,  
                        Barangay = a.Barangay,
                        CityName = a.CityName,
                        BarangayName = a.BarangayName,
                        RegionName = a.RegionName,
                        PostalCode = a.PostalCode,
                        PinnedLocation = a.PinnedLocation,
                        CustomerBringWithThem = a.CustomerBringWithThem,
                        Description = a.Description,
                        District = a.District,
                        ExperienceCategoryId = a.ExperienceCategoryId,
                        ExperienceCategory = a.ExperienceCategory,
                        SubCategory = a.SubCategory,
                        ExperienceTypeId = a.ExperienceTypeId,
                        ExperienceType = a.ExperienceType,
                        CreatedBy = a.CreatedBy,
                        Images = a.Images.Select(i => {
                            return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.ActivityImage
                            {
                                ImageSrc = i.ImageSrc,
                                Name = i.Name,
                                Order = i.Order
                            };
                        }),
                        IsPublished = a.IsPublished,
                        MinimumAge = a.MinimumAge,
                        Price = a.Price,
                        Remarks = a.Remarks,
                        ScheduleIndicator = a.ScheduleIndicator,
                        SearchTags = a.SearchTags,
                        SkillLevel = a.SkillLevel,
                        SpecificsYouWillProvide = a.SpecificsYouWillProvide,
                        SubCategoryId = a.SubCategoryId,
                        Title = a.Title,
                        Handler = a.Handler,
                        Owner = a.Owner != null ? new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.CustomerOwner {
                            Handler = a.Owner.Handler,
                            Id  = a.Owner.Id,
                            IsVerified= a.Owner.IsVerified,
                            Email = a.Owner.Email,
                            FirstName = a.Owner.FirstName,
                            LastName = a.Owner.LastName,
                        } : null,
                        IsNew = a.IsNew,
                        CompletedStudents = a.CompletedStudents,
                        OngoingStudents = a.OngoingStudents,
                        IsDeactivated = a.IsDeactivated,
                        NumberOfReviews = a.NumberOfReviews,
                        AverageRating = a.AverageRating,
                        ExperienceCreationType = a.ExperienceCreationType,
                        NumberOfTickets = a.NumberOfTickets
                    };
                }).AsQueryable()
            });
        }
        catch (Exception ex )
        {
            return new JsonResult(new GetAllActivitiesResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetOwnedActivity/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetActivityResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOwnedActivity(int id, [FromQuery] GetActivityArgs args)
    {
        try
        {
            var result = await getOwnedActivityHandler.ExecuteAsync(new Services.ActivityService.Interactors.GetOwnedActivityArgs {
                ActivityId = id,
                IncludeActivityAddress = args.IncludeActivityAddress ?? false,
                IncludeActivityDescription = args.IncludeActivityDescription ?? false,
                IncludeActivityImages = args.IncludeActivityImages ?? false,
                IncludeActivitySearchTags = args.IncludeActivitySearchTags ?? false,
                IncludeAtivitySchedules = args.IncludeAtivitySchedules ?? false,
                IsActive = args.IsActive,
                IncludeCustomer = args.IncludeCustomer,
                IncludeStudents = args.IncludeStudents,
                IncludeAddOns = args.IncludeAddOns
            });
            
            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetActivityResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }
            var activity = result.Result;

            return new JsonResult(new GetActivityResult {
                IsSuccess = true,
                Result = new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO {
                    ActivityId = activity.Id,
                    ActivityLevel = activity.ActivityLevel,
                    ActivitySchedules = activity.ActivitySchedules.Select(s => {
                        return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.ActivitySchedule {
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
                            HasExpiration = s.HasExpiration,
                            StartDate = s.StartDate,
                            PriceType = s.PriceType,
                            ScheduleType = s.ScheduleType,
                            SchedulingUrl = s.SchedulingUrl,
                            ActivityScheduleTimes = s.ActivityScheduleTimes.Select(act => new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.ActivityScheduleTimeModelDTO
                            {
                                ActivityScheduleId = act.ActivityScheduleId,
                                ActivityScheduleTimeId = act.ActivityScheduleTimeId,
                                DayOfWeek = act.DayOfWeek,
                                EndTime = act.EndTime,
                                StartTime = act.StartTime,
                                IsEnabled = act.IsEnabled
                            }).ToList()
                        };
                    }),
                    AddOns = activity.AddOns.Select(s =>
                    {
                        return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.AddOn {
                            Id          = s.Id,
                            ActivityId  = s.ActivityId,
                            Name        = s.Name,
                            Price       = s.Price,
                            UnitPrice   = s.UnitPrice,
                            Description = s.Description,
                            Order       = s.Order
                        };
                    }),
                    ClassPolicies = activity.ClassPolicies,
                    AdditionalRequirements = activity.AdditionalRequirements,
                    Address1 = activity.Address1,
                    Address2 = activity.Address2,
                    CanAdultsJoin = activity.CanAdultsJoin,
                    City = activity.City,
                    CityName = activity.CityName,
                    Subdivision = activity.Subdivision,
                    Region = activity.Region,
                    RegionName = activity.RegionName,
                    Barangay = activity.Barangay,
                    BarangayName = activity.BarangayName,
                    PostalCode = activity.PostalCode,
                    CustomerBringWithThem = activity.CustomerBringWithThem,
                    Description = activity.Description,
                    District = activity.District,
                    ExperienceCategoryId = activity.ExperienceCategoryId,
                    ExperienceTypeId = activity.ExperienceTypeId,
                    Images = activity.Images.Select(i => {
                        return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.ActivityImage {
                            Id = i.Id,
                            ImageSrc = i.ImageSrc,
                            Name = i.Name,
                            Order = i.Order
                        };
                    }),
                    IsPublished = activity.IsPublished,
                    MinimumAge = activity.MinimumAge,
                    Price = activity.Price,
                    Remarks = activity.Remarks,
                    ScheduleIndicator = activity.ScheduleIndicator,
                    SearchTags = activity.SearchTags,
                    SkillLevel = activity.SkillLevel,
                    SpecificsYouWillProvide = activity.SpecificsYouWillProvide,
                    SubCategoryId = activity.SubCategoryId,
                    Title = activity.Title,
                    Handler = activity.Handler,
                    PinnedLocation= activity.PinnedLocation,
                    Status = activity.Status,
                    ExperienceCreationType = activity.ExperienceCreationType,
                    Owner           = activity.Owner != null ? new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.CustomerOwner {
                            Handler = activity.Owner.Handler,
                            Id      = activity.Owner.Id
                        } : null,
                    CompletedStudents = activity.CompletedStudents,
                    OngoingStudents   = activity.OngoingStudents,
                    VideoLink         = activity.VideoLink
                }
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetActivityResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("GetOwnedActivityByHandler/{handler}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetActivityResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOwnedActivityByHandler(string handler, [FromQuery] GetActivityArgs args)
    {
        try
        {
            var result = await getOwnedActivityByHandler.ExecuteAsync(new Services.ActivityService.Interactors.GetOwnedActivityByHandlerArgs {
                Handler = handler,
                IncludeActivityAddress = args.IncludeActivityAddress ?? false,
                IncludeActivityDescription = args.IncludeActivityDescription ?? false,
                IncludeActivityImages = args.IncludeActivityImages ?? false,
                IncludeActivitySearchTags = args.IncludeActivitySearchTags ?? false,
                IncludeAtivitySchedules = args.IncludeAtivitySchedules ?? false,
                IsActive = args.IsActive,
                IncludeCustomer = args.IncludeCustomer,
                IncludeStudents = args.IncludeStudents
            });
            
            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetActivityResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }
            var activity = result.Result;

            return new JsonResult(new GetActivityResult {
                IsSuccess = true,
                Result = new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO {
                    ActivityId = activity.Id,
                    ActivityLevel = activity.ActivityLevel,
                    ActivitySchedules = activity.ActivitySchedules.Select(s => {
                        return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.ActivitySchedule {
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
                    }),
                    ClassPolicies = activity.ClassPolicies,
                    AdditionalRequirements = activity.AdditionalRequirements,
                    Address1 = activity.Address1,
                    Address2 = activity.Address2,
                    CanAdultsJoin = activity.CanAdultsJoin,
                    City = activity.City,
                    Subdivision = activity.Subdivision,
                    Region = activity.Region,
                    Barangay = activity.Barangay,
                    PostalCode = activity.PostalCode,
                    CustomerBringWithThem = activity.CustomerBringWithThem,
                    Description = activity.Description,
                    District = activity.District,
                    ExperienceCategoryId = activity.ExperienceCategoryId,
                    ExperienceTypeId = activity.ExperienceTypeId,
                    Images = activity.Images.Select(i => {
                        return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.ActivityImage {
                            Id = i.Id,
                            ImageSrc = i.ImageSrc,
                            Name = i.Name,
                            Order = i.Order
                        };
                    }),
                    IsPublished = activity.IsPublished,
                    MinimumAge = activity.MinimumAge,
                    Price = activity.Price,
                    Remarks = activity.Remarks,
                    ScheduleIndicator = activity.ScheduleIndicator,
                    SearchTags = activity.SearchTags,
                    SkillLevel = activity.SkillLevel,
                    SpecificsYouWillProvide = activity.SpecificsYouWillProvide,
                    SubCategoryId = activity.SubCategoryId,
                    Title = activity.Title,
                    Handler = activity.Handler,
                    Owner = activity.Owner != null ? new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.CustomerOwner {
                            Handler = activity.Owner.Handler,
                            Id  = activity.Owner.Id
                        } : null,
                    CompletedStudents = activity.CompletedStudents,
                    OngoingStudents = activity.CompletedStudents
                    
                }
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetActivityResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("GetActivity/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetActivityResult), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> GetActivity(int id, [FromQuery] GetActivityArgs args)
    {
        try
        {
            var result = await getActivityHandler.ExecuteAsync(new Services.ActivityService.Interactors.GetActivityArgs {
                ActivityId = id,
                IncludeActivityAddress = args.IncludeActivityAddress ?? false,
                IncludeActivityDescription = args.IncludeActivityDescription ?? false,
                IncludeActivityImages = args.IncludeActivityImages ?? false,
                IncludeActivitySearchTags = args.IncludeActivitySearchTags ?? false,
                IncludeAtivitySchedules = args.IncludeAtivitySchedules ?? false,
                IsActive = args.IsActive,
                IncludeCustomer = args.IncludeCustomer,
                IncludeStudents = args.IncludeStudents,
                IncludeTickets = args.IncludeTickets ?? false,
                IncludeAddOns = args.IncludeAddOns ?? false
            });
            
            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetActivityResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }
            var activity = result.Result;

            return new JsonResult(new GetActivityResult {
                IsSuccess = true,
                Result = new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO {
                    ActivityId = activity.Id,
                    ActivityLevel = activity.ActivityLevel,
                    ActivitySchedules = activity.ActivitySchedules.Select(s => {
                        return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.ActivitySchedule {
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
                            HasExpiration = s.HasExpiration,
                            StartDate = s.StartDate,
                            PriceType = s.PriceType,
                        };
                    }),
                    AdditionalRequirements = activity.AdditionalRequirements,
                    Address1 = activity.Address1,
                    Address2 = activity.Address2,
                    CanAdultsJoin = activity.CanAdultsJoin,
                    City = activity.City,
                    CityName = activity.CityName,
                    Subdivision = activity.Subdivision,
                    Region = activity.Region,
                    RegionName = activity.RegionName,
                    Barangay = activity.Barangay,
                    BarangayName = activity.BarangayName,
                    PostalCode = activity.PostalCode,
                    CustomerBringWithThem = activity.CustomerBringWithThem,
                    ClassPolicies = activity.ClassPolicies,
                    Description = activity.Description,
                    District = activity.District,
                    ExperienceCategoryId = activity.ExperienceCategoryId,
                    ExperienceTypeId = activity.ExperienceTypeId,
                    CreatedBy = activity.CreatedBy,
                    VideoLink = activity.VideoLink,
                    Images = activity.Images.Select(i => {
                        return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.ActivityImage {
                            Id = i.Id,
                            ImageSrc = i.ImageSrc,
                            Name = i.Name,
                            Order = i.Order
                        };
                    }),
                    IsPublished = activity.IsPublished,
                    MinimumAge = activity.MinimumAge,
                    Price = activity.Price,
                    Remarks = activity.Remarks,
                    ScheduleIndicator = activity.ScheduleIndicator,
                    SearchTags = activity.SearchTags,
                    SkillLevel = activity.SkillLevel,
                    SpecificsYouWillProvide = activity.SpecificsYouWillProvide,
                    SubCategoryId = activity.SubCategoryId,
                    Title = activity.Title,
                    Handler = activity.Handler,
                    PinnedLocation = activity.PinnedLocation,
                    ExperienceCreationType = activity.ExperienceCreationType,
                    Owner = activity.Owner != null ? new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.CustomerOwner {
                        Handler = activity.Owner.Handler,
                        Id = activity.Owner.Id,
                        ImageSrc = activity.Owner.ImageSrc,
                        FirstName = activity.Owner.FirstName,
                        LastName = activity.Owner.LastName,
                        IsVerified = activity.Owner.IsVerified,
                        IsOG = activity.Owner.IsOG,
                        IsOfficial = activity.Owner.IsOfficial,
                        Email = activity.Owner.Email,
                        PhoneNumber = activity.Owner.PhoneNumber
                    } : null,
                    AddOns = activity.AddOns.Select(s =>
                    {
                        return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.AddOn
                        {
                            Id          = s.Id,
                            ActivityId  = s.ActivityId,
                            Name        = s.Name,
                            Price       = s.Price,
                            UnitPrice   = s.UnitPrice,
                            Description = s.Description,
                            Order       = s.Order
                        };
                    }),
                    OngoingStudents = activity.OngoingStudents,
                    CompletedStudents = activity.CompletedStudents,
                    IsComingSoon = activity.IsComingSoon,
                    NumberOfTickets = activity.NumberOfTickets
                }
            }); 
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetActivityResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("GetActivityByHandler/{handler}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetActivityResult), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> GetActivityByHandler(string handler, [FromQuery] GetActivityArgs args)
    {
        try
        {
            var result = await getActivityByHandler.ExecuteAsync(new Services.ActivityService.Interactors.GetActivityByHandlerArgs {
                Handler = handler,
                IncludeActivityAddress = args.IncludeActivityAddress ?? false,
                IncludeActivityDescription = args.IncludeActivityDescription ?? false,
                IncludeActivityImages = args.IncludeActivityImages ?? false,
                IncludeActivitySearchTags = args.IncludeActivitySearchTags ?? false,
                IncludeAtivitySchedules = args.IncludeAtivitySchedules ?? false,
                IsActive = args.IsActive,
                IncludeCustomer = args.IncludeCustomer,
                IncludeStudents = args.IncludeStudents,
                IncludeAddOns = args.IncludeAddOns
            });
            
            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetActivityResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }
            var activity = result.Result;

            return new JsonResult(new GetActivityResult {
                IsSuccess = true,
                Result = new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO {
                    ActivityId = activity.Id,
                    ActivityLevel = activity.ActivityLevel,
                    ActivitySchedules = activity.ActivitySchedules.Select(s => {
                        return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.ActivitySchedule {
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
                            SessionName  = s.SessionName,
                            HasExpiration = s.HasExpiration,
                            StartDate = s.StartDate,
                            PriceType = s.PriceType,
                            ScheduleType = s.ScheduleType,
                            SchedulingUrl = s.SchedulingUrl
                        };
                    }),
                    AddOns = activity.AddOns.Select(s => {
                        return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.AddOn
                        {
                            Id = s.Id,
                            ActivityId = s.ActivityId,
                            Name = s.Name,
                            Price = s.Price,
                            UnitPrice = s.UnitPrice,
                            Description = s.Description,
                            Order = s.Order
                        };
                    }),
                    AdditionalRequirements = activity.AdditionalRequirements,
                    Address1 = activity.Address1,
                    Address2 = activity.Address2,
                    CanAdultsJoin = activity.CanAdultsJoin,
                    City = activity.City,
                    CityName = activity.CityName,
                    Subdivision = activity.Subdivision,
                    Region = activity.Region,
                    RegionName = activity.RegionName,
                    Barangay = activity.Barangay,
                    BarangayName = activity.BarangayName,
                    PostalCode = activity.PostalCode,
                    CustomerBringWithThem = activity.CustomerBringWithThem,
                    ClassPolicies = activity.ClassPolicies,
                    Description = activity.Description,
                    District = activity.District,
                    ExperienceCategoryId = activity.ExperienceCategoryId,
                    ExperienceTypeId = activity.ExperienceTypeId,
                    CreatedBy = activity.CreatedBy,
                    Images = activity.Images.Select(i => {
                        return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.ActivityImage {
                            Id = i.Id,
                            ImageSrc = i.ImageSrc,
                            Name = i.Name,
                            Order = i.Order
                        };
                    }),
                    IsPublished = activity.IsPublished,
                    MinimumAge = activity.MinimumAge,
                    Price = activity.Price,
                    Remarks = activity.Remarks,
                    ScheduleIndicator = activity.ScheduleIndicator,
                    SearchTags = activity.SearchTags,
                    SkillLevel = activity.SkillLevel,
                    SpecificsYouWillProvide = activity.SpecificsYouWillProvide,
                    SubCategoryId = activity.SubCategoryId,
                    Title = activity.Title,
                    Handler = activity.Handler,
                    PinnedLocation = activity.PinnedLocation,
                    ExperienceCreationType = activity.ExperienceCreationType,
                    Owner = activity.Owner != null ? new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.CustomerOwner {
                            Handler = activity.Owner.Handler,
                            Id  = activity.Owner.Id
                        } : null,
                    CompletedStudents = activity.CompletedStudents,
                    OngoingStudents = activity.OngoingStudents,
                    IsComingSoon = activity.IsComingSoon,
                    VideoLink = activity.VideoLink
                }
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetActivityResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("UploadActivityImage")]
    [HttpPost]
    [ProducesResponseType(typeof(UploadActivityImageResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> UploadActivityImage([FromForm] UploadActivityImageArgs args)
    {
        try
        {
            var result = await uploadActivityImageHandler.ExecuteAsync(new Services.ActivityService.Interactors.UploadActivityImageArgs {
                ActivityId = args.ActivityId,
                Images = args.Images,
                DeletedIds = args.DeletedIds,
                Orders = args.Orders
            });

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UploadActivityImageResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }

            return new JsonResult(new UploadActivityImageResult {IsSuccess = true, Result = true});
        }
        catch (Exception ex)
        {
            return new JsonResult(new UploadActivityImageResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("UpdateActivityImageOrder")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateActivityImageOrderResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> UpdateActivityImageOrder([FromBody] UpdateActivityImageOrderArgs args)
    {
        try
        {
            var orderResult = await updateActivityImageOrderHandler.ExecuteAsync(new Services.ActivityService.Interactors.UpdateActivityImageOrderArgs {
                ActivityId = args.ActivityId,
                ImageOrders = args.ImageOrders.Select(i => {
                    return new Services.ActivityService.Interactors.UpdateActivityImageOrderArgs.ImageOrder  {
                        NewOrder = i.NewOrder,
                        OldOrder = i.OldOrder
                    };
                }).ToList()
            });

            if(!orderResult.Succeeded || orderResult.Result == null)
            {
                return new JsonResult(new UpdateActivityImageOrderResult {ErrorInfo = new ErrorInfo {Message = orderResult.Message}});
            }

            return new JsonResult(new UpdateActivityImageOrderResult {IsSuccess = true, Result = new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityImagesDTO {
                Image1 = orderResult.Result.Image1Path,
                Image2 = orderResult.Result.Image2Path,
                Image3 = orderResult.Result.Image3Path
            }});
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateActivityImageOrderResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("GetActivitiesByCategories/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetActivitiesByCategoriesResult), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> GetActivitiesByCategories(int id, [FromQuery] GetActivityArgs args)
    {
        try
        {
            var result = await getActiviesByCategoriesHandler.ExecuteAsync(new Services.ActivityService.Interactors.GetActivitiesByCategoriesArgs{
                CategoryId= id,
                IncludeActivityAddress = args.IncludeActivityAddress ?? false,
                IncludeActivityDescription = args.IncludeActivityDescription ?? false,
                IncludeActivityImages = args.IncludeActivityImages ?? false,
                IncludeActivitySearchTags = args.IncludeActivitySearchTags ?? false,
                IncludeAtivitySchedules = args.IncludeAtivitySchedules ?? false,
                IsActive = args.IsActive,
                IncludeCustomer = args.IncludeCustomer
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetActivitiesByCategoriesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetActivitiesByCategoriesResult
            {
                IsSuccess = true,
                Result = result.Result.Activities.Select(a => {
                    return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO
                    {
                        ActivityId = a.Id,
                        ActivityLevel = a.ActivityLevel,
                        ActivitySchedules = a.ActivitySchedules.Select(s => {
                            return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.ActivitySchedule
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
                        }),
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
                        Images = a.Images.Select(i => {
                            return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.ActivityImage
                            {
                                ImageSrc = i.ImageSrc,
                                Name = i.Name,
                                Order = i.Order
                            };
                        }),
                        IsPublished = a.IsPublished,
                        MinimumAge = a.MinimumAge,
                        Price = a.Price,
                        Remarks = a.Remarks,
                        ScheduleIndicator = a.ScheduleIndicator,
                        SearchTags = a.SearchTags,
                        SkillLevel = a.SkillLevel,
                        SpecificsYouWillProvide = a.SpecificsYouWillProvide,
                        SubCategoryId = a.SubCategoryId,
                        Title = a.Title,
                        Owner = a.Owner != null ? new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.CustomerOwner {
                            Handler = a.Owner.Handler,
                            Id  = a.Owner.Id
                        } : null
                    };
                })
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetActivitiesByCategoriesResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("Regions")]
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(GetAllRegionsResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllRegions([FromQuery] GetAllRegionsArgs args)
    {
        try
        {
            var result = await getAllRegionsHandler.ExecuteAsync(new Services.ActivityService.Interactors.GetAllRegionsArgs
            {
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllRegionsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetAllRegionsResult
            {
                IsSuccess = true,
                Result = result.Result.Regions.Select(s =>
                {
                    return new Framework.ApiCommand.ApiCore.DTO.Location.RegionDTO
                    {
                        Code = s.Code,
                        Name = s.Name,
                        RegionName = s.RegionName,
                    };
                })
            }
            );
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetAllRegionsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("Cities")]
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(GetAllCitiesResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCitiesByRegionCode([FromQuery] GetAllCitiesArgs args)
    {
        try
        {
            var result = await getAllCitiesHandler.ExecuteAsync(new Services.ActivityService.Interactors.GetAllCitiesArgs
            {
                RegionCode = args.RegionCode,
                CountPerPage = args.CountPerPage,
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllCitiesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetAllCitiesResult
            {
                IsSuccess = true,
                Result = result.Result.Cities.Select(s =>
                {
                    return new Framework.ApiCommand.ApiCore.DTO.Location.CityDTO
                    {
                        Code           = s.Code,
                        Name           = s.Name,
                        RegionCode     = s.RegionCode,
                        IsMunicipality = s.IsMunicipality,
                        IsCity         = s.IsCity
                    };
                })
            }
            );
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetAllCitiesResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("Barangays")]
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(GetAllBarangaysResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllBarangaysByCityCode([FromQuery] GetAllBarangaysArgs args)
    {
        try
        {
            var result = await getAllBarangaysHandler.ExecuteAsync(new Services.ActivityService.Interactors.GetAllBarangaysArgs
            {
                CityCode = args.CityCode,
                CountPerPage = args.CountPerPage,
                IsCity = args.IsCity,
                IsMunicipality = args.IsMunicipality
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllBarangaysResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetAllBarangaysResult
            {
                IsSuccess = true,
                Result = result.Result.Barangays.Select(s =>
                {
                    return new Framework.ApiCommand.ApiCore.DTO.Location.BarangayDTO
                    {
                        Code = s.Code,
                        Name = s.Name,
                        CityCode = s.CityCode,
                    };
                })
            }
            );
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetAllBarangaysResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("Popular")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllActivitiesResult), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> GetPopularActivities([FromQuery] GetAllActivitiesArgs args)
    {
        _logger.LogInformation("Cinnamon.Api.Core > GetPopularActivities was called");

        try
        {
            var result = await getPopularActivitiesHandler.ExecuteAsync(new Services.ActivityService.Interactors.GetAllActivitiesArgs
            {
                IncludeActivityAddress = args.IncludeActivityAddress ?? false,
                IncludeActivityDescription = args.IncludeActivityDescription ?? false,
                IncludeActivityImages = args.IncludeActivityImages ?? false,
                IncludeActivitySearchTags = args.IncludeActivitySearchTags ?? false,
                IncludeAtivitySchedules = args.IncludeAtivitySchedules ?? false,
                IsActive = args.IsActive,
                IncludeCustomer = args.IncludeCustomer,
                IncludeExperienceTypes = args.IncludeExperienceTypes ?? false,
                IncludeExperienceCategories = args.IncludeExperienceCategories ?? false,
                IncludeSubCategories = args.IncludeSubCategories ?? false,
                PageIndex = args.PageIndex,
                CountPerPage = args.CountPerPage,
                IncludeStudents = args.IncludeStudents ?? false,
                IsDeactivated = args.IsDeactivated,
                IncludeReviews = args.IncludeReviews ?? false,
                IncludeTickets = args.IncludeTickets ?? false
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllActivitiesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetAllActivitiesResult
            {
                IsSuccess  = true,
                Pagination = result.Result.Pagination,
                ErrorInfo  = result.Result.ErrorInfo,
                Result = result.Result.Activities.Select(a => {
                    return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO
                    {
                        ActivityId    = a.Id,
                        ActivityLevel = a.ActivityLevel,
                        ActivitySchedules = a.ActivitySchedules.Select(s => {
                            return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.ActivitySchedule
                            {
                                DateTime         = s.DateTime,
                                Name             = s.Name,
                                PerUnit1         = s.PerUnit1,
                                PerUnit2         = s.PerUnit2,
                                Price            = s.Price,
                                PriceUnit1       = s.PriceUnit1,
                                PriceUnit2       = s.PriceUnit2,
                                UnitPrice        = s.UnitPrice,
                                Order            = s.Order,
                                IsActiveSchedule = s.IsActiveSchedule
                            };
                        }),
                        AdditionalRequirements = a.AdditionalRequirements,
                        Address1               = a.Address1,
                        Address2               = a.Address2,
                        CanAdultsJoin          = a.CanAdultsJoin,
                        City                   = a.City,
                        Subdivision            = a.Subdivision,
                        Region                 = a.Region,
                        Barangay               = a.Barangay,
                        CityName               = a.CityName,
                        BarangayName           = a.BarangayName,
                        RegionName             = a.RegionName,
                        PostalCode             = a.PostalCode,
                        CustomerBringWithThem  = a.CustomerBringWithThem,
                        Description            = a.Description,
                        District               = a.District,
                        ExperienceCategoryId   = a.ExperienceCategoryId,
                        ExperienceCategory     = a.ExperienceCategory,
                        SubCategory            = a.SubCategory,
                        ExperienceTypeId       = a.ExperienceTypeId,
                        ExperienceType         = a.ExperienceType,
                        CreatedBy              = a.CreatedBy,
                        Images = a.Images.Select(i => {
                            return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.ActivityImage
                            {
                                ImageSrc = i.ImageSrc,
                                Name     = i.Name,
                                Order    = i.Order
                            };
                        }),
                        IsPublished             = a.IsPublished,
                        MinimumAge              = a.MinimumAge,
                        Price                   = a.Price,
                        Remarks                 = a.Remarks,
                        ScheduleIndicator       = a.ScheduleIndicator,
                        SearchTags              = a.SearchTags,
                        SkillLevel              = a.SkillLevel,
                        SpecificsYouWillProvide = a.SpecificsYouWillProvide,
                        SubCategoryId           = a.SubCategoryId,
                        Title                   = a.Title,
                        Handler                 = a.Handler,
                        Owner = a.Owner != null ? new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.CustomerOwner
                        {
                            Handler    = a.Owner.Handler,
                            Id         = a.Owner.Id,
                            IsVerified = a.Owner.IsVerified,
                        } : null,
                        IsNew                  = a.IsNew,
                        OngoingStudents        = a.OngoingStudents,
                        CompletedStudents      = a.CompletedStudents,
                        NumberOfReviews        = a.NumberOfReviews,
                        AverageRating          = a.AverageRating,
                        ExperienceCreationType = a.ExperienceCreationType,
                        NumberOfTickets        = a.NumberOfTickets
                    };
                }).AsQueryable()
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetAllActivitiesResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetRefundableExperience")]
    [HttpGet]
    [ProducesResponseType(typeof(GetRefundableExperienceResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRefundableExperience()
    {
        try
        {
            var result = await getRefundableExperienceHandler.ExecuteAsync(new Services.ActivityService.Interactors.GetRefundableExperienceArgs {});
            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetRefundableExperienceResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }

            return new JsonResult(new GetRefundableExperienceResult 
            {
                IsSuccess = true,
                Result = result.Result.RefundableExperiences.Select(r => {
                    return new Framework.ApiCommand.ApiCore.DTO.Activity.RefundableExperienceDTO {
                        Name = r.Name,
                        PurchaseOrderId = r.PurchaseOrderId
                    };
                })
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetRefundableExperienceResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("Schedule/Update")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateScheduleResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateActivitySchedule([FromBody] UpdateScheduleArgs args)
    {
        try
        {
            var updateResult = await updateActivityScheduleHandler.ExecuteAsync(new Services.ActivityService.Interactors.UpdateScheduleArgs
            {
                Id               = args.Id,
                DateTime         = args.DateTime,
                IsActiveSchedule = args.IsActiveSchedule,
                IsSetSession     = args.IsSetSession,
                Name             = args.Name,
                Order            = args.Order,
                PerUnit1         = args.PerUnit1,
                PerUnit2         = args.PerUnit2,
                Price            = args.Price,
                PriceUnit1       = args.PriceUnit1,
                PriceUnit2       = args.PriceUnit2,
                SessionName      = args.SessionName,
                UnitPrice        = args.UnitPrice
            });

            if (!updateResult.Succeeded || updateResult.Result == null)
            {
                return new JsonResult(new UpdateScheduleResult { ErrorInfo = new ErrorInfo { Message = updateResult.Message } });
            }

            var updated = updateResult.Result;

            return new JsonResult(new UpdateScheduleResult
            {
                IsSuccess = true,
                Result = new Framework.ApiCommand.ApiData.DTO.Schedule.ScheduleDTO
                {
                    Id               = updated.Id,
                    DateTime         = updated.DateTime,
                    IsActiveSchedule = updated.IsActiveSchedule,
                    Name             = updated.Name,
                    Order            = updated.Order,
                    PerUnit1         = updated.PerUnit1,
                    PerUnit2         = updated.PerUnit2,
                    Price            = updated.Price,
                    PriceUnit1       = updated.PriceUnit1,
                    PriceUnit2       = updated.PriceUnit2,
                    UnitPrice        = updated.UnitPrice
                }
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateScheduleResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("Remove")]
    [HttpPost]
    [ProducesResponseType(typeof(DeleteActivityResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteActivityById([FromBody] DeleteActivityArgs args)
    {
        try
        {
            var deleteResult = await deleteActivityHandler.ExecuteAsync(new Services.ActivityService.Interactors.DeleteActivityArgs
            {
                ActivityId = args.ActivityId
            });

            if (!deleteResult.Succeeded || deleteResult.Result == null)
            {
                return new JsonResult(new DeleteActivityResult { ErrorInfo = new ErrorInfo { Message = deleteResult.Message } });
            }

            var result = deleteResult.Result;

            return new JsonResult(new DeleteActivityResult
            {
                IsSuccess = result.IsSuccess
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new DeleteActivityResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("OwnerPricingInclusive/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(OwnerPricingInclusiveResult), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> OwnerPricingInclusive(int id)
    {
        try
        {
            var result = await ownerPricingInclusiveHandler.ExecuteAsync(new Services.ActivityService.Interactors.OwnerPricingInclusiveArgs {
                CustomerId = id
            });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new OwnerPricingInclusiveResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new OwnerPricingInclusiveResult
            {
                IsSuccess = true,
                Result = new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityOwnerPricingInclusiveDTO {
                    IsInclusivePricing = result.Result.IsInclusivePricing
                }
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new OwnerPricingInclusiveResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CreateCoupon")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateCouponResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateCoupon([FromBody] CreateCouponArgs args)
    {
        try
        {
            var result = await providerCreateCouponHandler.ExecuteAsync(new Services.ActivityService.Interactors.ProviderCreateCouponArgs {
                ActivityId = args.ActivityId,
                Amount = args.Amount,
                Code = args.Code,
                DiscountType = args.DiscountType,
                FromDate = args.FromDate,
                MaximumSpend = args.MaximumSpend,
                Name = args.Name,
                ToDate = args.ToDate,
            });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreateCouponResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            var created = result.Result;

            return new JsonResult(new CreateCouponResult
            {
                IsSuccess = true,
                Result = new Framework.ApiCommand.ApiCore.DTO.Coupon.CouponDTO {
                    ActivityId = created.ActivityId,
                    Amount = created.Amount,
                    Code = created.Code,
                    CustomerId = created.CustomerId,
                    DiscountType = created.DiscountType,
                    FromDate = created.FromDate,
                    Id = created.Id,
                    IsAdmin = created.IsAdmin,
                    MaximumSpend = created.MaximumSpend,
                    Name = created.Name,
                    Status = created.Status,
                    ToDate = created.ToDate,
                    DateCreated = created.DateCreated
                }
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateCouponResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetCoupons")]
    [HttpGet]
    [ProducesResponseType(typeof(GetCouponsResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCoupons()
    {
        try
        {
            var result = await getCouponsHandler.ExecuteAsync(new Services.ActivityService.Interactors.GetCouponsArgs {});

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetCouponsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            var created = result.Result;

            return new JsonResult(new GetCouponsResult
            {
                IsSuccess = true,
                Result = result.Result.Coupons.Select(c => {
                    return new Framework.ApiCommand.ApiCore.DTO.Coupon.CouponDTO {
                        ActivityId = c.ActivityId,
                        Amount = c.Amount,
                        AppliedActivity = c.AppliedActivity != null ? new Framework.ApiCommand.ApiCore.DTO.Coupon.CouponDTO.Activity {
                            Id = c.AppliedActivity.Id,
                            Name = c.AppliedActivity.Name
                        } : null,
                        Code = c.Code,
                        CustomerId = c.CustomerId,
                        DiscountType = c.DiscountType,
                        FromDate = c.FromDate,
                        Id = c.Id,
                        IsAdmin = c.IsAdmin,
                        MaximumSpend = c.MaximumSpend,
                        Name = c.Name,
                        Status = c.Status,
                        ToDate = c.ToDate,
                        DateCreated = c.DateCreated
                    };
                })
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetCouponsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UpdateCouponStatus")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateCouponStatusResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateCouponStatus([FromBody] UpdateCouponStatusArgs args)
    {
        try
        {
            var result = await updateCouponStatusHandler.ExecuteAsync(new Services.ActivityService.Interactors.UpdateCouponStatusArgs {
                Id = args.Id,
                Status = args.Status
            });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateCouponStatusResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            var created = result.Result;

            return new JsonResult(new UpdateCouponStatusResult
            {
                IsSuccess = true,
                Result = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateCouponStatusResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("Favorite/Create")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateFavoriteResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateFavorite([FromBody] CreateFavoriteArgs args)
    {
        try
        {
            var result = await createFavoriteHandler.ExecuteAsync(new Services.ActivityService.Interactors.CreateFavoriteArgs
            {
                ActivityId = args.ActivityId,
                CustomerId = args.CustomerId
            });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreateFavoriteResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateFavoriteResult
            {
                IsSuccess = result.Succeeded,
                Result = new Framework.ApiCommand.ApiCore.DTO.Favorite.FavoriteDTO
                {
                    CustomerId = result.Result.CustomerId,
                    ActivityId = result.Result.ActivityId,
                }
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateFavoriteResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("Favorite/Remove")]
    [HttpPost]
    [ProducesResponseType(typeof(RemoveFavoriteResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveFavorite([FromBody] RemoveFavoriteArgs args)
    {
        try
        {
            var result = await removeFavoriteHandler.ExecuteAsync(new Services.ActivityService.Interactors.RemoveFavoriteArgs
            {
                ActivityId = args.ActivityId,
                CustomerId = args.CustomerId
            });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new RemoveFavoriteResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new RemoveFavoriteResult
            {
                IsSuccess = result.Succeeded,
                Result = new Framework.ApiCommand.ApiCore.DTO.Favorite.FavoriteDTO
                {
                    CustomerId = result.Result.CustomerId,
                    ActivityId = result.Result.ActivityId,
                }
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new RemoveFavoriteResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("Favorite/ByCustomer")]
    [HttpGet]
    [ProducesResponseType(typeof(GetFavoritesByCustomerResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFavoritesByCustomer([FromQuery] GetFavoritesByCustomerArgs args)
    {
        try
        {
            var result = await getFavoritesByCustomerHandler.ExecuteAsync(new Services.ActivityService.Interactors.GetFavoritesByCustomerArgs
            {
                CustomerId = args.CustomerId
            });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetFavoritesByCustomerResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetFavoritesByCustomerResult
            {
                IsSuccess = result.Succeeded,
                Result = result.Result.Favorites.Select(f => new Framework.ApiCommand.ApiCore.DTO.Favorite.FavoriteDTO
                {
                    ActivityId = f.ActivityId,
                    CustomerId = f.CustomerId,
                })
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetFavoritesByCustomerResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("ValidateCouponCode")]
    [HttpPost]
    [ProducesResponseType(typeof(ValidateCouponCodeResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> ValidateCouponCode([FromBody] ValidateCouponCodeArgs args)
    {
        try
        {
            var result = await validateCouponCodeHandler.ExecuteAsync(new Services.ActivityService.Interactors.ValidateCouponCodeArgs {
                ActivityId = args.ActivityId,
                Amount = args.Amount,
                CouponCode = args.CouponCode
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new ValidateCouponCodeResult { ErrorInfo = new ErrorInfo { Message = result.Message, Description = result.Error.Description } });
            }

            var validated = result.Result;

            return new JsonResult(new ValidateCouponCodeResult
            {
                IsSuccess = true,
                Result = new Framework.ApiCommand.ApiCore.DTO.Coupon.ValidatedCouponDTO {
                    Amount = validated.Amount,
                    DiscountType = validated.DiscountType,
                    IsValid = validated.IsValid,
                    MaximumSpend = validated.MaximumSpend
                }
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new ValidateCouponCodeResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UpdateCoupon")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateCouponResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateCoupon([FromBody] UpdateCouponArgs args)
    {
        try
        {
            var result = await updateCouponHandler.ExecuteAsync(new Services.ActivityService.Interactors.UpdateCouponArgs {
                From = args.DateFrom,
                Id = args.Id,
                Name = args.Name,
                To = args.DateTo
            });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateCouponResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            var updated = result.Result;

            return new JsonResult(new UpdateCouponResult
            {
                IsSuccess = true,
                Result = new Framework.ApiCommand.ApiCore.DTO.Coupon.CouponDTO {
                    ActivityId = updated.ActivityId,
                    Amount = updated.Amount,
                    Code = updated.Code,
                    CustomerId = updated.CustomerId,
                    DiscountType = updated.DiscountType,
                    FromDate = updated.FromDate,
                    Id = updated.Id,
                    IsAdmin = updated.IsAdmin,
                    MaximumSpend = updated.MaximumSpend,
                    Name = updated.Name,
                    Status = updated.Status,
                    ToDate = updated.ToDate,
                    DateCreated = updated.DateCreated
                }
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateCouponResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("RecommendedActivities")]
    [HttpGet]
    [ProducesResponseType(typeof(RecommendedActivitiesResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecommendedActivities()
    {
        try
        {
            var result = await recommendedActivitiesHandler.ExecuteAsync(new Services.ActivityService.Interactors.RecommendedActivityArgs { Count = 4});

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new RecommendedActivitiesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new RecommendedActivitiesResult
            {
                IsSuccess = result.Succeeded,
                Result = result.Result.RecommendedActivities.Select(a => {
                    return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO
                    {
                        ActivityId       = a.Id,
                        Handler          = a.Handler,
                        Description      = a.Description,
                        Title            = a.Title,
                        Price            = a.Price,
                        Address1         = a.Address1,
                        Address2         = a.Address2,
                        City             = a.City,
                        CityName         = a.CityName,
                        Subdivision      = a.Subdivision,
                        Region           = a.Region,
                        RegionName       = a.RegionName,
                        Barangay         = a.Barangay,
                        ExperienceTypeId = a.ExperienceTypeId,
                        Images           = a.Images.Select(i =>
                        {
                            return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.ActivityImage
                            {
                                Id       = i.Id,
                                ImageSrc = i.ImageSrc,
                                Name     = i.Name,
                                Order    = i.Order
                            };
                        }),
                        ActivitySchedules = a.ActivitySchedules.Select(i =>
                        {
                            return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.ActivitySchedule
                            {
                                Id               = i.Id,
                                DateTime         = i.DateTime,
                                Name             = i.Name,
                                PerUnit1         = i.PerUnit1,
                                PerUnit2         = i.PerUnit2,
                                Price            = i.Price,
                                PriceUnit1       = i.PriceUnit1,
                                PriceUnit2       = i.PriceUnit2,
                                UnitPrice        = i.UnitPrice,
                            };
                        })
                    };
                })
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new RecommendedActivitiesResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("ExperienceCreationTypes")]
    [HttpGet]
    [ProducesResponseType(typeof(GetExperienceCreationTypeResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetExperienceCreationTypes()
    {
        try
        {
            var result = await getExperienceCreationTypeHandler.ExecuteAsync(new Services.ActivityService.Interactors.GetExperienceCreationTypeArgs { });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetExperienceCreationTypeResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetExperienceCreationTypeResult
            {
                Result = result.Result.ExperienceCreationTypes.Select(e => {
                    return new Framework.ApiCommand.ApiCore.DTO.ExperienceCreationType.ExperienceCreationTypeDTO
                    {
                        Id = e.Id,
                        Description = e.Description,
                        ImagePath = e.ImagePath,
                        IsActive = e.IsActive,
                        Name = e.Name
                    };
                }),
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new RecommendedActivitiesResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("PopularActivities")]
    [HttpGet]
    [ProducesResponseType(typeof(PopularActivitiesResult), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> PopularActivities([FromQuery] PopularActivitiesArgs args)
    {
        try
        {
            var result = await popularActivitiesHandler.ExecuteAsync(new Services.ActivityService.Interactors.PopularActivitiesArgs
            {
                Skip = args.PageIndex,
                Take = args.CountPerPage,
                CategoryId = args.CategoryId
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new PopularActivitiesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            var mapResults = mapper.Map<IEnumerable<CoreDto.Activity.ActivityFeedDTO>>(result.Result.ActivityFeeds);

            return new JsonResult(new PopularActivitiesResult
            {
                IsSuccess = true,
                Result = mapResults,
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new PopularActivitiesResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetActivityScheduleTimes")]
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(GetActivityScheduleTimesResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActivityScheduleTimes([FromQuery] GetActivityScheduleTimesArgs args)
    {
        try
        {
            var result = await getActivityScheduleTimesHandler.ExecuteAsync(new Services.ActivityService.Interactors.GetActivityScheduleTimesArgs
            {
                ActivityScheduleId = args.ActivityScheduleId,
                DayOfWeek = args.DayOfWeek,
                ScheduleDate = args.ScheduleDate
            });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetActivityScheduleTimesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetActivityScheduleTimesResult
            {
                Result = result.Result.ActivityScheduleTimes.Select(e => {
                   return new Framework.ApiCommand.ApiCore.DTO.Schedule.ActivityScheduleTimeModel
                   {
                       ActivityScheduleId = e.ActivityScheduleId,
                       ActivityScheduleTimeId = e.ActivityScheduleTimeId,
                       DayOfWeek = e.DayOfWeek,
                       EndTime = e.EndTime,
                       StartTime = e.StartTime,
                       IsAvailable = e.IsAvailable
                   };
                }),
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetActivityScheduleTimesResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CreateOngoingActivitySchedule")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateOngoingActivityScheduleResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateOngoingActivitySchedule([FromBody] CreateOngoingActivityScheduleArgs args)
    {
        try
        {
            var result = await createOngoingActivityScheduleHandler.ExecuteAsync(new Services.ActivityService.Interactors.CreateOngoingActivityScheduleArgs
            {
                ActivityScheduleTimeId = args.ActivityScheduleTimeId,
                IsCompleted = args.IsCompleted,
                PurchaseOrderId = args.PurchaseOrderId,
                ScheduleDate = args.ScheduleDate,
                CreatedBy = args.CreatedBy
            });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreateOngoingActivityScheduleResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateOngoingActivityScheduleResult
            {
               IsSuccess = result.Succeeded,
               Result = result.Succeeded
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateOngoingActivityScheduleResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CreateOte")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateOteResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateOte([FromBody] CreateOteArgs args)
    {
        try
        {
            var activity = args.Activity;
            var result = await oteCreateHandler.ExecuteAsync(new Services.ActivityService.Interactors.OteCreateArgs {
                Activity = new Services.ActivityService.Interactors.OteCreateArgs.OteActivity {
                    BarangayCode = activity.BarangayCode ?? string.Empty,
                    BarangayName = activity.BarangayName ?? string.Empty,
                    CityName = activity.CityName ?? string.Empty,
                    CityNumber = activity.CityNumber ?? string.Empty,
                    Description = activity.Description,
                    EventName = activity.EventName,
                    ExperienceCreationTypeId = activity.ExperienceCreationTypeId,
                    CategoryId = activity.CategoryId,
                    ExperienceTypeId = activity.ExperienceTypeId,
                    HouseNo = activity.HouseNo ?? string.Empty,
                    IsPublished = activity.IsPublished,
                    PinnedLocation = activity.PinnedLocation ?? string.Empty,
                    PostalCode = activity.PostalCode ?? string.Empty,
                    Recurrence = activity.Recurrence,
                    RegionCode = activity.RegionCode ?? string.Empty,
                    RegionName = activity.RegionName ?? string.Empty,
                    ScheduleFrom = activity.ScheduleFrom,
                    ScheduleTo = activity.ScheduleTo,
                    IsComingSoon = activity.IsComingSoon,
                    
                    DurationEnd = activity.DurationEnd,
                    DurationEvery = activity.DurationEvery,
                    DurationStart = activity.DurationStart,
                    MonthDay = activity.MonthDay,
                    MonthRepeat = activity.MonthRepeat,
                    MonthSelection = activity.MonthSelection,
                    OnDayDate = activity.OnDayDate,
                    WeekString = activity.WeekString,

                    EventDurationCount = activity.EventDurationCount,
                    EventDurationTimeUnit = activity.EventDurationTimeUnit,
                    EventTicketLimit = activity.EventTicketLimit
                },
                Pricings = args.Pricings.Select(p => {
                    return new Services.ActivityService.Interactors.OteCreateArgs.OtePricing {
                        Description = p.Description,
                        IsAbsorbFees = p.IsAbsorbFees,
                        MaxSlots = p.MaxSlots,
                        Price = p.Price,
                        Name = p.Name
                    };
                }),
                DateOverrides = args.DateOverrides is not null ? 
                    args.DateOverrides.Select(d => new Services.ActivityService.Interactors.OteCreateArgs.DateOverride {
                        Date = d.Date,
                        TimeEnd = d.TimeEnd,
                        TimeStart = d.TimeStart
                    }) : null,
                OteOnlineEvents = args.OnlineEvents is not null ?
                    args.OnlineEvents.Select(s => new Services.ActivityService.Interactors.OteCreateArgs.OteOnlinEvent
                    {
                        Title = s.Title,
                        Description = s.Description,
                        VideoLink = s.VideoLink,    
                        TicketRestriction = s.TicketRestriction,
                    }) : null,
            });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreateOteResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateOteResult
            {
               IsSuccess = result.Succeeded,
               Result = new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO {
                ActivityId = result.Result.Id
               }
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateOteResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UpdateOte")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateOteResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateOte([FromBody] UpdateOteArgs args)
    {
        try
        {
            var activity = args.Activity;
            var result = await oteUpdateHandler.ExecuteAsync(new Services.ActivityService.Interactors.OteUpdateArgs {
                Activity = new Services.ActivityService.Interactors.OteUpdateArgs.OteActivity {
                    BarangayCode = activity.BarangayCode ?? string.Empty,
                    BarangayName = activity.BarangayName ?? string.Empty,
                    CategoryId = activity.CategoryId,
                    CityName = activity.CityName ?? string.Empty,
                    CityNumber = activity.CityNumber ?? string.Empty,
                    Description = activity.Description,
                    EventName = activity.EventName,
                    ExperienceTypeId = activity.ExperienceTypeId,
                    HouseNo = activity.HouseNo ?? string.Empty,
                    Id = activity.Id,
                    IsPublished = activity.IsPublished,
                    PinnedLocation = activity.PinnedLocation ?? string.Empty,
                    PostalCode = activity.PostalCode ?? string.Empty,
                    Recurrence = activity.Recurrence,
                    RegionCode = activity.RegionCode ?? string.Empty,
                    RegionName = activity.RegionName ?? string.Empty,
                    ScheduleFrom = activity.ScheduleFrom,
                    ScheduleTo = activity.ScheduleTo,
                    IsComingSoon = activity.IsComingSoon,

                    DurationEnd = activity.DurationEnd,
                    DurationEvery = activity.DurationEvery,
                    DurationStart = activity.DurationStart,
                    MonthDay = activity.MonthDay,
                    MonthRepeat = activity.MonthRepeat,
                    MonthSelection = activity.MonthSelection,
                    OnDayDate = activity.OnDayDate,
                    WeekString = activity.WeekString,

                    EventDurationCount = activity.EventDurationCount,
                    EventDurationTimeUnit = activity.EventDurationTimeUnit,
                    EventTicketLimit = activity.EventTicketLimit
                },
                Pricings = args.Pricings.Select(p => {
                    return new Services.ActivityService.Interactors.OteUpdateArgs.OtePricing {
                        Description = p.Description,
                        Id = p.Id,
                        IsAbsorbFees = p.IsAbsorbFees,
                        MaxSlots = p.MaxSlots,
                        Price = p.Price,
                        Name = p.Name
                    };
                }),
                DateOverrides = args.DateOverrides is not null ?
                    args.DateOverrides.Select(d => new Services.ActivityService.Interactors.OteUpdateArgs.DateOverride
                    {
                        Date = d.Date,
                        TimeEnd = d.TimeEnd,
                        TimeStart = d.TimeStart
                    }) : null,
                OnlineEvents = args.OnlineEvents is not null ? args.OnlineEvents.Select(s => {
                    return new Services.ActivityService.Interactors.OteUpdateArgs.OteOnlineEvent
                    {
                        Id = s.Id,
                        Title = s.Title,
                        Description = s.Description,
                        VideoLink = s.VideoLink,
                        TicketRestriction = s.TicketRestriction,
                    };
                }): null,
                OteReschedules = args.OteReschedules is not null ? args.OteReschedules.Select(s => new Services.ActivityService.Interactors.OteUpdateArgs.OteReschedule {
                    OldDate = s.OldDate,
                    NewDate = s.NewDate,
                    Id = s.Id
                }) : null
            });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateOteResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateOteResult
            {
               IsSuccess = result.Succeeded,
               Result = new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO {
                ActivityId = result.Result.Id
               }
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateOteResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("OteByHandler/{handler}")]
    [HttpGet]
    [ProducesResponseType(typeof(OteActivityResult), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> OteByHandler([FromQuery] OteActivityArgs args, string handler)
    {
        try
        {
            var result = await oteFindByHandler.ExecuteAsync(new Services.ActivityService.Interactors.OteFindByHandlerArgs {
                Handler            = handler,
                IncludeAddress     = args.IncludeAddress ?? false,
                IncludeDescription = args.IncludeDescription ?? false,
                IncludePricing     = args.IncludePricing ?? false,
                IncludeSchedule    = args.IncludeSchedule ?? false,
                IncludeImages      = args.IncludeImages ?? false,
                IncludeOnlineEvent = args.IncludeOnlineEvent ?? false
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new OteActivityResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            var mapResult = mapper.Map<ActivityResults.OteFindByHandlerResult, CoreDto.Activity.OteActivityDTO>(result.Result);
            return new JsonResult(new OteActivityResult
            {
                IsSuccess = true,
                Result = mapResult
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new OteActivityResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("TicketDetails/{guid}/{token}")]
    [HttpGet]
    [ProducesResponseType(typeof(OteTicketDetailsResult), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> TicketDetails(string guid, string token)
    {
        try
        {
            var result = await oteTicketDetailsHandler.ExecuteAsync(new Services.ActivityService.Interactors.OteTicketDetailsArgs {
                Guid = guid,
                Token = token
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new OteTicketDetailsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            var mapResult = mapper.Map<CoreDto.Activity.OteTicketDetailsDTO>(result.Result);
            return new JsonResult(new OteTicketDetailsResult
            {
                IsSuccess = true,
                Result = mapResult
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new OteTicketDetailsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CustomerOte")]
    [HttpGet]
    [ProducesResponseType(typeof(CustomerOteResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> CustomerOte()
    {
        try
        {
            var result = await customerOteHandler.ExecuteAsync(new ());
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CustomerOteResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            var mapResult = mapper.Map<IEnumerable<CoreDto.Activity.CustomerOteDTO>>(result.Result.CustomerOtes);
            return new JsonResult(new CustomerOteResult
            {
                IsSuccess = true,
                Result = mapResult
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CustomerOteResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("VerifyOTE")]
    [HttpPost]
    [ProducesResponseType(typeof(OteVerificationResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> VerifyOTE([FromBody] OteVerificationArgs args)
    {
        try
        {
            var result = await oteVerificationHandler.ExecuteAsync(new Services.ActivityService.Interactors.OteVerificationArgs {
                Handler = args.Handler,
                QrCode = args.QrCode,
                DateId = args.DateId
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new OteVerificationResult { ErrorInfo = new ErrorInfo { Message = result.Message, Code = result.Error.Code } });
            }

            return new JsonResult(new OteVerificationResult
            {
                IsSuccess = true,
                Result = new CoreDto.Activity.OteVerificationDTO {
                    TicketSeat = result.Result.TicketSeat,
                    Verified = result.Result.Verified,
                    Id = result.Result.Id
                }
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new OteVerificationResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("DeleteAddOns")]
    [HttpPost]
    [ProducesResponseType(typeof(DeleteAddOnsResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAddOnsById([FromBody] DeleteAddOnsArgs args)
    {
        try
        {
            var deleteResult = await deleteAddOnsHandler.ExecuteAsync(new Services.ActivityService.Interactors.DeleteAddOnsArgs
            {
               AddOnIds = args.AddOnsIds
            });

            if (!deleteResult.Succeeded || deleteResult.Result == null)
            {
                return new JsonResult(new DeleteAddOnsResult { ErrorInfo = new ErrorInfo { Message = deleteResult.Message } });
            }

            var result = deleteResult.Result;

            return new JsonResult(new DeleteAddOnsResult
            {
                IsSuccess = result.IsSuccess
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new DeleteAddOnsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
    [Route("DeleteAddOn")]
    [HttpPost]
    [ProducesResponseType(typeof(DeleteAddOnResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAddOnById([FromBody] DeleteAddOnArgs args)
    {
        try
        {
            var deleteResult = await deleteAddOnHandler.ExecuteAsync(new Services.ActivityService.Interactors.DeleteAddOnArgs
            {
                AddOnId = args.AddOnId
            });

            if (!deleteResult.Succeeded || deleteResult.Result == null)
            {
                return new JsonResult(new DeleteAddOnResult { ErrorInfo = new ErrorInfo { Message = deleteResult.Message } });
            }

            var result = deleteResult.Result;

            return new JsonResult(new DeleteAddOnResult
            {
                IsSuccess = result.IsSuccess
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new DeleteAddOnResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetOtePerDay")]
    [HttpGet]
    [ProducesResponseType(typeof(OtePerDayResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOtePerDay()
    {
        try
        {
            var result = await getOtePerDayHandler.ExecuteAsync(new ());
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new OtePerDayResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new OtePerDayResult
            {
                IsSuccess = true,
                Result = result.Result.OtePerDays.Select(e => {
                    return new CoreDto.Activity.OtePerDayDTO {
                        ActivityId = e.ActivityId,
                        CityName = e.CityName,
                        Date = e.Date,
                        DateEnd = e.DateEnd,
                        DateId = e.DateId,
                        DateStart = e.DateStart,
                        Description = e.Description,
                        EventImage = e.EventImage,
                        ExperienceTypeId = e.ExperienceTypeId,
                        Handler = e.Handler,
                        PinnedLocation = e.PinnedLocation,
                        RegionName = e.RegionName,
                        Title = e.Title,
                        ForceDisable = e.ForceDisable
                    };
                })
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new OtePerDayResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("DeleteOnlineEvent")]
    [HttpPost]
    [ProducesResponseType(typeof(DeleteOnlineEventResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteOnlineEventById([FromBody] DeleteOnlineEventArgs args)
    {
        try
        {
            var deleteResult = await deleteOnlineEventHandler.ExecuteAsync(new Services.ActivityService.Interactors.DeleteOnlineEventArgs
            {
                Id = args.Id
            });

            if (!deleteResult.Succeeded || deleteResult.Result == null)
            {
                return new JsonResult(new DeleteOnlineEventResult { ErrorInfo = new ErrorInfo { Message = deleteResult.Message } });
            }

            var result = deleteResult.Result;

            return new JsonResult(new DeleteOnlineEventResult
            {
                IsSuccess = result.IsSuccess
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new DeleteOnlineEventResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GenerateEventSharedLink")]
    [HttpPost]
    [ProducesResponseType(typeof(GenerateEventSharedLinkResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GenerateEventSharedLink([FromBody] GenerateEventSharedLinkArgs args)
    {
        try
        {
            var result = await generateEventSharedLinkHandler.ExecuteAsync(new Services.ActivityService.Interactors.GenerateEventSharedLinkArgs {
                DateId = args.DateId,
                Handler = args.Handler
            });

            if (!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new GenerateEventSharedLinkResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GenerateEventSharedLinkResult
            {
                IsSuccess = true,
                Result = new CoreDto.Activity.SharedLinkDTO {
                    Enable = result.Result.Enable,
                    GeneratedLink = result.Result.GeneratedLink,
                    Guid = result.Result.Guid,
                    Token = result.Result.Token
                }
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GenerateEventSharedLinkResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [AllowAnonymous]
    [Route("ValidateSharedLink")]
    [HttpGet]
    [ProducesResponseType(typeof(OteValidateSharedLinkResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> ValidateSharedLink([FromQuery] OteValidateSharedLinkArgs args)
    {
        try
        {
            var result = await oteValidateSharedLinkHandler.ExecuteAsync(new Services.ActivityService.Interactors.OteValidateSharedLinkArgs {
                Guid = args.Guid,
                Token = args.Token
            });

            if (!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new OteValidateSharedLinkResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new OteValidateSharedLinkResult
            {
                IsSuccess = true,
                Result = new CoreDto.Activity.OteValidateSharedLinkDTO {
                    Id = result.Result.Id,
                    Title = result.Result.EventTitle
                }
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new OteValidateSharedLinkResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [AllowAnonymous]
    [Route("VerifySharedEventLink")]
    [HttpPost]
    [ProducesResponseType(typeof(VerifySharedEventLinkResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> VerifySharedEventLink([FromBody] VerifySharedEventLinkArgs args)
    {
        try
        {
            var result = await oteSharedLinkVerificationHandler.ExecuteAsync(new Services.ActivityService.Interactors.OteSharedLinkVerificationArgs {
                Guid = args.Guid,
                QrCode = args.QrCode,
                Token = args.Token
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new VerifySharedEventLinkResult { ErrorInfo = new ErrorInfo { Message = result.Message, Code = result.Error.Code } });
            }

            return new JsonResult(new VerifySharedEventLinkResult
            {
                IsSuccess = true,
                Result = new CoreDto.Activity.OteVerificationDTO {
                    TicketSeat = result.Result.TicketSeat,
                    Verified = result.Result.Verified,
                    Id = result.Result.Id
                }
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new VerifySharedEventLinkResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UpdateSharedLinkStatus")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateSharedLinkStatusResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateSharedLinkStatus([FromBody] UpdateSharedLinkStatusArgs args)
    {
        try
        {
            var result = await oteUpdateSharedLinkStatusHandler.ExecuteAsync(new Services.ActivityService.Interactors.OteUpdateSharedLinkStatusArgs {
                Enable = args.Enable,
                Guid = args.Guid,
                Token = args.Token
            });

            if (!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new UpdateSharedLinkStatusResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateSharedLinkStatusResult
            {
                IsSuccess = true,
                Result = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateSharedLinkStatusResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }


    [Route("DeleteTicket")]
    [HttpPost]
    [ProducesResponseType(typeof(DeleteTicketResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteTicketById([FromBody] DeleteTicketArgs args)
    {
        try
        {
            var deleteResult = await deleteTicketHandler.ExecuteAsync(new Services.ActivityService.Interactors.DeleteTicketArgs
            {
                Id = args.Id
            });

            if (!deleteResult.Succeeded || deleteResult.Result == null)
            {
                return new JsonResult(new DeleteTicketResult { ErrorInfo = new ErrorInfo { Message = deleteResult.Message } });
            }

            var result = deleteResult.Result;

            return new JsonResult(new DeleteTicketResult
            {
                IsSuccess = result.IsSuccess
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new DeleteTicketResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }


    [AllowAnonymous]
    [Route("ActivityFeed")]
    [HttpGet]
    [ProducesResponseType(typeof(ActivityFeedResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> ActivityFeed([FromQuery] ActivityFeedArgs args)
    {
        try
        {
            var result = await activityFeedHandler.ExecuteAsync(new Services.ActivityService.Interactors.ActivityFeedArgs {
                CategoryId = args.CategoryId,
                Search = args.Search,
                Skip = args.Skip,
                Take = args.Take,
                ExperienceType = args.ExperienceType,
                ExperienceCategory = args.ExperienceCategory,
                StarReview = args.StarReview
            });

            if (!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new ActivityFeedResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            var mapResults = mapper.Map<IEnumerable<CoreDto.Activity.ActivityFeedDTO>>(result.Result.ActivityFeeds);

            return new JsonResult(new ActivityFeedResult
            {
                IsSuccess = true,
                Result = mapResults,
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new ActivityFeedResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("OteAlreadyBookedDates/{activityId}")]
    [HttpGet]
    [ProducesResponseType(typeof(OteAlreadyBookedDatesResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> OteAlreadyBookedDates(int activityId)
    {
        try
        {
            var result = await oteAlreadyBookedHandler.ExecuteAsync(new Services.ActivityService.Interactors.OteAlreadyBookedArgs {
                ActivityId = activityId
            });

            if (!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new OteAlreadyBookedDatesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            var mapResults = mapper.Map<IEnumerable<CoreDto.Activity.OteAlreadyBookedDTO>>(result.Result.OteAlreadyBookedItems);

            return new JsonResult(new OteAlreadyBookedDatesResult
            {
                IsSuccess = true,
                Result = mapResults,
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new OteAlreadyBookedDatesResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("OteBookedCount")]
    [HttpGet]
    [ProducesResponseType(typeof(OteBookedCountResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> OteBookedCount([FromQuery] OteBookedCountArgs args)
    {
        try
        {
            var result = await oteTicketBookedCountHandler.ExecuteAsync(new Services.ActivityService.Interactors.OteTicketBookedCountArgs {
                ActivityId = args.ActivityId
            });
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new OteBookedCountResult { ErrorInfo = new ErrorInfo { Message = result.Message } });    
            }

            var mappedResult = mapper.Map<OteTicketBookCountDTO>(result.Result);

            return new JsonResult(new OteBookedCountResult
            {
                IsSuccess = true,
                Result = mappedResult,
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new OteBookedCountResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("OteScheduleDates")]
    [HttpGet]
    [ProducesResponseType(typeof(OteScheduleDatesResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> OteScheduleDates([FromQuery] OteScheduleDatesArgs args)
    {
        try
        {
            DateTime? dateFrom = null;
            if(!string.IsNullOrEmpty(args.From))
            {
                dateFrom = DateTime.ParseExact(args.From, "yyyyMMdd", CultureInfo.InvariantCulture);
            }

            DateTime? dateTo = null;
            if(!string.IsNullOrEmpty(args.To))
            {
                dateTo = DateTime.ParseExact(args.To, "yyyyMMdd", CultureInfo.InvariantCulture);
            }
            
            var result = await oteScheduleDatesHandler.ExecuteAsync(new Services.ActivityService.Interactors.OteScheduleDatesArgs {
                ActivityId = args.ActivityId,
                DateFrom = dateFrom,
                DateTo = dateTo
            });
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new OteScheduleDatesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });    
            }

            var mappedResult = mapper.Map<IEnumerable<OteScheduleDateDTO>>(result.Result.OteDateSchedules);

            return new JsonResult(new OteScheduleDatesResult
            {
                IsSuccess = true,
                Result = mappedResult,
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new OteScheduleDatesResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}