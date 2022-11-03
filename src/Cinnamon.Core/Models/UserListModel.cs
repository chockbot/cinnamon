using Cinnamon.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Core
{
    public class UserListModel : BaseModel
    {
        public int Id { get; set; }
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "The Field Email contains invalid characters.")]
        public string Email { get; set; }
        [Required]
        public UserType Type { get; set; } = UserType.Maker;
        [Required(ErrorMessage="Please enter your First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter your Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please enter your Birthdate")]
        public string Birthdate { get; set; }
        [Required(ErrorMessage ="Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password",ErrorMessage ="The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public bool AcceptFlag { get; set; } = false;
    }
}
