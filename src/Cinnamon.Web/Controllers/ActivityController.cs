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
    private readonly ISeatPlanApiHandler seatPlanApiHandler;

    public ActivityController(IActivityApiHandler activityApiHandler, ISeatPlanApiHandler seatPlanApiHandler)
    {
        this.activityApiHandler = activityApiHandler;
        this.seatPlanApiHandler = seatPlanApiHandler;
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
                Images = args.Images,
                DeletedIds = args.DeletedIds,
                Orders = args.Orders
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
            var token = User.FindFirstValue("Token");
            if (token == null)
            {
                return Json(new { success = false, message = "Unable to identify current user" });
            }

            foreach (var schedule in args?.ActivitySchedules)
            {
                schedule.SessionName = schedule.SessionName ?? "N/A";
                schedule.StartDate = schedule.StartDate ?? DateTime.MinValue;
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
            var token = User.FindFirstValue("Token");
            if (token == null)
            {
                return Json(new { success = false, message = "Unable to identify current user" });
            }

            foreach (var schedule in args?.ActivitySchedules)
            {
                schedule.SessionName = schedule.SessionName ?? "N/A";
                schedule.StartDate = schedule.StartDate ?? DateTime.MinValue;
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

            return Json(new { success = true, message = "Successfully updated activity" });
        }
        catch
        {
            return Json(new { success = false, message = "An error occured please try again later" });
        }
    }

    [Route("SeatPlan")]
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateSeatPlanTemplate([FromForm] CreateSeatPlanTemplate args)
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

            var createTemplateRes = await seatPlanApiHandler.CreateSeatPlanTemplate(new Framework.ApiCommand.ApiCore.SeatPlan.Request.CreateSeatPlanTemplateArgs {
                Name = args.Name,
                Address = args.Address,
                FormatterId = args.FormatterId,
                ImageFile = args.ImageFile,
                JsonFile = args.JsonFile
            }, token);  

            if(!createTemplateRes.Succeeded || createTemplateRes.Result == null || !createTemplateRes.Result.IsSuccess)
            {
                return Json(new { success = false, message = createTemplateRes.Result?.ErrorInfo?.Message ?? createTemplateRes.Message });
            }

            return Json(new { success = true, message = "Seat plan template created successfully" });
        }
        catch (System.Exception)
        {
            return Json(new { success = false, message = "An error occured please try again later" });
        }
    }
}