using Cinnamon.Framework.ApiCommand.ApiData.FamilyMember.Response;
using Cinnamon.Framework.ApiCommand.ApiData.FamilyMember.Request;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface IFamilyMemberData 
{
    Task<AppResult<GetFamilyMemberResult>> GetFamilyMemberById(int id);
    Task<AppResult<GetFamilyMemberByCustomerIdResult>> GetFamilyMemberByCustomerId(int id);
    Task<AppResult<GetAllFamilyMemberResult>> GetAllFamilyMembers(GetAllFamilyMemberArgs args);
    Task<AppResult<CreateFamilyMemberResult>> CreateFamilyMember(CreateFamilyMemberArgs args);
    Task<AppResult<CreateManyFamilyMemberResult>> CreateManyFamilyMember(CreateManyFamilyMemberArgs args);
    Task<AppResult<UpdateFamilyMemberResult>> UpdateFamilyMember(UpdateFamilyMemberArgs args);
    Task<AppResult<UpdateManyFamilyMembersResult>> UpdateManyFamilyMember(UpdateManyFamilyMemberArgs args);
    Task<AppResult<DeleteManyFamilyMembersResult>> DeleteManyFamilyMembers(DeleteManyFamilyMembersArgs args);
}