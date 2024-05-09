using Microsoft.AspNetCore.Mvc;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.Activity.Response;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.Activity.Request;
using Microsoft.Extensions.Logging;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteSchedule;
using Cinnamon.Framework.ApiCommand.ApiData.OteTicket.Response;
using Cinnamon.Framework.ApiCommand.ApiData.OteTicket.Request;
using Cinnamon.Api.Data.Services.Repository.OnlineEvent;

namespace Cinnamon.Api.Data.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ActivityController : ControllerBase
{
    private readonly IActivityRepository activityRepository;
    private readonly ILogger _logger;
    public ActivityController(IActivityRepository activityRepository, ILogger<ActivityController> logger)
    {
        this.activityRepository = activityRepository;
        _logger = logger;
    }

    [Route("GetActivityById/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetActivityResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActivityById(int id, [FromQuery] GetActivityArgs args)
    {
        try
        {
            var result = await activityRepository.GetByIdAsync(id, args.CustomerId, args.IncludeAddress, args.IncludeDescription,
                args.IncludeSearchTags, args.IncludeSchedules, args.IncludeImages, args.IsActive, args.IncludeCustomer, args.IncludeStudents ?? false, args.IncludeTickets ?? false, args.IncludeAddOns ?? false);
            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetActivityResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult( new GetActivityResult { Result = result.Result, IsSuccess = true});
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetActivityResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetActivityByHandler/{handler}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetActivityResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActivityById(string handler, [FromQuery] GetActivityArgs args)
    {
        try
        {
            var result = await activityRepository.GetByHandlerAsync(handler, args.CustomerId, args.IncludeAddress, args.IncludeDescription,
                args.IncludeSearchTags, args.IncludeSchedules, args.IncludeImages, args.IsActive, args.IncludeCustomer, args.IncludeStudents ?? false, args.IncludeAddOns ?? false);
            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetActivityResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult( new GetActivityResult { Result = result.Result, IsSuccess = true});
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetActivityResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetAllActivities")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllActivitiesResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllActivities([FromQuery] GetAllActivities args)
    {
        try
        {
            IList<int> ids = new List<int>();
            if(!string.IsNullOrEmpty(args.Ids))
            {
                var stringIDs = args.Ids.Split(",");
                foreach(var id in stringIDs)
                {
                    if(int.TryParse(id, out int parsedId))
                    {
                        ids.Add(parsedId);
                    }
                }
            }

            int? skip = 0;
            int? take = args.CountPerPage;

            var isUsedFilters = (args.PageIndex.HasValue && args.CountPerPage.HasValue) || args.IsActive.HasValue ||
                args.IncludeAddress.HasValue || args.IncludeDescription.HasValue || args.IncludeImages.HasValue ||
                args.IncludeSchedules.HasValue || args.IncludeSearchTags.HasValue || ids.Count > 0 ||
                !string.IsNullOrEmpty(args.LikeHandler) || args.IncludeCustomer.HasValue || args.IncludeExperienceTypes.HasValue ||
                args.IncludeExperienceCategories.HasValue || args.IncludeSubCategories.HasValue || args.IncludeStudents.HasValue || 
                args.IsDeactivated.HasValue || args.IncludeTickets.HasValue || args.ForceDisable.HasValue;
            
            var includeAddress = args.IncludeAddress ?? false;

            if (args.IsAdmin.GetValueOrDefault())
            {
                skip = (args.PageIndex - 1) * args.CountPerPage;
            }
            else
            {
                if (args.PageIndex > 1)
                {
                    skip = ((args.PageIndex - 2) * args.CountPerPage) + 20;
                }
            }

            var result =
                isUsedFilters ?
                    await activityRepository
                        .GetAllAsync(args.CustomerId, args.IsActive, take, skip, args.ExperienceCategoryId.GetValueOrDefault(), args.SearchValue, args.IsDeactivated, args.Status,
                            args.IncludeAddress ?? false, args.IncludeDescription ?? false, args.IncludeSearchTags ?? false,
                            args.IncludeSchedules ?? false, args.IncludeImages ?? false, ids.Count > 0 ? ids : null, args.LikeHandler ?? null,
                            args.IncludeCustomer ?? false, args.IncludeExperienceTypes ?? false, args.IncludeExperienceCategories ?? false, 
                            args.IncludeSubCategories ?? false, args.IncludeStudents ?? false, args.IncludeReviews ?? false, args.IncludeTickets ?? false, args.ForceDisable) :
                    await activityRepository.GetAllAsync();

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllActivitiesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            // get all without pagination to get all rows
            var all = isUsedFilters ?
                        await activityRepository.GetAllAsync(args.CustomerId,args.IsActive, null, null, args.ExperienceCategoryId.GetValueOrDefault(), args.SearchValue, args.IsDeactivated, args.Status) :
                        await activityRepository.GetAllAsync();

            if(!all.Succeeded || all.Result == null)
            {
                return new JsonResult(new GetAllActivitiesResult { ErrorInfo = new ErrorInfo { Message = all.Message } });
            }

            var totalRecords = all.Result.Count();
            return new JsonResult(new GetAllActivitiesResult 
                { 
                    Result = result.Result,
                    IsSuccess = true,
                    Pagination = new Pagination 
                        { 
                            PageIndex = args.PageIndex,
                            PerPage = args.CountPerPage,
                            TotalRecords = totalRecords,
                            TotalPages = args.CountPerPage.HasValue && args.PageIndex.HasValue ? 
                                (int)Math.Ceiling((double)totalRecords / args.CountPerPage.Value) : null
                        }
                });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetAllActivitiesResult { ErrorInfo = new ErrorInfo { Message = ex.Message} });
        }
    }

    [Route("CreateActivity")]
    [HttpPost]
    [ProducesResponseType(typeof(CreatedActivityResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateActivity([FromBody] CreateActivityArgs args)
    {
        try
        {
            var result = await activityRepository.CreateActivityAsync(args.ExperienceTypeId, args.CustomerId, args.Title,
                args.Description, args.Price, args.ScheduleIndicator, args.Remarks, args.IsPublished, args.Address1,
                args.Address2, args.District, args.City, args.Subdivision, args.Region, args.Barangay, args.PostalCode, args.SpecificsYouWillProvide, args.CustomerBringWithThem, args.AdditionalRequirements,
                args.ActivityLevel, args.SkillLevel, args.MinimumAge, args.CanAdultsJoin, args.Searchtag1, args.Searhtag2,
                args.Searhtag3, args.Searchtag4, args.Searchtag5, args.ExperienceCategoryId, args.SubCategoryId, args.Handler,args.PinnedLocation, args.Status, 
                args.ExperienceCreationType, args.ClassPolicies, args.VideoLink);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreatedActivityResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreatedActivityResult { IsSuccess = true, Result = result.Result });
        }
        catch(Exception ex)
        {
            return new JsonResult(new CreatedActivityResult { ErrorInfo = new ErrorInfo { Message = ex.Message} });
        }
    }

    [Route("UpdateActivity")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdatedActivityResult), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> UpdateActivity([FromBody] UpdateActivity args)
    {
        try
        {
            var result = await activityRepository.UpdateActivityAsync(args.ActivityId, args.ExperienceTypeId, args.Title, args.Description,
                args.Price, args.ScheduleIndicator, args.Remarks, args.IsPublished, args.Address1, args.Address2, args.District,
                args.City, args.Subdivision, args.Region, args.Barangay, args.PostalCode, args.SpecificsYouWillProvide, args.CustomerBringWithThem, args.AdditionalRequirements, args.ActivityLevel, args.SkillLevel,
                args.MinimumAge, args.CanAdultsJoin, args.Searchtag1, args.Searhtag2, args.Searhtag3, args.Searchtag4, args.Searchtag5,
                args.ExperienceCategoryId, args.SubCategoryId, args.PinnedLocation, args.IsDeactivated, args.Status, args.Handler,
                args.ClassPolicies, args.VideoLink);


            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdatedActivityResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdatedActivityResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdatedActivityResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetActivitiesByCategories/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetActivitiesByCategoriesResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActivitiesByCategories(int id, [FromQuery] GetActivityArgs args)
    {
        try
        {
            var result = await activityRepository.GetActivitieByCategoriesAsync(id, args.CustomerId, args.IncludeAddress, args.IncludeDescription,
                args.IncludeSearchTags, args.IncludeSchedules, args.IncludeImages, args.IsActive, args.IncludeCustomer);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetActivityResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetActivityResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetActivityResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("Popular")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllActivitiesResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPopularActivities([FromQuery] GetAllActivities args)
    {
        _logger.LogInformation("Cinnamon.Api.Data > GetPopularActivities was called");

        try
        {
            IList<int> ids = new List<int>();
            if (!string.IsNullOrEmpty(args.Ids))
            {
                var stringIDs = args.Ids.Split(",");
                foreach (var id in stringIDs)
                {
                    if (int.TryParse(id, out int parsedId))
                    {
                        ids.Add(parsedId);
                    }
                }
            }

            var isUsedFilters = (args.PageIndex.HasValue && args.CountPerPage.HasValue) || args.IsActive.HasValue ||
                args.IncludeAddress.HasValue || args.IncludeDescription.HasValue || args.IncludeImages.HasValue ||
                args.IncludeSchedules.HasValue || args.IncludeSearchTags.HasValue || ids.Count > 0 ||
                !string.IsNullOrEmpty(args.LikeHandler) || args.IncludeCustomer.HasValue || args.IncludeExperienceTypes.HasValue || args.IncludeExperienceCategories.HasValue || args.IncludeSubCategories.HasValue || 
                args.IncludeStudents.HasValue || args.IsDeactivated.HasValue || args.IncludeTickets.HasValue;

            var includeAddress = args.IncludeAddress ?? false;

            var result =
                isUsedFilters ?
                    await activityRepository.GetPopularActivitiesAsync(args.CustomerId, args.IsActive, args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage, args.IsDeactivated,
                                        args.IncludeAddress ?? false, args.IncludeDescription ?? false, args.IncludeSearchTags ?? false, args.IncludeSchedules ?? false,
                                        args.IncludeImages ?? false, ids.Count > 0 ? ids : null, args.IncludeCustomer ?? false, args.IncludeExperienceTypes ?? false, 
                                        args.IncludeExperienceCategories ?? false, args.IncludeSubCategories ?? false, args.IncludeStudents ?? false, args.IncludeReviews ?? false, args.IncludeTickets ?? false) :
                    await activityRepository.GetAllAsync();
            
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllActivitiesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            var totalRecords = result.Result.Count();
            return new JsonResult(new GetAllActivitiesResult
            {
                Result = result.Result,
                IsSuccess = true,
                Pagination = new Pagination
                {
                    PageIndex = args.PageIndex,
                    PerPage = args.CountPerPage,
                    TotalRecords = totalRecords,
                    TotalPages = args.CountPerPage.HasValue && args.PageIndex.HasValue ?
                                (int)Math.Ceiling((double)totalRecords / args.CountPerPage.Value) : null
                }
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetAllActivitiesResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("Guid/Update")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdatedActivityResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateActivityGuid([FromBody] UpdateActivity args)
    {
        try
        {
            var result = await activityRepository.UpdateActivityGuid();
            if (!result.Succeeded || !result.Result)
            {
                return new JsonResult(new UpdatedActivityResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdatedActivityResult { IsSuccess = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdatedActivityResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("Remove")]
    [HttpPost]
    [ProducesResponseType(typeof(DeleteActivityResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteActivityById([FromBody] DeleteActivityArgs args)
    {
        try
        {
            var result = await activityRepository.RemoveActivityAsync(args.ActivityId);
            if (!result.Succeeded || !result.Result)
            {
                return new JsonResult(new DeleteActivityResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new DeleteActivityResult { IsSuccess = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new DeleteActivityResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("RecommendedActivities/{primaryId}/{count}")]
    [HttpGet]
    [ProducesResponseType(typeof(RecommendedActivitiesResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecommendedActivities(int primaryId, int count)
    {
        try
        {
            var result = await activityRepository.GetRecommendedActivities(primaryId, count);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new RecommendedActivitiesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new RecommendedActivitiesResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new RecommendedActivitiesResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("PopularActivities")]
    [HttpGet]
    [ProducesResponseType(typeof(PopularActivitiesResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> PopularActivities([FromQuery] PopularActivitiesArgs args)
    {
        try
        {
            var result = await activityRepository.PopularActivities(args.CountPerPage, args.PageIndex, args.CategoryId);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new PopularActivitiesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(
                new PopularActivitiesResult 
                {
                    Result = result.Result, IsSuccess = true, 
                    Pagination = new Pagination
                    {
                        PageIndex = args.PageIndex,
                        PerPage = args.CountPerPage,
                        TotalRecords = result.Result.Count(),
                        TotalPages = args.CountPerPage.HasValue && args.PageIndex.HasValue ?
                                    (int)Math.Ceiling((double)result.Result.Count() / args.CountPerPage.Value) : null
                    } 
                });
        }
        catch (Exception ex)
        {
            return new JsonResult(new PopularActivitiesResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [HttpPost]
    [Route("CreateOteActivity")]
    [ProducesResponseType(typeof(CreateOteActivityResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateOteActivity([FromBody] CreateOteActivityArgs args)
    {
        try
        {
            var activity = args.Activity;
            var pricings = args.Pricings.Select(p => {
                return new OteSchedulePricingDTO {
                    Description = p.Description,
                    IsAbsorbFees = p.IsAbsorbFees,
                    MaxSlots = p.MaxSlots,
                    Price = p.Price,
                    Name = p.Name
                };
            }).ToList();

            var onlineEvent = args.OnlineEvents is not null? args.OnlineEvents.Select(e => {
                return new OteOnlineEventsDTO {
                    Title                     = e.Title,
                    Description               = e.Description,
                    Videolink                 = e.VideoLink,
                    TicketRestriction         = e.TicketRestriction,
                };
            }).ToList() : null;

            var dates = args.Dates.Select(d => {
                return new OteScheduleDateDTO {
                    Date = d.Date,
                    DateEnd = d.DateEnd,
                    DateStart = d.DateStart,
                };
            }).ToList();

            var dateOverrides = args.DateOverrides is not null ? args.DateOverrides.Select(d => {
                return new OteDateOverrideDTO {
                    Date = d.Date,
                    DateEnd = d.DateEnd,
                    DateStart = d.DateStart
                };
            }).ToList() : null;

            var result = await activityRepository.CreateOteActivity(activity.EventName, activity.Description, activity.ExperienceTypeId, 
                activity.CustomerId, activity.StringPrice, activity.HouseNo, activity.CityNumber, activity.CityName,
                activity.RegionCode, activity.RegionName, activity.BarangayCode, activity.BarangayName, activity.PostalCode,
                activity.PinnedLocation, activity.ScheduleFrom, activity.ScheduleTo, activity.Recurrence, pricings, activity.IsPublished,
                activity.Handler, activity.ExperienceCreationTypeId, args.Activity.IsComingSoon, args.Activity.ExtraOptions, 
                args.Activity.RecurrenceDateEnd, args.Activity.RecurrenceDateStart, args.Activity.RepeatEvery,
                args.Activity.SelectedDays, dates, args.Activity.EventDurationCount, args.Activity.EventDurationTimeUnit,args.Activity.EventTicketLimit,
                dateOverrides, onlineEvent, args.Activity.CategoryId);
            
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new CreateOteActivityResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(
                new CreateOteActivityResult 
                {
                    Result = result.Result, 
                    IsSuccess = true,
                });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateOteActivityResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [HttpPost]
    [Route("UpdateOteActivity")]
    [ProducesResponseType(typeof(UpdateOteActivityResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> UpdateOteActivity([FromBody] UpdateOteActivityArgs args)
    {
        try
        {
            var activity = args.Activity;
            var pricings = args.Pricings.Select(p => {
                return new OteSchedulePricingDTO {
                    Id = p.Id,
                    Description = p.Description,
                    IsAbsorbFees = p.IsAbsorbFees,
                    MaxSlots = p.MaxSlots,
                    Price = p.Price,
                    Name = p.Name
                };
            }).ToList();

            var onlineEvents = args.OnlineEvents is not null ? args.OnlineEvents.Select(u => {
                return new OteOnlineEventsDTO {
                    Id                        = u.Id,
                    Description               = u.Description,
                    Title                     = u.Title,
                    TicketRestriction         = u.TicketRestriction,
                    Videolink                 = u.VideoLink, 
                };
            }).ToList() : null;

            var dates = args.Dates.Select(d => {
                return new OteScheduleDateDTO
                {
                    Date = d.Date,
                    DateEnd = d.DateEnd,
                    DateStart = d.DateStart,
                };
            }).ToList();

            var dateOverrides = args.DateOverrides is not null ? args.DateOverrides.Select(d => {
                return new OteDateOverrideDTO
                {
                    Date = d.Date,
                    DateEnd = d.DateEnd,
                    DateStart = d.DateStart
                };
            }).ToList() : null;

            var result = await activityRepository.UpdateOteActivity(args.Activity.Id, args.Activity.EventName, args.Activity.Description,
                args.Activity.ExperienceTypeId, args.Activity.StringPrice, args.Activity.HouseNo ?? string.Empty, args.Activity.CityNumber ?? string.Empty,
                args.Activity.CityName ?? string.Empty, args.Activity.RegionCode ?? string.Empty, args.Activity.RegionName ?? string.Empty,
                args.Activity.BarangayCode ?? string.Empty, args.Activity.BarangayName ?? string.Empty,
                args.Activity.PostalCode ?? string.Empty, args.Activity.PinnedLocation ?? string.Empty, args.Activity.ScheduleFrom, args.Activity.ScheduleTo, 
                args.Activity.Recurrence, pricings, args.Activity.IsPublished, args.Activity.Handler, args.Activity.CategoryId, args.Activity.IsComingSoon,
                args.Activity.EventTicketLimit, args.Activity.ExtraOptions, args.Activity.RecurrenceDateEnd, args.Activity.RecurrenceDateStart, args.Activity.RepeatEvery,
                args.Activity.SelectedDays, dates, args.Activity.EventDurationCount, args.Activity.EventDurationTimeUnit,
                dateOverrides, onlineEvents);
            
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new UpdateOteActivityResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(
                new UpdateOteActivityResult 
                {
                    Result = result.Result, 
                    IsSuccess = true,
                });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateOteActivityResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("OteActivity/{handler}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetOteActivityByHandlerResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOteActivityByHandler(string handler, [FromQuery] GetOteActivityArgs args)
    {
        try
        {
            var result = await activityRepository.FindOteByHandler(handler, args.IncludeDescription ?? false, args.IncludeAddress ?? false,
                args.IncludeSchedule ?? false, args.IncludePricing ?? false, args.IncludeProvider ?? false, args.IncludeImages ?? false, args.IncludeOnlineEvents ?? false, args.IncludeTickets ?? false);
            
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new GetOteActivityByHandlerResult { ErrorInfo = new ErrorInfo { Message = result.Message } }); 
            }

            return new JsonResult(new GetOteActivityByHandlerResult { IsSuccess = true, Result = result.Result }); 
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetOteActivityByHandlerResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
    [Route("ote/add-ticket-solds")]
    [HttpPost]
    [ProducesResponseType(typeof(AddTicketSoldResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddTicketSolds([FromBody] AddTicketSoldArgs args)
    {
        try
        {
            var ticketDtos = args.TicketSolds.Select(t => {
                return new OteSchedulePricingDTO {
                    Id = t.Id,
                    TicketSold = t.TicketSold
                };
            });

            var result = await activityRepository.AddTicketSold(ticketDtos);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new AddTicketSoldResult { ErrorInfo = new ErrorInfo { Message = result.Message } }); 
            }

            return new JsonResult(new AddTicketSoldResult { IsSuccess = true, Result = result.Result }); 
        }
        catch (Exception ex)
        {
            return new JsonResult(new AddTicketSoldResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetOTEByProvider")]
    [HttpGet]
    [ProducesResponseType(typeof(GetOTEByProvideResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOTEByProvider([FromQuery] GetOTEByProvideArgs args)
    {
        try
        {
            var result = await activityRepository.GetOTEByProvider(args.Id);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetOTEByProvideResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetOTEByProvideResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetOTEByProvideResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("customer-ote/{customerId}")]
    [HttpGet]
    [ProducesResponseType(typeof(CustomerOteResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> CustomerOte(int customerId)
    {
        try
        {
            var result = await activityRepository.CustomerOte(customerId);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CustomerOteResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CustomerOteResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CustomerOteResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("OtePerDate")]
    [HttpGet]
    [ProducesResponseType(typeof(OtePerDateResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> OtePerDate([FromQuery] OtePerDateArgs args)
    {
        try
        {
            var result = await activityRepository.OtePerDate(args.ProviderId);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new OtePerDateResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new OtePerDateResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new OtePerDateResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("ExpiredEvents")]
    [HttpGet]
    [ProducesResponseType(typeof(ExpiredEventsResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> ExpiredEvents()
    {
        try
        {
            var result = await activityRepository.ExpiredEvents();
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new ExpiredEventsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new ExpiredEventsResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new ExpiredEventsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("ForceDisableActivities")]
    [HttpPost]
    [ProducesResponseType(typeof(ForceDisableActivitiesResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> ForceDisableActivities([FromBody] ForceDisableActivitiesArgs args)
    {
        try
        {
            var result = await activityRepository.ForceDisableActivities(args.Ids);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new ForceDisableActivitiesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new ForceDisableActivitiesResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new ForceDisableActivitiesResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("ActivityFeed")]
    [HttpGet]
    [ProducesResponseType(typeof(ActivityFeedResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> ActivityFeed([FromQuery] ActivityFeedArgs args)
    {
        try
        {
            var result = await activityRepository.ActivityFeed(args.Take, args.Skip, args.Search, 
                args.CategoryId, args.StarReview, args.ExperienceType);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new ActivityFeedResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new ActivityFeedResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new ActivityFeedResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("BatchSummaryUpdate")]
    [HttpPost]
    [ProducesResponseType(typeof(BatchSummaryUpdateResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> BatchSummaryUpdate()
    {
        try
        {
            var result = await activityRepository.BatchSummaryUpdate();
            if (!result.Succeeded || !result.Result)
            {
                return new JsonResult(new BatchSummaryUpdateResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new BatchSummaryUpdateResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new BatchSummaryUpdateResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
    [Route("DeleteTicket")]
    [HttpPost]
    [ProducesResponseType(typeof(DeleteTicketResult), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> DeleteAddOn([FromBody] DeleteTicketArgs args)
    {
        try
        {
            var result = await activityRepository.DeleteTicket(args.Id);
            if (!result.Succeeded || !result.Result)
            {
                return new JsonResult(new DeleteTicketResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new DeleteTicketResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new DeleteTicketResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}