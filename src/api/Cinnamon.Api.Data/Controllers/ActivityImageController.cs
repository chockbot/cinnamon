using Microsoft.AspNetCore.Mvc;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Api.Data.Models.ActivityImage.Request;
using Cinnamon.Api.Data.Models.ActivityImage.Response;

namespace Cinnamon.Api.Data.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ActivityImageController : ControllerBase 
{
    private readonly IActivityImageRepository activityImageRepository;

    public ActivityImageController(IActivityImageRepository activityImageRepository)
    {
        this.activityImageRepository = activityImageRepository;
    }

    [Route("GetActivityImageById/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetActivityImageResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActivityImageById(int id)
    {
        try
        {
            var result = await activityImageRepository.GetByIdAsync(id);
            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetActivityImageResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
            }

            return new JsonResult( new GetActivityImageResult { Result = result.Result, IsSuccess = true});
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetActivityImageResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetAllActivityImages")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllActivityImagesResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllActivityImages([FromQuery] GetAllActivityImagesArgs args)
    {
        try
        {
            var result =
                args.PageIndex.HasValue && args.CountPerPage.HasValue ?
                    await activityImageRepository.GetAllAsync(args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage) :
                    await activityImageRepository.GetAllAsync();

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllActivityImagesResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
            }

            // get all without pagination to get all rows
            var all = args.PageIndex.HasValue && args.CountPerPage.HasValue ?
                await activityImageRepository.GetAllAsync(null, null) :
                await activityImageRepository.GetAllAsync();

            if(!all.Succeeded || all.Result == null)
            {
                return new JsonResult(new GetAllActivityImagesResult { ErrorInfo = new Models.ErrorInfo { Message = all.Message } });
            }

            var totalRecords = all.Result.Count();
            return new JsonResult(new GetAllActivityImagesResult 
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
            return new JsonResult(new GetAllActivityImagesResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message} });
        }
    }

    [Route("CreateActivityImage")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateActivityImageResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateActivityImage([FromBody] CreateActivityImageArgs args)
    {
        try
        {
            var result = await activityImageRepository.Create(args.ActivityId, args.ImageName, args.ImagePath);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreateActivityImageResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateActivityImageResult { IsSuccess = true, Result = result.Result });
        }
        catch(Exception ex)
        {
            return new JsonResult(new CreateActivityImageResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message} });
        }
    }

    [Route("UpdateActivityImage")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateActivityImageResult), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> UpdateActivityImage([FromBody] UpdateActivityImageArgs args)
    {
        try
        {
            var result = await activityImageRepository.Update(args.ActivityImageId, args.ImageName, args.ImagePath);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateActivityImageResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateActivityImageResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateActivityImageResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
        }
    }
}