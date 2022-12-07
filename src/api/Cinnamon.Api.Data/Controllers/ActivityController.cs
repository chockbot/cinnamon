using Microsoft.AspNetCore.Mvc;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Activity.DTO;

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

    [Route("GetAllActivities")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ActivityDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllActivities()
    {
        var result = await activityRepository.GetAllAsync(null,null,null);
        if (!result.Succeeded)
        {
            return NotFound();
        }

        return new JsonResult(result.Result);
    }
}