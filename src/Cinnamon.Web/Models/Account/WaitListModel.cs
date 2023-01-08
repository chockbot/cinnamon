using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Web.Models.Account
{
    public class WaitListModel
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "The Field Email contains invalid characters.")]
        public string Email { get; set; }
        public string Guid { get; set; }
        public string Token { get; set; }
        public bool IsVerified { get; set; }
    }
}
