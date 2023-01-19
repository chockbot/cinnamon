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
        [Required]
        public string Subdivision { get; set; }
        [Required]
        public string Region { get; set; }
        [Required]
        public string Barangay { get; set; }
        [Required]
        public string PostalCode { get; set; }
    }
}
