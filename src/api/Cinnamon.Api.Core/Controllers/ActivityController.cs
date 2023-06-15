using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService;
using Cinnamon.Framework.ApiCommand.ApiCore;
using Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Activity.Response;
using Cinnamon.Framework.ApiCommand.ApiCore.Favorite.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Favorite.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    private readonly ICreateFavoriteHandler createFavoriteHandler;
    private readonly IRemoveFavoriteHandler removeFavoriteHandler;
    private readonly IGetFavoritesByCustomerHandler getFavoritesByCustomerHandler;
    private readonly ILogger _logger;

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
        IDeleteActivityHandler deleteActivityHandler, IOwnerPricingInclusiveHandler ownerPricingInclusiveHandler, ICreateFavoriteHandler createFavoriteHandler, IRemoveFavoriteHandler removeFavoriteHandler, IGetFavoritesByCustomerHandler getFavoritesByCustomerHandler)
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
        this.createFavoriteHandler = createFavoriteHandler;
        this.removeFavoriteHandler = removeFavoriteHandler;
        this.getFavoritesByCustomerHandler = getFavoritesByCustomerHandler;
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
                ActivitySchedules = args.ActivitySchedules.Select(s => {
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
                        IsActiveSchedule = s.IsActiveSchedule
                    };
                }),
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
                IsSetSession = args.IsSetSession,
                SessionName = args.SessionName ?? string.Empty,
                PinnedLocation = args.PinnedLocation ?? string.Empty,
                Status = args.Status
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
                IsSetSession = activity.IsSetSession,
                SessionName = activity.SessionName
            }});
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
                ActivityId = args.ActivityId,
                ActivityLevel = args.ActivityLevel,
                AdditionalRequirements = args.AdditionalRequirements,
                Address1 = args.Address1,
                Address2 = args.Address2,
                CanAdultsJoin = args.CanAdultsJoin,
                City = args.City,
                Subdivision = args.Subdivision,
                Region = args.Region,
                Barangay = args.Barangay,
                PostalCode = args.PostalCode,
                CustomerBringWithThem = args.CustomerBringWithThem,
                Description = args.Description,
                District = args.District,
                ExperienceCategoryId = args.ExperienceCategoryId,
                ExperienceTypeId = args.ExperienceTypeId,
                IsPublished = args.IsPublished,
                MinimumAge = args.MinimumAge,
                Price = args.Price,
                Remarks = args.Remarks,
                ScheduleIndicator = args.ScheduleIndicator,
                SearchTags = args.SearchTags,
                SkillLevel = args.SkillLevel,
                SpecificsYouWillProvide = args.SpecificsYouWillProvide,
                SubCategoryId = args.SubCategoryId,
                Title = args.Title,
                IsSetSession = args.IsSetSession,
                SessionName = args.SessionName,
                PinnedLocation = args.PinnedLocation,
                IsDeactivated = args.IsDeactivated,
                IsAdmin = args.IsAdmin,
                Status = args.Status,
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
                            IsActiveSchedule = s.IsActiveSchedule
                        };
                    }) : null,
                DeletedScheduleIds  = args.DeletedScheduleIds != null ? args.DeletedScheduleIds : Enumerable.Empty<int>()
            });
            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateActivityResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }
            var activity = result.Result;

            return new JsonResult(new UpdateActivityResult {IsSuccess = true, Result = new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO {
                ActivityId = activity.ActivityId,
                ActivityLevel = activity.ActivityLevel,
                AdditionalRequirements = activity.AdditionalRequirements,
                Address1 = activity.Address1,
                Address2 = activity.Address2,
                CanAdultsJoin = activity.CanAdultsJoin,
                City = activity.City,
                Subdivision = activity.Subdivision,
                Region = activity.Region,
                Barangay = activity.Barangay,
                PostalCode =activity.PostalCode,
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
                IsSetSession = activity.IsSetSession,
                SessionName = activity.SessionName,
                Status = activity.Status
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
                        IsSetSession = a.IsSetSession,
                        SessionName = a.SessionName,
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
                                IsActiveSchedule = s.IsActiveSchedule
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
                        Handler = a.Handler,
                        IsSetSession = a.IsSetSession,
                        SessionName = a.SessionName,
                        IsNew = a.IsNew,
                        OngoingStudents = a.OngoingStudents,
                        CompletedStudents = a.CompletedStudents,
                        Status = a.Status,
                        Owner = a.Owner != null ? new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.CustomerOwner {
                            Handler = a.Owner.Handler,
                            Id  = a.Owner.Id,
                            IsVerified = a.Owner.IsVerified,
                        } : null
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
                        IsSetSession = a.IsSetSession,
                        SessionName = a.SessionName,
                        IsNew = a.IsNew,
                        OngoingStudents = a.OngoingStudents,
                        CompletedStudents = a.CompletedStudents,
                        AverageRating = a.AverageRating,
                        NumberOfReviews = a.NumberOfReviews,
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
                        IsSetSession = a.IsSetSession,
                        SessionName = a.SessionName,
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
                IncludeCustomer = args.IncludeCustomer
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
                    IsSetSession = activity.IsSetSession,
                    SessionName = activity.SessionName,
                    PinnedLocation= activity.PinnedLocation,
                    Status = activity.Status,
                    Owner = activity.Owner != null ? new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.CustomerOwner {
                            Handler = activity.Owner.Handler,
                            Id  = activity.Owner.Id
                        } : null
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
                IncludeCustomer = args.IncludeCustomer
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
                    IsSetSession = activity.IsSetSession,
                    SessionName = activity.SessionName,
                    Owner = activity.Owner != null ? new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.CustomerOwner {
                            Handler = activity.Owner.Handler,
                            Id  = activity.Owner.Id
                        } : null
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
                IncludeCustomer = args.IncludeCustomer
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
                    IsSetSession = activity.IsSetSession,
                    SessionName = activity.SessionName,
                    PinnedLocation = activity.PinnedLocation,
                    Owner = activity.Owner != null ? new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.CustomerOwner {
                            Handler = activity.Owner.Handler,
                            Id  = activity.Owner.Id,
                            ImageSrc = activity.Owner.ImageSrc,
                            FirstName = activity.Owner.FirstName,
                            LastName = activity.Owner.LastName,
                            IsVerified = activity.Owner.IsVerified,
                            IsOG = activity.Owner.IsOG,
                            IsOfficial = activity.Owner.IsOfficial,
                            Email = activity.Owner.Email,
                            PhoneNumber = activity.Owner.PhoneNumber
                        } : null
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
                IncludeCustomer = args.IncludeCustomer
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
                    IsSetSession = activity.IsSetSession,
                    SessionName = activity.SessionName,
                    PinnedLocation = activity.PinnedLocation,
                    Owner = activity.Owner != null ? new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.CustomerOwner {
                            Handler = activity.Owner.Handler,
                            Id  = activity.Owner.Id
                        } : null
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
                Image1 = args.Image1,
                Image2 = args.Image2,
                Image3 = args.Image3
            });

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UploadActivityImageResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }

            return new JsonResult(new UploadActivityImageResult {IsSuccess = true, Result = new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityImagesDTO {
                Image1 = result.Result.Image1Path,
                Image2 = result.Result.Image2Path,
                Image3 = result.Result.Image3Path
            }});
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
                        IsSetSession = a.IsSetSession,
                        SessionName = a.SessionName,
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
                        IsSetSession = a.IsSetSession,
                        SessionName = a.SessionName,
                        Owner = a.Owner != null ? new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.CustomerOwner
                        {
                            Handler = a.Owner.Handler,
                            Id = a.Owner.Id,
                            IsVerified = a.Owner.IsVerified,
                        } : null,
                        IsNew = a.IsNew,
                        OngoingStudents = a.OngoingStudents,
                        CompletedStudents = a.CompletedStudents,
                        NumberOfReviews = a.NumberOfReviews,
                        AverageRating = a.AverageRating
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
}