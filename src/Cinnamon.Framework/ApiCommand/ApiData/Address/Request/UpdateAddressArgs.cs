using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Address.Request
{
    public class UpdateAddressArgs
    {
        [Required]
        public int AddressId { get; set; }
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
