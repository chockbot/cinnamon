using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Cinnamon.Framework.ApiCommand.ApiData.SearchTags.Response;
using Cinnamon.Framework.ApiCommand.ApiData.SearchTags.Request;

namespace Cinnamon.Api.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchTagsController : ControllerBase
    {
        private readonly ISearchTagsRepository searchTagsRepository;

        public SearchTagsController(ISearchTagsRepository searchTagsRepository)
        {
            this.searchTagsRepository = searchTagsRepository;   
        }

        [Route("GetSearchTagsById/{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(GetSearchTagsResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSearchTagsById(int id)
        {
            try
            {
                var result = await searchTagsRepository.GetByIdAsync(id);
                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new GetSearchTagsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }
                return new JsonResult(new GetSearchTagsResult { Result = result.Result, IsSuccess = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetSearchTagsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("GetAllSearchTags")]
        [HttpGet]
        [ProducesResponseType(typeof(GetAllSearchTagsResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllSearchTags([FromQuery] GetAllSearchTagsArgs args)
        {
            try
            {
                var result =
                   args.PageIndex.HasValue && args.CountPerPage.HasValue ?
                   await searchTagsRepository.GetAllAsync(args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage) :
                   await searchTagsRepository.GetAllAsync();
                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new GetAllSearchTagsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }
                // get all without pagination to get all rows
                var all = args.PageIndex.HasValue && args.CountPerPage.HasValue ?
                await searchTagsRepository.GetAllAsync(null, null) :
                await searchTagsRepository.GetAllAsync();

                if (!all.Succeeded || all.Result == null)
                {
                    return new JsonResult(new GetAllSearchTagsResult { ErrorInfo = new ErrorInfo { Message = all.Message } });
                }

                var totalRecords = all.Result.Count();
                return new JsonResult(new GetAllSearchTagsResult
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
                return new JsonResult(new GetAllSearchTagsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("CreateSearchTags")]
        [HttpPost]
        [ProducesResponseType(typeof(CreateSearchTagsResult), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSearchTags([FromBody] CreateSearchTagsArgs args)
        {
            try
            {
                var result = await searchTagsRepository.CreateSearchTagsAsync(args.ActivityId, args.SearchTag1, args.SearchTag2, args.SearchTag3, args.SearchTag4, args.SearchTag5);
                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new CreateSearchTagsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                return new JsonResult(new CreateSearchTagsResult { IsSuccess = true, Result = result.Result });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CreateSearchTagsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("UpdateSearchTags")]
        [HttpPost]
        [ProducesResponseType(typeof(UpdateSearchTagsResult), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateSearchTags([FromBody] UpdateSearchTagsArgs args)
        {
            try
            {
                var result = await searchTagsRepository.UpdateSearchTagsAsync(args.searchTagId, args.activityId, args.searchTag1, args.searchTag2, args.searchTag3, args.searchTag4, args.searchTag5);

                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new UpdateSearchTagsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                return new JsonResult(new UpdateSearchTagsResult { IsSuccess = true, Result = result.Result });
            }
            catch (Exception ex)
            {
                return new JsonResult(new UpdateSearchTagsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }
    }
}
