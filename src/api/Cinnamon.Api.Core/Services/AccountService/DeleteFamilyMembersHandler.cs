using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AccountService;

public class DeleteFamilyMembersHandler : IDeleteFamilyMembersHandler
{
    private readonly IFamilyMemberData familyMemberData;
    private readonly IGetFamilyMembersHandler getFamilyMembersHandler;

    public DeleteFamilyMembersHandler(IFamilyMemberData familyMemberData, IGetFamilyMembersHandler getFamilyMembersHandler)
    {
        this.familyMemberData = familyMemberData;
        this.getFamilyMembersHandler = getFamilyMembersHandler;
    }

    public AppResult<DeleteFamilyMembersResult> Execute(DeleteFamilyMembersArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<DeleteFamilyMembersResult>.CreateFailed(ex, "An error occured in DeleteFamilyMembersHandler");
        }
    }

    public async Task<AppResult<DeleteFamilyMembersResult>> ExecuteAsync(DeleteFamilyMembersArgs args)
    {
        try
        {
            var associatedMembers = await getFamilyMembersHandler.ExecuteAsync(new GetFamilyMembersArgs {});
            if(!associatedMembers.Succeeded || associatedMembers.Result == null)
            {
                return AppResult<DeleteFamilyMembersResult>.CreateFailed(new ApplicationException(associatedMembers.Message), associatedMembers.Message);
            }

            var ids = associatedMembers.Result.FamilyMembers.Where(i => args.Ids.Contains(i.Id)).Select(i => i.Id);
            if(ids.Count() <= 0)
            {
                return AppResult<DeleteFamilyMembersResult>.CreateFailed(new ApplicationException("Didn't find Ids need to delete"), "Didn't find Ids need to delete");
            }

            var result = await familyMemberData.DeleteManyFamilyMembers(new Framework.ApiCommand.ApiData.FamilyMember.Request.DeleteManyFamilyMembersArgs {
                Ids = ids
            });

            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<DeleteFamilyMembersResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if(result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<DeleteFamilyMembersResult>.CreateFailed(new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in DeleteFamilyMembersHandler");
            }

            return AppResult<DeleteFamilyMembersResult>.CreateSucceeded(new DeleteFamilyMembersResult {}, "Successfully deleted family members");
            
        }
        catch (Exception ex)
        {
            return AppResult<DeleteFamilyMembersResult>.CreateFailed(ex, "An error occured in DeleteFamilyMembersHandler");
        }
    }
}