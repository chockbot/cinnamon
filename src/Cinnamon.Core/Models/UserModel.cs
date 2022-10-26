using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Core
{
    public class UserModel : BaseModel
    {
        public WaitListModel waitList { get; set; }
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
