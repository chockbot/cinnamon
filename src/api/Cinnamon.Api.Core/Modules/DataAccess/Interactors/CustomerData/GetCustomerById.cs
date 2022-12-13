namespace Cinnamon.Api.Core.Modules.DataAccess.Interactors.CustomerData;

public class GetCustomerByIdArgs 
{
    public int CustomerId {get; set;}
}

public class GetCustomerByIdResult : AbstractModel<Customer> 
{
}