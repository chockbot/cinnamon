using Cinnamon.Api.Data.Models.Activity.Request;
using Cinnamon.Api.Data.Models.ExperienceCategory.Request;
using Cinnamon.Api.Data.Models.ExperienceCategory.Response;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Data.Controllers;
public class ExperienceCategoryController : ControllerBase
{
	private readonly IExperienceCategoryRepository experienceCategoryRepository;
	public ExperienceCategoryController(IExperienceCategoryRepository experienceCategoryRepository)
	{
		this.experienceCategoryRepository = experienceCategoryRepository;
	}
    [Route("GetCategoryById/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetCategoryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategoryById(int id)
    {
		try
		{
            var result = await experienceCategoryRepository.GetByIdAsync(id);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetCategoryResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
            }
            return new JsonResult(new GetCategoryResult { Result = result.Result, IsSuccess = true });
        }
		catch (Exception ex)
		{
            return new JsonResult(new GetCategoryResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
        }
    }
    [Route("GetAllCategory")]
	[HttpGet]
	[ProducesResponseType(typeof(GetAllCategoryResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetAllCategory([FromQuery] GetAllCategoryArgs args)
	{
		try
		{
            var result =
               args.PageIndex.HasValue && args.CountPerPage.HasValue ?
               await experienceCategoryRepository.GetAllAsync(args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage) :
               await experienceCategoryRepository.GetAllAsync();
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllCategoryResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
            }
            // get all without pagination to get all rows
            var all = args.PageIndex.HasValue && args.CountPerPage.HasValue ?
            await experienceCategoryRepository.GetAllAsync(null, null) :
            await experienceCategoryRepository.GetAllAsync();

            if (!all.Succeeded || all.Result == null)
            {
                return new JsonResult(new GetAllCategoryResult { ErrorInfo = new Models.ErrorInfo { Message = all.Message } });
            }

            var totalRecords = all.Result.Count();
            return new JsonResult(new GetAllCategoryResult
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
            return new JsonResult(new GetAllCategoryResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
        }
	}
    [Route("CreateCategory")]
    [HttpPost]
    [ProducesResponseType(typeof(CreatedCategoryResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryArgs args)
    {
        try
        {
            var result = await experienceCategoryRepository.CreateExperienceCategoryAsync(args.Category, args.IconPath);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreatedCategoryResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreatedCategoryResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreatedCategoryResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
        }
    }
    [Route("UpdateCategory")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdatedCategoryResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategory args)
    {
        try
        {
            var result = await experienceCategoryRepository.UpdateExperienceCategoryAsync(args.Id,args.Category, args.IconPath);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdatedCategoryResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdatedCategoryResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdatedCategoryResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
        }
    }
}

