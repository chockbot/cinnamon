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
    private readonly IGetDynamicContentHandler getDynamicContentHandler;

    public SystemController(IGetSystemDateHandler getSystemDateHandler, IGetAnnouncementsHandler getAnnouncementsHandler,
        IGetDynamicContentHandler getDynamicContentHandler)
    {
        this.getSystemDateHandler = getSystemDateHandler;
        this.getAnnouncementsHandler = getAnnouncementsHandler;
        this.getDynamicContentHandler = getDynamicContentHandler;
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

    [AllowAnonymous]
    [Route("GetEventPolicies")]
    [HttpGet]
    [ProducesResponseType(typeof(GetEventPoliciesResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEventPolicies()
    {
        try
        {
            var result = await getDynamicContentHandler.ExecuteAsync(new Services.AdminService.Interactors.GetDynamicContentArgs {
                Identifier = "event-policies"
            });

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetEventPoliciesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            var content = result.Result;

            return new JsonResult(new GetEventPoliciesResult 
                {
                    IsSuccess = true, 
                    Result = new Framework.ApiCommand.ApiCore.DTO.DynamicContent.DynamicContentDTO {
                        Content = content.Content,
                        DateLastUpdated = content.DateLastUpdated,
                        Description = content.Description,
                        Id = content.Id,
                        Identifier = content.Identifier,
                        Title = content.Title
                    }
                } );
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetEventPoliciesResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [AllowAnonymous]
    [Route("GetEventBuyerPolicies")]
    [HttpGet]
    [ProducesResponseType(typeof(GetEventBuyerPoliciesResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEventBuyerPolicies()
    {
        try
        {
            var result = await getDynamicContentHandler.ExecuteAsync(new Services.AdminService.Interactors.GetDynamicContentArgs {
                Identifier = "event-buyer-policies"
            });

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetEventBuyerPoliciesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            var content = result.Result;

            return new JsonResult(new GetEventBuyerPoliciesResult 
                {
                    IsSuccess = true, 
                    Result = new Framework.ApiCommand.ApiCore.DTO.DynamicContent.DynamicContentDTO {
                        Content = content.Content,
                        DateLastUpdated = content.DateLastUpdated,
                        Description = content.Description,
                        Id = content.Id,
                        Identifier = content.Identifier,
                        Title = content.Title
                    }
                } );
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetEventBuyerPoliciesResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [AllowAnonymous]
    [Route("GetEventSellerPolicies")]
    [HttpGet]
    [ProducesResponseType(typeof(GetEventSellerPoliciesResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEventSellerPolicies()
    {
        try
        {
            var result = await getDynamicContentHandler.ExecuteAsync(new Services.AdminService.Interactors.GetDynamicContentArgs {
                Identifier = "event-seller-policies"
            });

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetEventSellerPoliciesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            var content = result.Result;

            return new JsonResult(new GetEventSellerPoliciesResult 
                {
                    IsSuccess = true, 
                    Result = new Framework.ApiCommand.ApiCore.DTO.DynamicContent.DynamicContentDTO {
                        Content = content.Content,
                        DateLastUpdated = content.DateLastUpdated,
                        Description = content.Description,
                        Id = content.Id,
                        Identifier = content.Identifier,
                        Title = content.Title
                    }
                } );
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetEventSellerPoliciesResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [AllowAnonymous]
    [Route("GetPrivacyPolicies")]
    [HttpGet]
    [ProducesResponseType(typeof(GetPrivacyPoliciesResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPrivacyPolicies()
    {
        try
        {
            var result = await getDynamicContentHandler.ExecuteAsync(new Services.AdminService.Interactors.GetDynamicContentArgs {
                Identifier = "privacy-policies"
            });

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetPrivacyPoliciesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            var content = result.Result;

            return new JsonResult(new GetPrivacyPoliciesResult 
                {
                    IsSuccess = true, 
                    Result = new Framework.ApiCommand.ApiCore.DTO.DynamicContent.DynamicContentDTO {
                        Content = content.Content,
                        DateLastUpdated = content.DateLastUpdated,
                        Description = content.Description,
                        Id = content.Id,
                        Identifier = content.Identifier,
                        Title = content.Title
                    }
                } );
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetPrivacyPoliciesResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }
}