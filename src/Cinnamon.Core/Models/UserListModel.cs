using Cinnamon.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Core
{
    public class UserListModel : BaseModel
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "The Field Email contains invalid characters.")]
        public string Email { get; set; }
        [Required]
        public UserType Type { get; set; } = UserType.Maker;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Birthdate { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public bool AcceptFlag { get; set; } = false;
    }
}
