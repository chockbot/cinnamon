using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Framework.ApiCommand.ApiCore;
using Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Activity.Response;
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

    public ActivityController(ICreateActivityHandler createActivityHandler, IGetExperienceTypesHandler getExperienceTypesHandler,
        IGetExperienceCategoriesHandler getExperienceCategoriesHandler, IGetSubCategoriesHandler getSubCategoriesHandler,
        IGetOwnedActivitiesHandler getOwnedActivitiesHandler, IUpdateActivityHandler updateActivityHandler,
        IGetOwnedActivityHandler getOwnedActivityHandler, IUploadActivityImageHandler uploadActivityImageHandler, 
        IGetAddressHandler getAddressHandler, IGetActivityHandler getActivityHandler, IGetAllActivitiesHandler getAllActivitiesHandler, 
        IGetActivityImagesHandler getActivityImagesHandler, IGetActiviesByCategoriesHandler getActiviesByCategoriesHandler)
    {
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
                        UnitPrice = s.UnitPrice
                    };
                }),
                AdditionalRequirements = args.AdditionalRequirements,
                Address1 = args.Address1 ?? string.Empty,
                Address2 = args.Address2 ?? string.Empty,
                CanAdultsJoin = args.CanAdultsJoin,
                City = args.City ?? string.Empty,
                CustomerBringWithThem = args.CustomerBringWithThem,
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
                SpecificsYouWillProvide = args.SpecificsYouWillProvide,
                SubCategoryId = args.SubCategoryId,
                Title = args.Title
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
                        UnitPrice = s.UnitPrice
                    };
                }),
                AdditionalRequirements = activity.AdditionalRequirements,
                Address1 = activity.Address1,
                Address2 = activity.Address2,
                CanAdultsJoin = activity.CanAdultsJoin,
                City = activity.City,
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
                Title = activity.Title
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
                            UnitPrice = s.UnitPrice
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
                Title = activity.Title
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
                IsActive = args.IsActive
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
                                DateTime = s.DateTime,
                                Name = s.Name,
                                PerUnit1 = s.PerUnit1,
                                PerUnit2 = s.PerUnit2,
                                Price = s.Price,
                                PriceUnit1 = s.PriceUnit1,
                                PriceUnit2 = s.PriceUnit2,
                                UnitPrice = s.UnitPrice
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
                                Name = i.Name
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
                        Title = a.Title
                    };
                })
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetOwnedActivitiesResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }
    
    [Route("GetAllActivities")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllActivitiesResult), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllActivities()
    {
        try
        {
            var result = await getAllActivitiesHandler.ExecuteAsync(new Services.ActivityService.Interactors.GetAllActivitiesArgs { });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllActivitiesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetAllActivitiesResult
            {
                IsSuccess = true,
                Result = result.Result.Activities.Select(s => {
                    return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO
                    {
                        ActivityId = s.ActivityId,
                        ExperienceTypeId = s.ExperienceTypeId,
                        ExperienceCategoryId = s.ExperienceCategoryId,
                        SubCategoryId = s.SubCategoryId,
                        Title = s.Title,
                        Description = s.Description,
                        Price = s.Price,
                        ScheduleIndicator = s.ScheduleIndicator,
                        Remarks = s.Remarks,
                        IsPublished = s.IsPublished,
                        Address1 = s.Address1,
                        Address2 = s.Address2,
                        District = s.District,
                        City = s.City,
                        SpecificsYouWillProvide = s.SpecificsYouWillProvide,
                        CustomerBringWithThem = s.CustomerBringWithThem,
                        AdditionalRequirements = s.AdditionalRequirements,
                        ActivityLevel = s.ActivityLevel,
                        SkillLevel = s.SkillLevel,
                        MinimumAge = s.MinimumAge,
                        CanAdultsJoin = s.CanAdultsJoin
                    };
                })
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
                IsActive = args.IsActive
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
                            UnitPrice = s.UnitPrice
                        };
                    }),
                    AdditionalRequirements = activity.AdditionalRequirements,
                    Address1 = activity.Address1,
                    Address2 = activity.Address2,
                    CanAdultsJoin = activity.CanAdultsJoin,
                    City = activity.City,
                    CustomerBringWithThem = activity.CustomerBringWithThem,
                    Description = activity.Description,
                    District = activity.District,
                    ExperienceCategoryId = activity.ExperienceCategoryId,
                    ExperienceTypeId = activity.ExperienceTypeId,
                    Images = activity.Images.Select(i => {
                        return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.ActivityImage {
                            Id = i.Id,
                            ImageSrc = i.ImageSrc,
                            Name = i.Name
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
                    Title = activity.Title
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
                IsActive = args.IsActive
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
                            UnitPrice = s.UnitPrice
                        };
                    }),
                    AdditionalRequirements = activity.AdditionalRequirements,
                    Address1 = activity.Address1,
                    Address2 = activity.Address2,
                    CanAdultsJoin = activity.CanAdultsJoin,
                    City = activity.City,
                    CustomerBringWithThem = activity.CustomerBringWithThem,
                    Description = activity.Description,
                    District = activity.District,
                    ExperienceCategoryId = activity.ExperienceCategoryId,
                    ExperienceTypeId = activity.ExperienceTypeId,
                    Images = activity.Images.Select(i => {
                        return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO.ActivityImage {
                            Id = i.Id,
                            ImageSrc = i.ImageSrc,
                            Name = i.Name
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
                    Title = activity.Title
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

    [Route("GetActivitiesByCategories/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetActivitiesByCategoriesResult), StatusCodes.Status200OK)]
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
                IsActive = args.IsActive
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetActivitiesByCategoriesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetActivitiesByCategoriesResult
            {
                IsSuccess = true,
                Result = result.Result.Activities.Select(s => {
                    return new Framework.ApiCommand.ApiCore.DTO.Activity.ActivityDTO
                    {
                        ActivityId = s.Id,
                        ExperienceTypeId = s.ExperienceTypeId,
                        ExperienceCategoryId = s.ExperienceCategoryId,
                        SubCategoryId = s.SubCategoryId,
                        Title = s.Title,
                        Description = s.Description,
                        Price = s.Price,
                        ScheduleIndicator = s.ScheduleIndicator,
                        Remarks = s.Remarks,
                        IsPublished = s.IsPublished,
                        Address1 = s.Address1,
                        Address2 = s.Address2,
                        District = s.District,
                        City = s.City,
                        SpecificsYouWillProvide = s.SpecificsYouWillProvide,
                        CustomerBringWithThem = s.CustomerBringWithThem,
                        AdditionalRequirements = s.AdditionalRequirements,
                        ActivityLevel = s.ActivityLevel,
                        SkillLevel = s.SkillLevel,
                        MinimumAge = s.MinimumAge,
                        CanAdultsJoin = s.CanAdultsJoin
                    };
                })
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetAllActivitiesResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}