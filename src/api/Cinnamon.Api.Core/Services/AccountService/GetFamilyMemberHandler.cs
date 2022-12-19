using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;
using System.Security.Claims;

namespace Cinnamon.Api.Core.Services.AccountService;

public class GetFamilyMemberHandler : IGetFamilyMembersHandler
{
    private readonly IFamilyMemberData familyMemberData;
    private readonly IHttpContextAccessor httpContext;

    public GetFamilyMemberHandler(IFamilyMemberData familyMemberData, IHttpContextAccessor httpContext)
    {
        this.familyMemberData = familyMemberData;
        this.httpContext = httpContext;
    }

    public AppResult<GetFamilyMemberResult> Execute(GetFamilyMembersArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetFamilyMemberResult>.CreateFailed(ex, "An error occured in GetFamilyMemberHandler");
        }
    }

    public async Task<AppResult<GetFamilyMemberResult>> ExecuteAsync(GetFamilyMembersArgs args)
    {
        try
        {
            // get customer id saved in claims
            var customerId = httpContext.HttpContext?.User.FindFirstValue("UserId");
            int id = customerId != null ? Convert.ToInt32(customerId) : 0;

            var result = await familyMemberData.GetFamilyMemberByCustomerId(id);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<GetFamilyMemberResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if(result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetFamilyMemberResult>.CreateFailed(new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetFamilyMemberHandler");
            }

            var members = result.Result.Result.Select(i => {
                return new GetFamilyMemberResult.FamilyMember {
                    BirthMonth = i.BirthMonth,
                    BirthYear = i.BirthYear,
                    Gender = i.Gender,
                    Id = i.Id,
                    Name = i.Name
                };
            });

            return AppResult<GetFamilyMemberResult>.CreateSucceeded(new GetFamilyMemberResult {FamilyMembers = members}, "Successfully get family members");
        }
        catch (Exception ex)
        {
            return AppResult<GetFamilyMemberResult>.CreateFailed(ex, "An error occured in GetFamilyMemberHandler");
        }
    }
}