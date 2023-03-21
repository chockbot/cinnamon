using Microsoft.AspNetCore.Mvc;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.Activity.Response;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.Activity.Request;
using Microsoft.Extensions.Logging;

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
                args.IncludeSearchTags, args.IncludeSchedules, args.IncludeImages, args.IsActive, args.IncludeCustomer);
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
                args.IncludeSearchTags, args.IncludeSchedules, args.IncludeImages, args.IsActive, args.IncludeCustomer);
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

            var isUsedFilters = (args.PageIndex.HasValue && args.CountPerPage.HasValue) || args.IsActive.HasValue ||
                args.IncludeAddress.HasValue || args.IncludeDescription.HasValue || args.IncludeImages.HasValue ||
                args.IncludeSchedules.HasValue || args.IncludeSearchTags.HasValue || ids.Count > 0 ||
                !string.IsNullOrEmpty(args.LikeHandler) || args.IncludeCustomer.HasValue || args.IncludeExperienceTypes.HasValue || args.IncludeExperienceCategories.HasValue || args.IncludeSubCategories.HasValue || args.IncludeStudents.HasValue;
            
            var includeAddress = args.IncludeAddress ?? false;
                
            var result =
                isUsedFilters ?
                    await activityRepository
                        .GetAllAsync(args.CustomerId, args.IsActive, args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage,args.ExperienceCategoryId.GetValueOrDefault(), args.SearchValue,
                            args.IncludeAddress ?? false, args.IncludeDescription ?? false, args.IncludeSearchTags ?? false,
                            args.IncludeSchedules ?? false, args.IncludeImages ?? false, ids.Count > 0 ? ids : null, args.LikeHandler ?? null,
                            args.IncludeCustomer ?? false, args.IncludeExperienceTypes ?? false, args.IncludeExperienceCategories ?? false, args.IncludeSubCategories ?? false, args.IncludeStudents ?? false) :
                    await activityRepository.GetAllAsync();

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllActivitiesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            // get all without pagination to get all rows
            var all = isUsedFilters ?
                        await activityRepository.GetAllAsync(args.CustomerId,args.IsActive, null, null, args.ExperienceCategoryId.GetValueOrDefault(), args.SearchValue) :
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
                args.Address2, args.District, args.City,args.Subdivision,args.Region,args.Barangay,args.PostalCode, args.SpecificsYouWillProvide, args.CustomerBringWithThem, args.AdditionalRequirements,
                args.ActivityLevel, args.SkillLevel, args.MinimumAge, args.CanAdultsJoin, args.Searchtag1, args.Searhtag2,
                args.Searhtag3, args.Searchtag4, args.Searchtag5, args.ExperienceCategoryId, args.SubCategoryId, args.Handler, args.IsSetSession, args.SessionName, args.PinnedLocation);

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
                args.City, args.Subdivision, args.Region, args.Barangay, args.PostalCode, args.SpecificsYouWillProvide, args.CustomerBringWithThem,args.AdditionalRequirements, args.ActivityLevel, args.SkillLevel,
                args.MinimumAge, args.CanAdultsJoin, args.Searchtag1, args.Searhtag2, args.Searhtag3, args.Searchtag4, args.Searchtag5,
                args.ExperienceCategoryId, args.SubCategoryId,args.IsSetSession, args.SessionName, args.PinnedLocation);

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
                !string.IsNullOrEmpty(args.LikeHandler) || args.IncludeCustomer.HasValue || args.IncludeExperienceTypes.HasValue || args.IncludeExperienceCategories.HasValue || args.IncludeSubCategories.HasValue || args.IncludeStudents.HasValue;

            var includeAddress = args.IncludeAddress ?? false;

            var result =
                isUsedFilters ?
                    await activityRepository.GetPopularActivitiesAsync(args.CustomerId, args.IsActive, args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage,
                                        args.IncludeAddress ?? false, args.IncludeDescription ?? false, args.IncludeSearchTags ?? false, args.IncludeSchedules ?? false,
                                        args.IncludeImages ?? false, ids.Count > 0 ? ids : null, args.IncludeCustomer ?? false, args.IncludeExperienceTypes ?? false, 
                                        args.IncludeExperienceCategories ?? false, args.IncludeSubCategories ?? false, args.IncludeStudents ?? false) :
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
}