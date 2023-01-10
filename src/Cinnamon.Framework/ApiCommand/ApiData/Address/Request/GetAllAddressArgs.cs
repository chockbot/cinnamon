namespace Cinnamon.Framework.ApiCommand.ApiData.Address.Request;
public class GetAllAddressArgs
{
    public bool? IsActive { get; set; }
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
}
