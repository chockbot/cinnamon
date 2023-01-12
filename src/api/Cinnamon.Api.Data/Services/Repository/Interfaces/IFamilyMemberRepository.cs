using Cinnamon.Framework.ApiCommand.ApiData.DTO.FamilyMember;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IFamilyMemberRepository 
{
    Task<AppResult<FamilyMemberDTO>> GetByIdAsync(int id);
    Task<AppResult<IEnumerable<FamilyMemberDTO>>> GetByCustomerIdAsync(int id);
    Task<AppResult<IEnumerable<FamilyMemberDTO>>> GetAllAsync(int? count, int? skip);
    Task<AppResult<IEnumerable<FamilyMemberDTO>>> GetAllAsync();
    Task<AppResult<FamilyMemberDTO>> Create(int customerId, string name, string gender, string birthmonth, int birthyear);
    Task<AppResult<IEnumerable<FamilyMemberDTO>>> Create(int customerId, IEnumerable<FamilyMemberDTO> familyMembers);
    Task<AppResult<FamilyMemberDTO>> Update(int familyMemberId, string? name, string? gender, string? birthmonth, int? birthyear);
    Task<AppResult<IEnumerable<FamilyMemberDTO>>> Update(IEnumerable<FamilyMemberDTO> familyMembers);
    Task<AppResult<bool>> DeleteFamilyMembers(IEnumerable<int> ids);
}