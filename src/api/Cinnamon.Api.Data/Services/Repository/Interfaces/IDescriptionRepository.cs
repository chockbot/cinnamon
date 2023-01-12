using Cinnamon.Framework.Common;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Description;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces
{
    public interface IDescriptionRepository
    {
        Task<AppResult<DescriptionDTO>> GetByIdAsync(int id);
        Task<AppResult<DescriptionDTO>> GetByActivityIdAsync(int id);
        Task<AppResult<IEnumerable<DescriptionDTO>>> GetAllAsync();
        Task<AppResult<DescriptionDTO>> UpdateDescription(int DescriptionId, string Description, string SpecificsYouWillProvide, string CustomerBringWithThem, string? AdditionalRequirements, string ActivityLevel, string SkillLevel, int MinimumAge,bool CanAdultsJoin);
        Task<AppResult<DescriptionDTO>> CreateDescription(int ActivityId, string Description, string SpecificsYouWillProvide, string CustomerBringWithThem, string? AdditionalRequirements, string ActivityLevel, string SkillLevel, int MinimumAge, bool CanAdultsJoin);
    }
}
