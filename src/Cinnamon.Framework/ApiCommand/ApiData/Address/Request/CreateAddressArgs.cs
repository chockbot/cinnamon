using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Address.Request
{
    public class CreateAddressArgs
    {
        [Required]
        public int ActivityId { get; set; }
        [Required]
        public string Address1 { get; set; }
        [Required]
        public string Address2 { get; set; }
        [Required]
        public string District { get; set; }
        [Required]
        public string City { get; set; }
    }
}
