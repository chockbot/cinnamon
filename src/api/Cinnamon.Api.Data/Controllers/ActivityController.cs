using Microsoft.AspNetCore.Mvc;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Api.Data.Models.Activity.Request;
using Cinnamon.Api.Data.Models.Activity.Response;

namespace Cinnamon.Api.Data.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ActivityController : ControllerBase
{
    private readonly IActivityRepository activityRepository;

    public ActivityController(IActivityRepository activityRepository)
    {
        this.activityRepository = activityRepository;
    }

    [Route("GetActivityById/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetActivityResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetActivityById(int id)
    {
        try
        {
            if(id <= 0)
            {
                return NotFound();
            }

            var result = await activityRepository.GetByIdAsync(id);
            if(!result.Succeeded)
            {
                return new JsonResult(new GetActivityResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
            }

            if(result.Result == null)
            {
                return NotFound();
            }

            return new JsonResult( new GetActivityResult { Result = result.Result, IsSuccess = true});
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetActivityResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetAllActivities")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllActivitiesResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllActivities([FromQuery] GetAllActivitiesArgs args)
    {
        try
        {
            var result =
                args.PageIndex.HasValue && args.CountPerPage.HasValue ?
                await activityRepository.GetAllAsync(args.IsActive, args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage) :
                await activityRepository.GetAllAsync();

            if (!result.Succeeded || result.Result == null)
            {
                return NotFound();
            }

            // get all without pagination to get all rows
            var all = args.PageIndex.HasValue && args.CountPerPage.HasValue ?
                await activityRepository.GetAllAsync(args.IsActive, null, null) :
                await activityRepository.GetAllAsync();

            if(!all.Succeeded || all.Result == null)
            {
                return NotFound();
            }

            var totalRecords = all.Result.Count();
            return new JsonResult(new GetAllActivitiesResult 
                { 
                    Result = result.Result,
                    IsSuccess = true,
                    Pagination = new Models.Pagination 
                        { 
                            PageIndex = args.PageIndex,
                            PerPage = args.CountPerPage,
                            TotalRecords = totalRecords,
                            TotalPages = args.CountPerPage.HasValue && args.PageIndex.HasValue ? 
                                (int)Math.Ceiling(Convert.ToDouble(totalRecords / args.CountPerPage.Value)) : null 
                        }
                });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetAllActivitiesResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message} });
        }
    }

    [Route("CreateActivity")]
    [HttpPost]
    [ProducesResponseType(typeof(CreatedActivityResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateActivity([FromBody] CreateActivityArgs args)
    {
        try
        {
            var result = await activityRepository.CreateActivityAsync(args.ExperienceTypeId, args.CustomerId, args.Title,
                args.Description, args.Price, args.ScheduleIndicator, args.Remarks, args.IsPublished, args.Address1,
                args.Address2, args.District, args.City, args.SpecificsYouWillProvide, args.CustomerBringWithThem, args.AdditionalRequirements,
                args.ActivityLevel, args.SkillLevel, args.MinimumAge, args.CanAdultsJoin, args.Searchtag1, args.Searhtag2,
                args.Searhtag3, args.Searchtag4, args.Searchtag5);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreatedActivityResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreatedActivityResult { IsSuccess = true, Result = result.Result });
        }
        catch(Exception ex)
        {
            return new JsonResult(new CreatedActivityResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message} });
        }
    }

    [Route("UpdateActivity")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdatedActivityResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateActivity([FromBody] UpdateActivity args)
    {
        try
        {
            var result = await activityRepository.UpdateActivityAsync(args.ActivityId, args.ExperienceTypeId, args.Title, args.Description,
                args.Price, args.ScheduleIndicator, args.Remarks, args.IsPublished, args.Address1, args.Address2, args.District,
                args.City, args.SpecificsYouWillProvide, args.CustomerBringWithThem,args.AdditionalRequirements, args.ActivityLevel, args.SkillLevel,
                args.MinimumAge, args.CanAdultsJoin, args.Searchtag1, args.Searhtag2, args.Searhtag3, args.Searchtag4, args.Searchtag5);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdatedActivityResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreatedActivityResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdatedActivityResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
        }
    }
}