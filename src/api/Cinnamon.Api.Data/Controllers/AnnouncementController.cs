using AutoMapper;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.Announcement.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Announcement.Response;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Announcement;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Data.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnnouncementsController : ControllerBase 
{
    private readonly IAnnouncementRepository announcementRepository;
    private readonly IMapper mapper;

    public AnnouncementsController(IAnnouncementRepository announcementRepository, IMapper mapper)
    {
        this.announcementRepository = announcementRepository;
        this.mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetAnnouncementsResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Index()
    {
        try
        {
            var result = await announcementRepository.GetAllAnnouncements();
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new GetAnnouncementsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            
            return new JsonResult(new GetAnnouncementsResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetAnnouncementsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateAnnouncementResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Index([FromBody] CreateAnnouncementArgs args)
    {
        try
        {
            var dto = mapper.Map<AnnouncementDTO>(args);

            var result = await announcementRepository.CreateAnnouncement(dto);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new CreateAnnouncementResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateAnnouncementResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateAnnouncementResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [HttpPost]
    [Route("DeleteAnnouncement")]
    [ProducesResponseType(typeof(DeleteAnnouncementResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAnnouncement([FromBody] DeleteAnnouncementArgs args)
    {
        try
        {
            var dto = mapper.Map<AnnouncementDTO>(args);

            var result = await announcementRepository.DeleteAnnouncement(dto);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new DeleteAnnouncementResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new DeleteAnnouncementResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new DeleteAnnouncementResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [HttpPost]
    [Route("UpdateAnnouncement")]
    [ProducesResponseType(typeof(UpdateAnnouncementResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAnnouncement([FromBody] UpdateAnnouncementArgs args)
    {
        try
        {
            var dto = mapper.Map<AnnouncementDTO>(args);

            var result = await announcementRepository.UpdateAnnouncement(dto);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new UpdateAnnouncementResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateAnnouncementResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateAnnouncementResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}