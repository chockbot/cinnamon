using AutoMapper;
using Cinnamon.Api.Core.Services.DirectStudentService.Handlers;
using Cinnamon.Framework.ApiCommand.ApiCore;
using Cinnamon.Framework.ApiCommand.ApiCore.DirectStudent.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.DirectStudent.Response;
using Cinnamon.Framework.ApiCommand.ApiCore.DTO.DirectStudents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Core.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DirectStudentsController : ControllerBase 
{
    private readonly IDirectStudentsInfoHandler directStudentsInfoHandler;
    private readonly IMapper mapper;

    public DirectStudentsController(IDirectStudentsInfoHandler directStudentsInfoHandler, IMapper mapper)
    {
        this.directStudentsInfoHandler = directStudentsInfoHandler;
        this.mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(DirectStudentInfoReult), StatusCodes.Status200OK)]   
    public async Task<IActionResult> Index([FromQuery] DirectStudentInfoArgs args)
    {
        try
        {
            var result = await directStudentsInfoHandler.ExecuteAsync(new Services.DirectStudentService.Interactors.DirectStudentsInfosArgs {
                PageCount = args.CountPerPage,
                PageIndex = args.PageIndex
            });
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new DirectStudentInfoReult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }

            var mapped = mapper.Map<IEnumerable<DirectStudentInfoDTO>>(result.Result.DirectStudentInfos);

            return new JsonResult(new DirectStudentInfoReult {
                IsSuccess = true,
                Result = mapped
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new DirectStudentInfoReult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }
}