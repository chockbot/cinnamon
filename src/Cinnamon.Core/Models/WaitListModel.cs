using Cinnamon.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Core
{
    public class WaitListModel : BaseModel
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "The Field Email contains invalid characters.")]
        public string Email { get; set; }
        [Required]
        public UserType Type { get; set; } = UserType.Maker;
    }
}
