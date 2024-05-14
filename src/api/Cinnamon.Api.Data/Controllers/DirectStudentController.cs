using AutoMapper;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.DirectStudent.Request;
using Cinnamon.Framework.ApiCommand.ApiData.DirectStudent.Response;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.DirectStudent;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Data.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DirectStudentController : ControllerBase
{
    private readonly IDirectStudentRepository directStudentRepository;
    private readonly IMapper mapper;

    public DirectStudentController(IDirectStudentRepository directStudentRepository, IMapper mapper)
    {
        this.directStudentRepository = directStudentRepository;
        this.mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateDirectStudentsResult), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDirectStudents([FromBody] CreateDirectStudentsArgs args)
    {
        try
        {
            var dtos = mapper.Map<IEnumerable<DirectStudentDTO>>(args.CreateDirectStudents);

            var result = await directStudentRepository.CreateDirectStudents(dtos);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new CreateDirectStudentsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateDirectStudentsResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateDirectStudentsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}