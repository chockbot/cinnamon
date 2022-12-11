using Microsoft.AspNetCore.Mvc;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.ExperienceCategory.DTO;
using Cinnamon.Api.Data.Models.ExperienceCategory.Response;
using Cinnamon.Api.Data.Models.Activity.Response;
using Cinnamon.Api.Data.Services.Repository.Activity;
using Cinnamon.Api.Data.Models.Activity.Request;
using Cinnamon.Api.Data.Models.ExperienceCategory.Request;

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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCategoryById(int id)
    {
		try
		{
            if (id <= 0)
            {
                return NotFound();
            }

            var result = await experienceCategoryRepository.GetByIdAsync(id);
            if (!result.Succeeded)
            {
                return new JsonResult(new GetCategoryResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
            }

            if (result.Result == null)
            {
                return NotFound();
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
	public async Task<IActionResult> GetAllCategory()
	{
		try
		{
            var result = await experienceCategoryRepository.GetAllAsync();
            if (!result.Succeeded)
            {
                return NotFound();
            }

            return new JsonResult(result.Result);
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

