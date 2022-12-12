using Microsoft.AspNetCore.Mvc;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Api.Data.Models.FamilyMember.Request;
using Cinnamon.Api.Data.Models.FamilyMember.Response;
using Cinnamon.Api.Data.Services.Repository.FamilyMember.DTO;

namespace Cinnamon.Api.Data.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FamilyMemberController : ControllerBase 
{
    private readonly IFamilyMemberRepository familyMemberRepository;

    public FamilyMemberController(IFamilyMemberRepository familyMemberRepository)
    {
        this.familyMemberRepository = familyMemberRepository;
    }

    [Route("GetFamilyMemberById/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetFamilyMemberResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFamilyMemberById(int id)
    {
        try
        {
            var result = await familyMemberRepository.GetByIdAsync(id);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetFamilyMemberResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetFamilyMemberResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetFamilyMemberResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetAllFamilyMembers")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllFamilyMemberResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllFamilyMembers([FromQuery] GetAllFamilyMemberArgs args)
    {
        try
        {
            var result =
                args.PageIndex.HasValue && args.CountPerPage.HasValue ?
                await familyMemberRepository.GetAllAsync(args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage) :
                await familyMemberRepository.GetAllAsync();

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllFamilyMemberResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
            }

            // get all without pagination to get all rows
            var all = args.PageIndex.HasValue && args.CountPerPage.HasValue ?
                await familyMemberRepository.GetAllAsync(null, null) :
                await familyMemberRepository.GetAllAsync();

            if (!all.Succeeded || all.Result == null)
            {
                return new JsonResult(new GetAllFamilyMemberResult { ErrorInfo = new Models.ErrorInfo { Message = all.Message } });
            }

            var totalRecords = all.Result.Count();
            return new JsonResult(new GetAllFamilyMemberResult
            {
                Result = result.Result,
                IsSuccess = true,
                Pagination = new Models.Pagination
                {
                    PageIndex = args.PageIndex,
                    PerPage = args.CountPerPage,
                    TotalRecords = totalRecords,
                    TotalPages = args.CountPerPage.HasValue && args.PageIndex.HasValue ?
                                (int)Math.Ceiling(Convert.ToDouble(totalRecords / args.CountPerPage.Value)) : null
                }
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetAllFamilyMemberResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CreateFamilyMember")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateFamilyMemberResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateFamilyMember([FromBody] CreateFamilyMemberArgs args)
    {
        try
        {
            var result = await familyMemberRepository.Create(args.CustomerId, args.Name, args.Gender, args.BirthMonth, args.BirthYear);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreateFamilyMemberResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateFamilyMemberResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateFamilyMemberResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CreateManyFamilyMember")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateManyFamilyMemberResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateManyFamilyMember([FromBody] CreateManyFamilyMemberArgs args)
    {
        try
        {
            var members = args.FamilyMembers.Select(f => {
                return new FamilyMemberDTO {
                    BirthMonth = f.BirthMonth,
                    BirthYear = f.BirthYear,
                    Gender = f.Gender,
                    Name = f.Name
                };
            });
            var result = await familyMemberRepository.Create(args.CustomerId, members);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreateManyFamilyMemberResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateManyFamilyMemberResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateManyFamilyMemberResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UpdateFamilyMember")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateFamilyMemberResult), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> UpdateFamilyMember([FromBody] UpdateFamilyMemberArgs args)
    {
        try
        {
            var result = await familyMemberRepository.Update(args.FamilyMemberId, args.Name, args.Gender, args.BirthMonth, args.BirthYear);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateFamilyMemberResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateFamilyMemberResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateFamilyMemberResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UpdateManyFamilyMember")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateManyFamilyMembersResult), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> UpdateManyFamilyMember([FromBody] UpdateManyFamilyMemberArgs args)
    {
        try
        {
            var members = args.FamilyMembers.Select(f => {
                FamilyMemberDTO member = new();

                member.BirthMonth = f.BirthMonth ?? member.BirthMonth;
                member.BirthYear = f.BirthYear ?? member.BirthYear;
                member.Gender = f.Gender ?? member.Gender;
                member.Name = f.Name ?? member.Name;
                member.Id = f.FamilyMemberId;

                return member;
            });
            var result = await familyMemberRepository.Update(members);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateManyFamilyMembersResult { ErrorInfo = new Models.ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateManyFamilyMembersResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateManyFamilyMembersResult { ErrorInfo = new Models.ErrorInfo { Message = ex.Message } });
        }
    }
}