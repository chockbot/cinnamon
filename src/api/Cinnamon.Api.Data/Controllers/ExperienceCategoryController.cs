using Microsoft.AspNetCore.Mvc;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.ExperienceCategory.DTO;

namespace Cinnamon.Api.Data.Controllers;
public class ExperienceCategoryController : ControllerBase
{
   private readonly IExperienceCategoryRepository experienceCategoryRepository;
	public ExperienceCategoryController(IExperienceCategoryRepository experienceCategoryRepository)
	{
		this.experienceCategoryRepository = experienceCategoryRepository;	
	}
	[Route("GetAllExprienceCategory")]
	[HttpGet]
	[ProducesResponseType(typeof(IEnumerable<ExperienceCategoryDTO>), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetAllExperienceCategory()
	{
		var result = await experienceCategoryRepository.GetAllAsync();
        if (!result.Succeeded)
        {
            return NotFound();
        }

        return new JsonResult(result.Result);
    }
}

