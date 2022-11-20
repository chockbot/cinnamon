using Cinnamon.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinnamon.Core
{
    public class ProfileModel : BaseModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }   
        public string ImageName { get; set; } = "";
        public long ImageSize { get; set; }
        public string ImageLocation { get; set; } = "";
    }
}
