namespace Cinnamon.Api.Core.Services.PaymentGatewayService.Resolver;

public class PGDriverResolver 
{
    public Type ResolvePgDriver(string interactorHandler, string driverClassName)
    {
        var interactorHandlerInterface = Type.GetType($"Cinnamon.Api.Core.Services.PaymentGatewayService.Handlers.{interactorHandler}");
        var driverClassInterface = Type.GetType($"Cinnamon.Api.Core.Services.PaymentGatewayService.Handlers.{driverClassName}");

        if(interactorHandlerInterface == null || driverClassInterface == null)
        {
            throw new ApplicationException("Can't resolve payment gateway driver");
        }

        var pgDriverType = AppDomain.CurrentDomain.
                            GetAssemblies().SelectMany(e => e.GetTypes()).
                            Where(p => interactorHandlerInterface.IsAssignableFrom(p) && driverClassInterface.IsAssignableFrom(p)).
                            FirstOrDefault();

        if(pgDriverType == null)
        {
            throw new ApplicationException("Can't resolve payment gateway driver");
        }
        
        return pgDriverType;
    }
}