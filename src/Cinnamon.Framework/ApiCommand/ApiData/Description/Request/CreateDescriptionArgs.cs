using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Description.Request
{
    public class CreateDescriptionArgs
    {
        [Required]
        public int ActivityId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string SpecificsYouWillProvide { get; set; }
        [Required]
        public string CustomerBringWithThem { get; set; }
        public string? AdditionalRequirements { get; set; }
        [Required]
        public string ActivityLevel { get; set; }
        [Required]
        public string SkillLevel { get; set; }
        [Required]
        public int MinimumAge { get; set; }
        [Required]
        public bool CanAdultsJoin { get; set; }
        [Required]
        public string? ClassPolicies { get; set; }
    }
}
