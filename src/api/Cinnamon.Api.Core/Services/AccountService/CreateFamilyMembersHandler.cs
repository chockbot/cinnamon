using System.Security.Claims;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AccountService;

public class CreateFamilyMembersHandler : ICreateFamilyMembersHandler
{
    private readonly IFamilyMemberData familyMemberData;
    private readonly IHttpContextAccessor httpContext;

    public CreateFamilyMembersHandler(IFamilyMemberData familyMemberData, IHttpContextAccessor httpContext)
    {
        this.familyMemberData = familyMemberData;
        this.httpContext = httpContext;
    }
    
    public AppResult<CreateFamilyMembersResult> Execute(CreateFamilyMembersArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<CreateFamilyMembersResult>.CreateFailed(ex, "An error occured in CreateFamilyMembersHandler");
        }
    }

    public async Task<AppResult<CreateFamilyMembersResult>> ExecuteAsync(CreateFamilyMembersArgs args)
    {
        try
        {
            // get customer id saved in claims
            var customerId = httpContext.HttpContext?.User.FindFirstValue("UserId");
            if(customerId == null)
            {
                return AppResult<CreateFamilyMembersResult>.CreateFailed(
                    new ApplicationException("Unable to determine current account login"), "Unable to determine current account login");
            }
            int id = Convert.ToInt32(customerId);

            var result = await familyMemberData.CreateManyFamilyMember(new Framework.ApiCommand.ApiData.FamilyMember.Request.CreateManyFamilyMemberArgs {
                CustomerId = id,
                FamilyMembers = args.FamilyMembers.Select(f => {
                    return new Framework.ApiCommand.ApiData.FamilyMember.Request.CreateManyFamilyMemberArgs.CreateFamilyMember {
                        BirthMonth = f.BirthMonth,
                        BirthYear = f.BirthYear,
                        Gender = f.Gender,
                        Name = f.Name
                    };
                })
            });

            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<CreateFamilyMembersResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if(result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<CreateFamilyMembersResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in CreateFamilyMembersHandler");
            }

            return AppResult<CreateFamilyMembersResult>.CreateSucceeded(new CreateFamilyMembersResult {
                FamilyMembers = result.Result.Result.Select(f => {
                    return new CreateFamilyMembersResult.FamilyMember {
                        BirthMonth = f.BirthMonth,
                        BirthYear = f.BirthYear,
                        Gender = f.Gender,
                        Id = f.Id,
                        Name = f.Name,
                        CustomerId = id
                    };
                })
            }, "Successfully created family members");
        }
        catch (Exception ex)
        {
            return AppResult<CreateFamilyMembersResult>.CreateFailed(ex, "An error occured in CreateFamilyMembersHandler");
        }
    }
}