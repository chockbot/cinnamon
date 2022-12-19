using System.Security.Claims;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.ApiCommand.ApiData.FamilyMember.Request;

namespace Cinnamon.Api.Core.Services.AccountService;

public class UpdateFamilyMembersHandler : IUpdateFamilyMembersHandler
{
    private readonly IFamilyMemberData familyMemberData;
    private readonly IHttpContextAccessor httpContext;
    private readonly IGetFamilyMembersHandler getFamilyMembersHandler;

    public UpdateFamilyMembersHandler(IFamilyMemberData familyMemberData, IHttpContextAccessor httpContext,
        IGetFamilyMembersHandler getFamilyMembersHandler)
    {
        this.familyMemberData = familyMemberData;
        this.httpContext = httpContext;
        this.getFamilyMembersHandler = getFamilyMembersHandler;
    }

    public AppResult<UpdateFamilyMembersResult> Execute(UpdateFamilyMembersArgs args)
    {
        throw new NotImplementedException();
    }

    public async Task<AppResult<UpdateFamilyMembersResult>> ExecuteAsync(UpdateFamilyMembersArgs args)
    {
        try
        {
            // get customer id saved in claims
            var customerId = httpContext.HttpContext?.User.FindFirstValue("UserId");
            int id = customerId != null ? Convert.ToInt32(customerId) : 0;

            // get all family members
            var getMembers = await getFamilyMembersHandler.ExecuteAsync(new GetFamilyMembersArgs{});
            if(!getMembers.Succeeded || getMembers.Result == null)
            {
                return AppResult<UpdateFamilyMembersResult>.CreateFailed(new ApplicationException(getMembers.Message), getMembers.Message);
            }
            var ids = getMembers.Result.FamilyMembers.Select(i => i.Id);

            // check id if associated to customer
            var associatedMembers = args.FamilyMembers
                                        .Where(i => ids.Contains(i.Id))
                                        .Select(f => {
                                            return new UpdateManyFamilyMemberArgs.UpdateFamilyMember {
                                                BirthMonth = f.BirthMonth,
                                                BirthYear = f.BirthYear,
                                                Id = f.Id,
                                                Gender = f.Gender,
                                                Name = f.Name,
                                                CustomerId = id
                                            };
                                        });

            var update = await familyMemberData.UpdateManyFamilyMember(new UpdateManyFamilyMemberArgs {
                FamilyMembers = associatedMembers
            });

            if(!update.Succeeded || update.Result == null)
            {
                return AppResult<UpdateFamilyMembersResult>.CreateFailed(new ApplicationException(update.Message), update.Message);
            }

            if(update.Succeeded && !update.Result.IsSuccess)
            {
                return AppResult<UpdateFamilyMembersResult>.CreateFailed(
                    new ApplicationException(update.Result.ErrorInfo?.Message), "An error occured in UpdateFamilyMembersHandler");
            }
            var updatedData = update.Result.Result.Select(f => {
                return new UpdateFamilyMembersResult.FamilyMember {
                    BirthMonth = f.BirthMonth,
                    BirthYear = f.BirthYear,
                    Gender = f.Gender,
                    Id = f.Id,
                    Name = f.Name
                };
            });

            return AppResult<UpdateFamilyMembersResult>.CreateSucceeded(new UpdateFamilyMembersResult {
                FamilyMembers = updatedData
            }, "Successfully updated family members");
        }
        catch (Exception ex)
        {
            return AppResult<UpdateFamilyMembersResult>.CreateFailed(ex, "An error occured in UpdateFamilyMembersHandler");
        }
    }
}