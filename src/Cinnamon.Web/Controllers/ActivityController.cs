using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Cinnamon.Web.Models.Forms;
using Microsoft.AspNetCore.Authorization;
using Cinnamon.Web.Modules.ApiAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;

namespace Cinnamon.Web.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ActivityController : Controller 
{
    private readonly IActivityApiHandler activityApiHandler;

    public ActivityController(IActivityApiHandler activityApiHandler)
    {
        this.activityApiHandler = activityApiHandler;
    }

    [Route("UploadActivityImage")]
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UploadActivityImage([FromForm] UploadActivityImages args)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Please provide required fields" });
            }

            var token = User.FindFirstValue("Token");
            if(token == null)
            {
                return Json(new { success = false, message = "Unable to identify current user" });
            }

            var result = await activityApiHandler.UploadActivityImages(new Framework.ApiCommand.ApiCore.Activity.Request.UploadActivityImageArgs {
                ActivityId = args.ActivityId,
                Image1 = args.Image1,
                Image2 = args.Image2,
                Image3 = args.Image3
            }, token);

            if(!result.Succeeded || result.Result == null)
            {
                return Json(new { success = false, message = result.Message });
            }

            if(result.Succeeded && !result.Result.IsSuccess)
            {
                return Json(new { success = false, message = result.Result.ErrorInfo?.Message });
            }

            return Json(new { success = true, message = "Successfully uploaded" });
        }
        catch
        {
            return Json(new { success = false, message = "An error occured please try again later" });
        }
    }

    [Route("Create")]
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateActivity([FromForm] CreateActivityArgs args)
    {
        try
        {
            var image1 = args.Image1;
            var image2 = args.Image2;
            var image3 = args.Image3;

            args.Image1 = null;
            args.Image2 = null;
            args.Image3 = null;

            var token = User.FindFirstValue("Token");
            if (token == null)
            {
                return Json(new { success = false, message = "Unable to identify current user" });
            }

            var result = await activityApiHandler.CreateActivity(args, token);

            if (!result.Succeeded || result.Result == null)
            {
                return Json(new { success = false, message = result.Message });
            }

            if (result.Succeeded && !result.Result.IsSuccess)
            {
                return Json(new { success = false, message = result.Result.ErrorInfo?.Message });
            }

            var uploadResult = await activityApiHandler.UploadActivityImages(new Framework.ApiCommand.ApiCore.Activity.Request.UploadActivityImageArgs
            {
                ActivityId = result.Result.Result.ActivityId,
                Image1 = image1,
                Image2 = image2,
                Image3 = image3
            }, token);

            return Json(new { success = true, message = "Successfully created activity" });
        }
        catch
        {
            return Json(new { success = false, message = "An error occured please try again later" });
        }
    }

    [Route("Update")]
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UpdateActivity([FromForm] UpdateActivityArgs args)
    {
        try
        {
            var image1 = args.Image1;
            var image2 = args.Image2;
            var image3 = args.Image3;

            args.Image1 = null;
            args.Image2 = null;
            args.Image3 = null;

            var token = User.FindFirstValue("Token");
            if (token == null)
            {
                return Json(new { success = false, message = "Unable to identify current user" });
            }

            var result = await activityApiHandler.UpdateActivity(args, token);

            if (!result.Succeeded || result.Result == null)
            {
                return Json(new { success = false, message = result.Message });
            }

            if (result.Succeeded && !result.Result.IsSuccess)
            {
                return Json(new { success = false, message = result.Result.ErrorInfo?.Message });
            }

            var uploadResult = await activityApiHandler.UploadActivityImages(new Framework.ApiCommand.ApiCore.Activity.Request.UploadActivityImageArgs
            {
                ActivityId = result.Result.Result.ActivityId,
                Image1 = image1,
                Image2 = image2,
                Image3 = image3
            }, token);

            return Json(new { success = true, message = "Successfully updated activity" });
        }
        catch
        {
            return Json(new { success = false, message = "An error occured please try again later" });
        }
    }
}