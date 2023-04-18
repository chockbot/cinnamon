using Microsoft.AspNetCore.Mvc;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.ActivityImage.Response;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.ActivityImage.Request;

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
                return new JsonResult(new GetActivityImageResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult( new GetActivityImageResult { Result = result.Result, IsSuccess = true});
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetActivityImageResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
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
                return new JsonResult(new GetAllActivityImagesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            // get all without pagination to get all rows
            var all = args.PageIndex.HasValue && args.CountPerPage.HasValue ?
                await activityImageRepository.GetAllAsync(null, null) :
                await activityImageRepository.GetAllAsync();

            if(!all.Succeeded || all.Result == null)
            {
                return new JsonResult(new GetAllActivityImagesResult { ErrorInfo = new ErrorInfo { Message = all.Message } });
            }

            var totalRecords = all.Result.Count();
            return new JsonResult(new GetAllActivityImagesResult 
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
            return new JsonResult(new GetAllActivityImagesResult { ErrorInfo = new ErrorInfo { Message = ex.Message} });
        }
    }

    [Route("CreateActivityImage")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateActivityImageResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateActivityImage([FromBody] CreateActivityImageArgs args)
    {
        try
        {
            var result = await activityImageRepository.Create(args.ActivityId, args.ImageName, args.ImagePath, args.Order);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreateActivityImageResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateActivityImageResult { IsSuccess = true, Result = result.Result });
        }
        catch(Exception ex)
        {
            return new JsonResult(new CreateActivityImageResult { ErrorInfo = new ErrorInfo { Message = ex.Message} });
        }
    }

    [Route("UpdateActivityImage")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateActivityImageResult), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> UpdateActivityImage([FromBody] UpdateActivityImageArgs args)
    {
        try
        {
            var result = await activityImageRepository.Update(args.ActivityImageId, args.ImageName, args.ImagePath, args.Order);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateActivityImageResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateActivityImageResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateActivityImageResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UpdateManyActivityImage")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateManyActivityImageResult), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> UpdateManyActivityImage([FromBody] UpdateManyActivityImageArgs args)
    {
        try
        {
            var result = await activityImageRepository.Update(args.Images.Select(s => {
                return new Framework.ApiCommand.ApiData.DTO.ActivityImage.ActivityImageDTO {
                    ActivityId = s.ActivityId,
                    Id = s.Id,
                    ImageLocation = s.ImageSrc,
                    ImageName = s.ImageName,
                    Order = s.Order
                };
            }));

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateManyActivityImageResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateManyActivityImageResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateManyActivityImageResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CreateManyActivityImage")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateManyActivityImageResult), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> CreateManyActivityImage([FromBody] CreateManyActivityImageArgs args)
    {
        try
        {
            var result = await activityImageRepository.Create(args.Images.Select(s => {
                return new Framework.ApiCommand.ApiData.DTO.ActivityImage.ActivityImageDTO {
                    ActivityId = s.ActivityId,
                    ImageLocation = s.ImageSrc,
                    ImageName = s.ImageName,
                    Order = s.Order
                };
            }));

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreateManyActivityImageResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateManyActivityImageResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateManyActivityImageResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}