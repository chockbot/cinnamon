using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.Common;
using Entities = Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.FamilyMember;

namespace Cinnamon.Api.Data.Services.Repository.FamilyMember;

public class FamilyMemberRepository : IFamilyMemberRepository
{
    private readonly IDataStore dataStore;

    public FamilyMemberRepository(IDataStore dataStore)
    {
        this.dataStore = dataStore;
    }

    public async Task<AppResult<FamilyMemberDTO>> Create(int customerId, string name, string gender, string birthmonth, string birthyear)
    {
        try
        {
            var familyMember = new Entities.FamilyMember 
            {
                BirthMonth = birthmonth,
                BirthYear = birthyear,
                CustomerId = customerId,
                Gender = gender,
                Name = name,
            };

            var result = await dataStore.FamilyMember.Add(familyMember);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<FamilyMemberDTO>.CreateFailed(
                    new ApplicationException("An error occured when creating family member"), "An error occured when creating family member");
            }

            return AppResult<FamilyMemberDTO>.CreateSucceeded(new FamilyMemberDTO {
                BirthMonth = birthmonth,
                BirthYear = birthyear,
                Gender = gender,
                Id = result.Result.Id,
                Name = name
            },"Successfully created family member");
        }
        catch (Exception ex)
        {
            return AppResult<FamilyMemberDTO>.CreateFailed(ex, "An error occured when creating family member");
        }
    }

    public async Task<AppResult<IEnumerable<FamilyMemberDTO>>> Create(int customerId, IEnumerable<FamilyMemberDTO> familyMembers)
    {
        try
        {
            // check customer if existed
            var customerRes = await dataStore.Customer.GetByIdAsync(customerId);
            if(!customerRes.Succeeded || customerRes.Result == null)
            {
                return AppResult<IEnumerable<FamilyMemberDTO>>.CreateFailed(
                    new ApplicationException("Can't find customer id provided"), "Can't find customer id provided");
            }

            var members = familyMembers.Select(f => {
                return new Entities.FamilyMember {
                    BirthMonth = f.BirthMonth,
                    BirthYear = f.BirthYear,
                    CustomerId = customerId,
                    Gender = f.Gender,
                    Name = f.Name,
                };
            });

            var createdRes = await dataStore.FamilyMember.AddRange(members);
            if(!createdRes.Succeeded || createdRes.Result == null)
            {
                return AppResult<IEnumerable<FamilyMemberDTO>>.CreateFailed(
                    new ApplicationException("An error occured when creating family members"), "An error occured when creating family members");
            }

            var createdMembers = createdRes.Result.Select(f => {
                return new FamilyMemberDTO {
                    BirthMonth = f.BirthMonth,
                    BirthYear = f.BirthYear,
                    Gender = f.Gender,
                    Id = f.Id,
                    Name = f.Name
                };
            });

            return AppResult<IEnumerable<FamilyMemberDTO>>.CreateSucceeded(createdMembers, "Successfully created family members");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<FamilyMemberDTO>>.CreateFailed(ex, "An error occured when creating family members");
        }
    }

    public async Task<AppResult<IEnumerable<FamilyMemberDTO>>> GetAllAsync(int? count, int? skip)
    {
        try
        {
            var result = await dataStore.FamilyMember.FindAsync(f => true, count, skip);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<FamilyMemberDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }

            var members = result.Result.Select(f => {
                return new FamilyMemberDTO {
                    BirthMonth = f.BirthMonth,
                    BirthYear = f.BirthYear,
                    Gender = f.Gender,
                    Name = f.Name,
                    Id = f.Id
                };
            });

            return AppResult<IEnumerable<FamilyMemberDTO>>.CreateSucceeded(members, "Sucessfully getting family members");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<FamilyMemberDTO>>.CreateFailed(ex, "An error occured when getting family members");
        }
    }

    public async Task<AppResult<IEnumerable<FamilyMemberDTO>>> GetAllAsync()
    {
        try
        {
            var result = await dataStore.FamilyMember.GetAllAsync();
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<FamilyMemberDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }

            var members = result.Result.Select(f => {
                return new FamilyMemberDTO {
                    BirthMonth = f.BirthMonth,
                    BirthYear = f.BirthYear,
                    Gender = f.Gender,
                    Name = f.Name,
                    Id = f.Id
                };
            });

            return AppResult<IEnumerable<FamilyMemberDTO>>.CreateSucceeded(members, "Sucessfully getting family members");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<FamilyMemberDTO>>.CreateFailed(ex, "An error occured when getting family members");
        }
    }

    public async Task<AppResult<FamilyMemberDTO>> GetByIdAsync(int id)
    {
        try
        {
            var result = await dataStore.FamilyMember.GetByIdAsync(id);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<FamilyMemberDTO>.CreateFailed(
                    new ApplicationException("Can't find family member by id"), "Can't find family member by id");
            }

            return AppResult<FamilyMemberDTO>.CreateSucceeded(new FamilyMemberDTO {
                BirthMonth = result.Result.BirthMonth,
                BirthYear = result.Result.BirthYear,
                Gender = result.Result.Gender,
                Name = result.Result.Name,
                Id = result.Result.Id
            }, "Successfully getting family member by id");
        }
        catch (Exception ex)
        {
            return AppResult<FamilyMemberDTO>.CreateFailed(ex, "An error occured when getting family member by id");
        }
    }

    public async Task<AppResult<FamilyMemberDTO>> Update(int familyMemberId, string? name, string? gender, string? birthmonth, string? birthyear)
    {
        try
        {
            // check family member id
            var result = await dataStore.FamilyMember.GetByIdAsync(familyMemberId);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<FamilyMemberDTO>.CreateFailed(
                    new ApplicationException("Can't find family member to update"), "Can't find family member to update");
            }
            var member = result.Result;

            member.Name = name ?? member.Name;
            member.Gender = gender ?? member.Gender;
            member.BirthMonth = birthmonth ?? member.BirthMonth;
            member.BirthYear = birthyear ?? member.BirthYear;

            var updatedRes = await dataStore.FamilyMember.Update(member);
            if(!updatedRes.Succeeded || updatedRes.Result == null)
            {
                return AppResult<FamilyMemberDTO>.CreateFailed(
                    new ApplicationException("An error occured when updating family member"), "An error occured when updating family member");
            }

            return AppResult<FamilyMemberDTO>.CreateSucceeded(new FamilyMemberDTO {
                BirthMonth =  updatedRes.Result.BirthMonth,
                BirthYear = result.Result.BirthYear,
                Gender = result.Result.Gender,
                Name = result.Result.Name,
                Id = result.Result.Id
            }, "Successfully updated family member");
        }
        catch (Exception ex)
        {
            return AppResult<FamilyMemberDTO>.CreateFailed(ex, "An error occured when updating family member");
        }
    }

    public async Task<AppResult<IEnumerable<FamilyMemberDTO>>> Update(IEnumerable<FamilyMemberDTO> familyMembers)
    {
        try
        {
            var members = familyMembers.Select(f => {
                return new Entities.FamilyMember {
                    BirthMonth = f.BirthMonth,
                    BirthYear = f.BirthYear,
                    Id = f.Id,
                    Gender = f.Gender,
                    Name = f.Name
                };
            });

            var updatedRes = await dataStore.FamilyMember.UpdateRange(members);
            if(!updatedRes.Succeeded || updatedRes.Result == null)
            {
                return AppResult<IEnumerable<FamilyMemberDTO>>.CreateFailed(
                    new ApplicationException("An error occured when updating all entities"), "An error occured when updating all entities");
            }

            var updatedDTO = updatedRes.Result.Select(f => {
                return new FamilyMemberDTO {
                    BirthMonth = f.BirthMonth,
                    BirthYear = f.BirthYear,
                    Gender = f.Gender,
                    Name = f.Name,
                    Id = f.Id
                };
            });

            return AppResult<IEnumerable<FamilyMemberDTO>>.CreateSucceeded(updatedDTO, "Successfully updated family members");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<FamilyMemberDTO>>.CreateFailed(ex, "An error occured when updating all entities");
        }
    }

    public async Task<AppResult<bool>> DeleteFamilyMembers(IEnumerable<int> ids)
    {
        try
        {
            var familyRes = await dataStore.FamilyMember.FindAsync(f => ids.Contains(f.Id), null, null);
            if(!familyRes.Succeeded || familyRes.Result == null)
            {
                return AppResult<bool>.CreateFailed(
                    new ApplicationException("An error occured when deleting family member records"), "An error occured when deleting family member records");
            }

            if(familyRes.Result.Count() == 0)
            {
                return AppResult<bool>.CreateFailed(new ApplicationException("Family member ids not found"), "Family member ids not found");
            }

            var result = await dataStore.FamilyMember.RemoveRange(familyRes.Result);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<bool>.CreateFailed(
                    new ApplicationException("An error occured when deleting family member records"), "An error occured when deleting family member records");
            }

            return AppResult<bool>.CreateSucceeded(true, "Successfully deleted family members");
        }
        catch (Exception ex)
        {
            return AppResult<bool>.CreateFailed(ex, "An error occured when deleting family members");
        }
    }
}