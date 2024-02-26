using Cinnamon.Framework.ApiCommand.ApiCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Cinnamon.Framework.ApiCommand.ApiCore.System.Response;
using Cinnamon.Api.Core.Services.SystemService.Handlers;
using Cinnamon.Api.Core.Services.AdminService.Handlers;

namespace Cinnamon.Api.Core.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SystemController : ControllerBase
{
    private readonly IGetSystemDateHandler getSystemDateHandler;
    private readonly IGetAnnouncementsHandler getAnnouncementsHandler;

    public SystemController(IGetSystemDateHandler getSystemDateHandler, IGetAnnouncementsHandler getAnnouncementsHandler)
    {
        this.getSystemDateHandler = getSystemDateHandler;
        this.getAnnouncementsHandler = getAnnouncementsHandler;
    }

    [Route("GetServerDate")]
    [HttpGet]
    [ProducesResponseType(typeof(GetServerDateResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetServerDate()
    {
        try
        {
            var result = await getSystemDateHandler.ExecuteAsync(new Services.SystemService.Interactors.GetSystemDateArgs {});

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetServerDateResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetServerDateResult 
                {
                    IsSuccess = true, 
                    Result = result.Result.ServerDate
                } );
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetServerDateResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [AllowAnonymous]
    [Route("GetAnnouncements")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAnnouncementsResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAnnouncements()
    {
        try
        {
            var result = await getAnnouncementsHandler.ExecuteAsync(new Services.AdminService.Interactors.GetAnnouncementsArgs {
                Status = "published"
            });

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAnnouncementsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetAnnouncementsResult 
                {
                    IsSuccess = true, 
                    Result = result.Result.Announcements.Select(a => new Framework.ApiCommand.ApiCore.DTO.Announcement.AnnouncementDTO {
                        ButtonLabel = a.ButtonLabel,
                        Description = a.Description,
                        Id = a.Id,
                        Link = a.Link,
                        Status = a.Status,
                        Title = a.Title
                    })
                } );
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetAnnouncementsResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }
}