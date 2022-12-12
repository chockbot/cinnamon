using Cinnamon.Api.Data.Services.Repository.ActivityDescription.DTO;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces
{
    public interface IDescriptionRepository
    {
        Task<AppResult<DescriptionDTO>> GetByIdAsync(int id);
        Task<AppResult<IEnumerable<DescriptionDTO>>> GetAllAsync(int? count, int? skip);
        Task<AppResult<DescriptionDTO>> UpdateDescription(int DescriptionId, string Description, string SpecificsYouWillProvide, string CustomerBringWithThem, string? AdditionalRequirements, string ActivityLevel, string SkillLevel, int MinimumAge,bool CanAdultsJoin);
        Task<AppResult<DescriptionDTO>> CreateDescription(int ActivityId, string Description, string SpecificsYouWillProvide, string CustomerBringWithThem, string? AdditionalRequirements, string ActivityLevel, string SkillLevel, int MinimumAge, bool CanAdultsJoin);
    }
}
