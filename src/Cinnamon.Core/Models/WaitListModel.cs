using Cinnamon.Core.Enums;

namespace Cinnamon.Core
{
    public class WaitListModel : BaseModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public UserType Type { get; set; } = UserType.Maker;
    }
}
